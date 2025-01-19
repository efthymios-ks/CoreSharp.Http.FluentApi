using CoreSharp.Http.FluentApi.Services;
using CoreSharp.Http.FluentApi.Services.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute.ReceivedExtensions;
using System.Text;
using Tests.Common.Mocks;

namespace CoreSharp.Http.FluentApi.Tests.Steps.SafeMethods;

public sealed class SafeMethodWithResultAsStreamAndCacheTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenCalled_ShouldSetProperties()
    {
        // Arrange
        const int durationMinutes = 1;
        var safeMethodWithResultAsStream = MockCreate<ISafeMethodWithResultAsStream>();
        var duration = TimeSpan.FromMinutes(durationMinutes);

        // Act
        var safeMethodWithResultAsStreamAndCache = new SafeMethodWithResultAsStreamAndCache(safeMethodWithResultAsStream, duration);

        // Assert
        var safeMethodWithResultAsStreamAndCacheAsInterface = (ISafeMethodWithResultAsStreamAndCache)safeMethodWithResultAsStreamAndCache;
        Assert.Equal(duration, safeMethodWithResultAsStreamAndCacheAsInterface.CacheDuration);
    }

    [Fact]
    public void WithCacheInvalidation_WhenFactoryIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var safeMethodWithResultAsStreamAndCache = MockCreate<SafeMethodWithResultAsStreamAndCache>();
        Func<bool> cacheInvalidationFactory = null!;

        // Act
        void Action()
            => safeMethodWithResultAsStreamAndCache.WithCacheInvalidation(cacheInvalidationFactory);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task WithCacheInvalidation_WhenFactoryIsNotNull_ShouldSetCacheInvalidationFactory()
    {
        // Arrange
        var safeMethodWithResultAsStreamAndCache = MockCreate<SafeMethodWithResultAsStreamAndCache>();
        var cacheInvalidationFactoryCalledCount = 0;
        bool CacheInvalidationFactory()
        {
            cacheInvalidationFactoryCalledCount++;
            return true;
        }

        // Act
        var result = safeMethodWithResultAsStreamAndCache.WithCacheInvalidation(CacheInvalidationFactory);
        var safeMethodWithResultAsStreamAndCacheAsInterface = (ISafeMethodWithResultAsStreamAndCache)safeMethodWithResultAsStreamAndCache;
        var shouldInvalidateCache = await safeMethodWithResultAsStreamAndCacheAsInterface.CacheInvalidationFactory();

        // Assert
        Assert.Same(safeMethodWithResultAsStreamAndCache, result);
        Assert.Equal(1, cacheInvalidationFactoryCalledCount);
        Assert.True(shouldInvalidateCache);
    }

    [Fact]
    public void WithCacheInvalidation_WhenTaskFactoryIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var safeMethodWithResultAsStreamAndCache = MockCreate<SafeMethodWithResultAsStreamAndCache>();
        Func<Task<bool>> cacheInvalidationFactory = null!;

        // Act
        void Action()
            => safeMethodWithResultAsStreamAndCache.WithCacheInvalidation(cacheInvalidationFactory);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task WithCacheInvalidation_WhenTaskFactoryIsNotNull_ShouldSetCacheInvalidationFactory()
    {
        // Arrange
        var safeMethodWithResultAsStreamAndCache = MockCreate<SafeMethodWithResultAsStreamAndCache>();
        var cacheInvalidationFactoryCalledCount = 0;
        Task<bool> CacheInvalidationFactory()
        {
            cacheInvalidationFactoryCalledCount++;
            return Task.FromResult(true);
        }

        // Act
        var result = safeMethodWithResultAsStreamAndCache.WithCacheInvalidation(CacheInvalidationFactory);
        var safeMethodWithResultAsStreamAndCacheAsInterface = (ISafeMethodWithResultAsStreamAndCache)safeMethodWithResultAsStreamAndCache;
        var shouldInvalidateCache = await safeMethodWithResultAsStreamAndCacheAsInterface.CacheInvalidationFactory();

        // Assert
        Assert.Same(safeMethodWithResultAsStreamAndCache, result);
        Assert.Equal(1, cacheInvalidationFactoryCalledCount);
        Assert.True(shouldInvalidateCache);
    }

    [Fact]
    public async Task SendAsync_WhenResultIsNotCached_ShouldRequestCacheAndReturn()
    {
        // Arrange
        var memoryCache = MockFreeze<IMemoryCache>();
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var safeMethodWithResultAsStream = MockCreate<ISafeMethodWithResultAsStream>();
        ICacheStorage cacheStorage = new CacheStorage(memoryCache);
        var safeMethodWithResultAsStreamAndCache = new SafeMethodWithResultAsStreamAndCache(safeMethodWithResultAsStream, TimeSpan.FromMinutes(1));

        safeMethodWithResultAsStream
            .Endpoint
            !.Request
            !.CacheStorage
            .Returns(cacheStorage);

        memoryCache
            .TryGetValue<byte[]>(default!, out _)
            .ReturnsForAnyArgs(false);

        var expectedResult = Encoding.UTF8.GetBytes("Data");
        using var cacheEntry = MockCreate<ICacheEntry>();
        cacheEntry.Value = expectedResult;
        memoryCache
            .CreateEntry(default!)
            .ReturnsForAnyArgs(cacheEntry);

        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent(Encoding.UTF8.GetString(expectedResult))
        };

        // Act
        using var resultAsStream = await safeMethodWithResultAsStreamAndCache.SendAsync();

        // Assert 
        var resultAsBytes = await GetBytesAsync(resultAsStream);
        Assert.Equivalent(expectedResult, resultAsBytes);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .TryGetValue<byte[]>(default!, out _);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .Set<byte[]>(default!, default!, default(TimeSpan));
    }

    [Fact]
    public async Task SendAsync_WhenResultIsCached_ShouldReturnCachedResult()
    {
        // Arrange
        var memoryCache = MockFreeze<IMemoryCache>();
        var safeMethodWithResultAsStream = MockCreate<ISafeMethodWithResultAsStream>();
        ICacheStorage cacheStorage = new CacheStorage(memoryCache);
        var safeMethodWithResultAsStreamAndCache = new SafeMethodWithResultAsStreamAndCache(safeMethodWithResultAsStream, TimeSpan.FromMinutes(1));

        safeMethodWithResultAsStream
            .Endpoint
            !.Request
            !.CacheStorage
            .Returns(cacheStorage);

        var expectedResult = Encoding.UTF8.GetBytes("Data");
        memoryCache
            .TryGetValue<byte[]>(default!, out _)
            .ReturnsForAnyArgs(call =>
            {
                call[1] = expectedResult;
                return true;
            });

        // Act
        using var resultAsStream = await safeMethodWithResultAsStreamAndCache.SendAsync();

        // Assert
        var resultAsBytes = await GetBytesAsync(resultAsStream);
        Assert.Equivalent(expectedResult, resultAsBytes);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .TryGetValue<byte[]>(default!, out _);
        memoryCache
            .DidNotReceiveWithAnyArgs()
            .Set<byte[]>(default!, default!, default(TimeSpan));
    }

    private static async Task<byte[]> GetBytesAsync(Stream? streamSource)
    {
        if (streamSource is null)
        {
            return [];
        }

        var streamTarget = new MemoryStream();
        await streamSource.CopyToAsync(streamTarget);
        return streamTarget.ToArray();
    }
}
