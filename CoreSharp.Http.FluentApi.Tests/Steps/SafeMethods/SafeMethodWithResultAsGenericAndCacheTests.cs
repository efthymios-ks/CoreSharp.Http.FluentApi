using CoreSharp.Http.FluentApi.Services;
using CoreSharp.Http.FluentApi.Services.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute.ReceivedExtensions;
using System.Text;
using Tests.Common.Mocks;

namespace CoreSharp.Http.FluentApi.Tests.Steps.SafeMethods;

public sealed class SafeMethodWithResultAsGenericAndCacheTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenSafeMethodWithResultAsGenericIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        ISafeMethodWithResultAsGeneric<string> safeMethodWithResultAsGeneric = null!;
        var duration = TimeSpan.FromMinutes(1);

        // Arrange
        void Action()
            => _ = new SafeMethodWithResultAsGenericAndCache<string>(safeMethodWithResultAsGeneric, duration);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenCalled_ShouldSetProperties()
    {
        // Arrange
        var safeMethodWithResultAsGeneric = MockCreate<ISafeMethodWithResultAsGeneric<string>>();
        var duration = TimeSpan.FromMinutes(1);

        // Arrange
        var safeMethodWithResultAsBytesAndCache = new SafeMethodWithResultAsGenericAndCache<string>(safeMethodWithResultAsGeneric, duration);

        // Assert
        var safeMethodWithResultAsBytesAndCacheAsInterface = (ISafeMethodWithResultAsGenericAndCache<string>)safeMethodWithResultAsBytesAndCache;
        Assert.Equal(duration, safeMethodWithResultAsBytesAndCacheAsInterface.CacheDuration);
    }

    [Fact]
    public void WithCacheInvalidation_WhenFactoryIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var safeMethodWithResultAsBytesAndCache = MockCreate<SafeMethodWithResultAsGenericAndCache<string>>();
        Func<bool> cacheInvalidationFactory = null!;

        // Act
        void Action()
            => safeMethodWithResultAsBytesAndCache.WithCacheInvalidation(cacheInvalidationFactory);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task WithCacheInvalidation_WhenFactoryIsNotNull_ShouldSetCacheInvalidationFactory()
    {
        // Arrange
        var safeMethodWithResultAsBytesAndCache = MockCreate<SafeMethodWithResultAsGenericAndCache<string>>();
        var cacheInvalidationFactoryCalledCount = 0;
        bool CacheInvalidationFactory()
        {
            cacheInvalidationFactoryCalledCount++;
            return true;
        }

        // Act
        var result = safeMethodWithResultAsBytesAndCache.WithCacheInvalidation(CacheInvalidationFactory);
        var safeMethodWithResultAsBytesAndCacheAsInterface = (ISafeMethodWithResultAsGenericAndCache<string>)safeMethodWithResultAsBytesAndCache;
        var shouldInvalidateCache = await safeMethodWithResultAsBytesAndCacheAsInterface.CacheInvalidationFactory();

        // Assert
        Assert.Same(safeMethodWithResultAsBytesAndCache, result);
        Assert.Equal(1, cacheInvalidationFactoryCalledCount);
        Assert.True(shouldInvalidateCache);
    }

    [Fact]
    public void WithCacheInvalidation_WhenTaskFactoryIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var safeMethodWithResultAsBytesAndCache = MockCreate<SafeMethodWithResultAsGenericAndCache<string>>();
        Func<Task<bool>> cacheInvalidationFactory = null!;

        // Act
        void Action()
            => safeMethodWithResultAsBytesAndCache.WithCacheInvalidation(cacheInvalidationFactory);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task WithCacheInvalidation_WhenTaskFactoryIsNotNull_ShouldSetCacheInvalidationFactory()
    {
        // Arrange
        var safeMethodWithResultAsBytesAndCache = MockCreate<SafeMethodWithResultAsGenericAndCache<string>>();
        var cacheInvalidationFactoryCalledCount = 0;
        Task<bool> CacheInvalidationFactory()
        {
            cacheInvalidationFactoryCalledCount++;
            return Task.FromResult(true);
        }

        // Act
        var result = safeMethodWithResultAsBytesAndCache.WithCacheInvalidation(CacheInvalidationFactory);
        var safeMethodWithResultAsBytesAndCacheAsInterface = (ISafeMethodWithResultAsGenericAndCache<string>)safeMethodWithResultAsBytesAndCache;
        var shouldInvalidateCache = await safeMethodWithResultAsBytesAndCacheAsInterface.CacheInvalidationFactory();

        // Assert
        Assert.Same(safeMethodWithResultAsBytesAndCache, result);
        Assert.Equal(1, cacheInvalidationFactoryCalledCount);
        Assert.True(shouldInvalidateCache);
    }

    [Fact]
    public async Task SendAsync_WhenResultIsNotCached_ShouldRequestCacheAndReturn()
    {
        // Arrange
        var memoryCache = MockFreeze<IMemoryCache>();
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var safeMethodWithResultAsGeneric = MockCreate<ISafeMethodWithResultAsGeneric<string>>();
        ICacheStorage cacheStorage = new CacheStorage(memoryCache);
        safeMethodWithResultAsGeneric.DeserializeFunction = async response =>
        {
            await using var memoryStream = new MemoryStream();
            await response.CopyToAsync(memoryStream);
            var buffer = memoryStream.ToArray();
            return Encoding.UTF8.GetString(buffer);
        };
        var safeMethodWithResultAsBytesAndCache = new SafeMethodWithResultAsGenericAndCache<string>(safeMethodWithResultAsGeneric, TimeSpan.FromMinutes(1));

        safeMethodWithResultAsGeneric
            .Endpoint
            !.Request
            !.CacheStorage
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
        var result = await safeMethodWithResultAsBytesAndCache.SendAsync();

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
        var safeMethodWithResultAsGeneric = MockCreate<ISafeMethodWithResultAsGeneric<string>>();
        ICacheStorage cacheStorage = new CacheStorage(memoryCache);
        var safeMethodWithResultAsBytesAndCache = new SafeMethodWithResultAsGenericAndCache<string>(safeMethodWithResultAsGeneric, TimeSpan.FromMinutes(1));

        safeMethodWithResultAsGeneric
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
        var result = await safeMethodWithResultAsBytesAndCache.SendAsync();

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
