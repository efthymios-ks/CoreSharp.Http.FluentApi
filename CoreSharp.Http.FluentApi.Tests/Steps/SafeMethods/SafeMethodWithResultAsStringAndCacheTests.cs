using CoreSharp.Http.FluentApi.Services;
using CoreSharp.Http.FluentApi.Services.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute.ReceivedExtensions;
using Tests.Common.Mocks;

namespace CoreSharp.Http.FluentApi.Tests.Steps.SafeMethods;

public sealed class SafeMethodWithResultAsStringAndCacheTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenCalled_ShouldSetProperties()
    {
        // Arrange
        var safeMethodWithResultAsString = MockCreate<ISafeMethodWithResultAsString>();
        const int durationMinutes = 1;
        var duration = TimeSpan.FromMinutes(durationMinutes);

        // Act
        var safeMethodWithResultAsStringAndCache = new SafeMethodWithResultAsStringAndCache(safeMethodWithResultAsString, duration);

        // Assert
        var safeMethodWithResultAsStringAndCacheAsInterface = (ISafeMethodWithResultAsStringAndCache)safeMethodWithResultAsStringAndCache;
        Assert.Equal(duration, safeMethodWithResultAsStringAndCacheAsInterface.CacheDuration);
    }

    [Fact]
    public void WithCacheInvalidation_WhenFactoryIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var safeMethodWithResultAsStringAndCache = MockCreate<SafeMethodWithResultAsStringAndCache>();
        Func<bool> cacheInvalidationFactory = null!;

        // Act
        void Action()
            => safeMethodWithResultAsStringAndCache.WithCacheInvalidation(cacheInvalidationFactory);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task WithCacheInvalidation_WhenFactoryIsNotNull_ShouldSetCacheInvalidationFactory()
    {
        // Arrange
        var safeMethodWithResultAsStringAndCache = MockCreate<SafeMethodWithResultAsStringAndCache>();
        var cacheInvalidationFactoryCalledCount = 0;
        bool CacheInvalidationFactory()
        {
            cacheInvalidationFactoryCalledCount++;
            return true;
        }

        // Act
        var result = safeMethodWithResultAsStringAndCache.WithCacheInvalidation(CacheInvalidationFactory);
        var safeMethodWithResultAsStringAndCacheAsInterface = (ISafeMethodWithResultAsStringAndCache)safeMethodWithResultAsStringAndCache;
        var shouldInvalidateCache = await safeMethodWithResultAsStringAndCacheAsInterface.CacheInvalidationFactory();

        // Assert
        Assert.Same(safeMethodWithResultAsStringAndCache, result);
        Assert.Equal(1, cacheInvalidationFactoryCalledCount);
        Assert.True(shouldInvalidateCache);
    }

    [Fact]
    public void WithCacheInvalidation_WhenTaskFactoryIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var safeMethodWithResultAsStringAndCache = MockCreate<SafeMethodWithResultAsStringAndCache>();
        Func<Task<bool>> cacheInvalidationFactory = null!;

        // Act
        void Action()
            => safeMethodWithResultAsStringAndCache.WithCacheInvalidation(cacheInvalidationFactory);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task WithCacheInvalidation_WhenTaskFactoryIsNotNull_ShouldSetCacheInvalidationFactory()
    {
        // Arrange
        var safeMethodWithResultAsStringAndCache = MockCreate<SafeMethodWithResultAsStringAndCache>();
        var cacheInvalidationFactoryCalledCount = 0;
        Task<bool> CacheInvalidationFactory()
        {
            cacheInvalidationFactoryCalledCount++;
            return Task.FromResult(true);
        }

        // Act
        var result = safeMethodWithResultAsStringAndCache.WithCacheInvalidation(CacheInvalidationFactory);
        var safeMethodWithResultAsStringAndCacheAsInterface = (ISafeMethodWithResultAsStringAndCache)safeMethodWithResultAsStringAndCache;
        var shouldInvalidateCache = await safeMethodWithResultAsStringAndCacheAsInterface.CacheInvalidationFactory();

        // Assert
        Assert.Same(safeMethodWithResultAsStringAndCache, result);
        Assert.Equal(1, cacheInvalidationFactoryCalledCount);
        Assert.True(shouldInvalidateCache);
    }

    [Fact]
    public async Task SendAsync_WhenResultIsNotCached_ShouldRequestCacheAndReturn()
    {
        // Arrange
        var memoryCache = MockFreeze<IMemoryCache>();
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var safeMethodWithResultAsString = MockCreate<ISafeMethodWithResultAsString>();
        ICacheStorage cacheStorage = new CacheStorage(memoryCache);
        var safeMethodWithResultAsStringAndCache = new SafeMethodWithResultAsStringAndCache(safeMethodWithResultAsString, TimeSpan.FromMinutes(1));

        safeMethodWithResultAsString
            .Endpoint!
            .Request!
            .CacheStorage
            .Returns(cacheStorage);

        memoryCache
            .TryGetValue<byte[]>(default!, out _)
            .ReturnsForAnyArgs(false);

        const string expectedResult = "Data";
        using var cacheEntry = MockCreate<ICacheEntry>();
        cacheEntry.Value = expectedResult;
        memoryCache
            .CreateEntry(default!)
            .ReturnsForAnyArgs(cacheEntry);

        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent(expectedResult)
        };

        // Act
        var result = await safeMethodWithResultAsStringAndCache.SendAsync();

        // Assert
        Assert.Equivalent(expectedResult, result);
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
        var safeMethodWithResultAsString = MockCreate<ISafeMethodWithResultAsString>();
        ICacheStorage cacheStorage = new CacheStorage(memoryCache);
        var safeMethodWithResultAsStringAndCache = new SafeMethodWithResultAsStringAndCache(safeMethodWithResultAsString, TimeSpan.FromMinutes(1));

        safeMethodWithResultAsString
            .Endpoint
            !.Request
            !.CacheStorage
            .Returns(cacheStorage);

        const string expectedResult = "Data";
        memoryCache
            .TryGetValue<byte[]>(default!, out _)
            .ReturnsForAnyArgs(call =>
            {
                call[1] = expectedResult;
                return true;
            });

        // Act
        var result = await safeMethodWithResultAsStringAndCache.SendAsync();

        // Assert
        Assert.Equivalent(expectedResult, result);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .TryGetValue<byte[]>(default!, out _);
        memoryCache
            .DidNotReceiveWithAnyArgs()
            .Set<byte[]>(default!, default!, default(TimeSpan));
    }
}
