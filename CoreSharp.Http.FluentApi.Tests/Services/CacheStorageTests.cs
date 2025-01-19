using CoreSharp.Http.FluentApi.Services;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using Microsoft.Extensions.Caching.Memory;

namespace CoreSharp.Http.FluentApi.Tests.Services;

public sealed class CacheStorageTests : ProjectTestsBase
{
    [Fact]
    public void Instance_WhenCalled_ShouldReturnNotNullValue()
    {
        // Act
        var cacheStorage = CacheStorage.Instance;

        // Assert
        Assert.NotNull(cacheStorage);
    }

    [Fact]
    public void Instance_WhenCalled_ShouldReturnSameInstance()
    {
        // Act
        var cacheStorage1 = CacheStorage.Instance;
        var cacheStorage2 = CacheStorage.Instance;

        // Assert
        Assert.Same(cacheStorage1, cacheStorage2);
    }

    [Fact]
    public void GetCacheKey_WhenMethodIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange 
        var cacheStorage = MockCreate<CacheStorage>();

        IMethod method = null!;

        // Act
        void Action()
            => cacheStorage.GetCacheKey<string>(method);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void GetCacheKey_WhenHasQueryParameters_ShouldIncludeThemInKey()
    {
        // Arrange 
        var cacheStorage = MockCreate<CacheStorage>();
        var method = MockCreate<IMethod>();
        method.Endpoint!.QueryParameters["Key1"] = "Value1";
        method.Endpoint.QueryParameters["Key2"] = "Value2";

        // Act
        var cacheKey = cacheStorage.GetCacheKey<string>(method);

        // Assert
        Assert.Contains("Key1=Value1", cacheKey);
        Assert.Contains("Key2=Value2", cacheKey);
    }

    [Fact]
    public void GetCacheKey_WhenHasHeaders_ShouldIncludeThemInKey()
    {
        // Arrange 
        var cacheStorage = MockCreate<CacheStorage>();
        var method = MockCreate<IMethod>();
        method.Endpoint!.Request!.Headers["Key1"] = "Value1";
        method.Endpoint.Request.Headers["Key2"] = "Value2";

        // Act
        var cacheKey = cacheStorage.GetCacheKey<string>(method);

        // Assert
        Assert.Contains("Key1=Value1", cacheKey);
        Assert.Contains("Key2=Value2", cacheKey);
    }

    [Fact]
    public void GetCacheKey_WhenCalled_ShouldIncludeEndpointInKey()
    {
        // Arrange 
        var cacheStorage = MockCreate<CacheStorage>();
        var method = MockCreate<IMethod>();
        const string endpoint = @"https://www.example.com/";
        method.Endpoint!.Endpoint = endpoint;

        // Act
        var cacheKey = cacheStorage.GetCacheKey<string>(method);

        // Assert
        Assert.Contains(endpoint, cacheKey);
    }

    [Fact]
    public void GetCacheKey_WhenCalled_ShouldIncludeResponseTypeInKey()
    {
        // Arrange
        var cacheStorage = MockCreate<CacheStorage>();
        var method = MockCreate<IMethod>();

        // Act
        var cacheKey = cacheStorage.GetCacheKey<string>(method);

        // Assert
        Assert.Contains(typeof(string).FullName!, cacheKey);
    }

    [Fact]
    public void GetOrAddAsync_WhenMethodIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var cacheStorage = MockCreate<CacheStorage>();
        IMethod method = null!;
        var cacheDuration = TimeSpan.FromMinutes(1);
        static Task<string> RequestFactory() => Task.FromResult("Result");
        static Task<bool> CacheInvalidationFactory() => Task.FromResult(false);

        // Act
        Task Action() => cacheStorage.GetOrAddAsync(
            method,
            cacheDuration,
            RequestFactory,
            CacheInvalidationFactory
        );

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public void GetOrAddAsync_WhenCacheInvalidationFactoryIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var cacheStorage = MockCreate<CacheStorage>();
        var method = MockCreate<IMethod>();
        var cacheDuration = TimeSpan.FromMinutes(1);
        Func<Task<bool>> cacheInvalidationFactory = null!;
        static Task<string> RequestFactory() => Task.FromResult("Result");

        // Act
        Task Action() => cacheStorage.GetOrAddAsync(
            method,
            cacheDuration,
            RequestFactory,
            cacheInvalidationFactory
        );

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public void GetOrAddAsync_WhenRequestFactoryIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var cacheStorage = MockCreate<CacheStorage>();
        var method = MockCreate<IMethod>();
        var cacheDuration = TimeSpan.FromMinutes(1);
        static Task<bool> CacheInvalidationFactory() => Task.FromResult(false);
        Func<Task<string>> requestFactory = null!;

        // Act
        Task Action() => cacheStorage.GetOrAddAsync(
            method,
            cacheDuration,
            requestFactory,
            CacheInvalidationFactory
        );

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public void GetOrAddAsync_WhenCacheDurationIsZero_ShouldCallFactoryAndReturnResult()
    {
        // Arrange
        var memoryCache = MockFreeze<IMemoryCache>();
        var cacheStorage = MockCreate<CacheStorage>();
        var method = MockCreate<IMethod>();
        var cacheDuration = TimeSpan.Zero;
        const string expectedResult = "Result";

        var requestFactoryCallCount = 0;
        Task<string> RequestFactory()
        {
            requestFactoryCallCount++;
            return Task.FromResult(expectedResult);
        }

        var cacheInvalidationFactoryCallCount = 0;
        Task<bool> CacheInvalidationFactory()
        {
            cacheInvalidationFactoryCallCount++;
            return Task.FromResult(false);
        }

        // Act
        var result = cacheStorage.GetOrAddAsync(
            method,
            cacheDuration,
            RequestFactory,
            CacheInvalidationFactory);

        // Assert
        Assert.Equal(expectedResult, result.Result);
        Assert.Equal(1, requestFactoryCallCount);
        Assert.Equal(0, cacheInvalidationFactoryCallCount);
        memoryCache
            .DidNotReceiveWithAnyArgs()
            .TryGetValue<string>(default!, out var cachedResult);
        memoryCache
            .DidNotReceiveWithAnyArgs()
            .Set<string>(default!, default!, default(TimeSpan));
    }

    [Fact]
    public void GetOrAddAsync_WhenForceRequestIsTrue_ShouldCallFactoryAndCacheAndReturnResult()
    {
        // Arrange
        var memoryCache = MockFreeze<IMemoryCache>();
        var cacheStorage = MockCreate<CacheStorage>();
        var method = MockCreate<IMethod>();
        var cacheDuration = TimeSpan.FromMinutes(1);
        const string expectedResult = "Result";

        var requestFactoryCallCount = 0;
        Task<string> RequestFactory()
        {
            requestFactoryCallCount++;
            return Task.FromResult(expectedResult);
        }

        var cacheInvalidationFactoryCallCount = 0;
        Task<bool> CacheInvalidationFactory()
        {
            cacheInvalidationFactoryCallCount++;
            return Task.FromResult(true);
        }

        // Act
        var result = cacheStorage.GetOrAddAsync(
            method,
            cacheDuration,
            RequestFactory,
            CacheInvalidationFactory);

        // Assert
        Assert.Equal(expectedResult, result.Result);
        Assert.Equal(1, requestFactoryCallCount);
        Assert.Equal(1, cacheInvalidationFactoryCallCount);
        memoryCache
            .DidNotReceiveWithAnyArgs()
            .TryGetValue<string>(default!, out var cachedResult);
        memoryCache
            .ReceivedWithAnyArgs()
            .Set<string>(default!, default!, default(TimeSpan));
    }

    [Fact]
    public void GetOrAddAsync_WhenCacheKeyExists_ShouldReturnCachedResult()
    {
        // Arrange
        var memoryCache = MockFreeze<IMemoryCache>();
        var cacheStorage = MockCreate<CacheStorage>();
        var method = MockCreate<IMethod>();
        var cacheDuration = TimeSpan.FromMinutes(1);
        const string newResult = "Result";
        const string cachedResult = "CachedResult";

        var requestFactoryCallCount = 0;
        Task<string> RequestFactory()
        {
            requestFactoryCallCount++;
            return Task.FromResult(newResult);
        }

        var cacheInvalidationFactoryCallCount = 0;
        Task<bool> CacheInvalidationFactory()
        {
            cacheInvalidationFactoryCallCount++;
            return Task.FromResult(false);
        }

        memoryCache
            .TryGetValue<string>(default!, out _)
            .ReturnsForAnyArgs(call =>
            {
                call[1] = cachedResult;
                return true;
            });

        // Act
        var result = cacheStorage.GetOrAddAsync(
            method,
            cacheDuration,
            RequestFactory,
            CacheInvalidationFactory);

        // Assert
        Assert.Equal(cachedResult, result.Result);
        Assert.Equal(0, requestFactoryCallCount);
        Assert.Equal(1, cacheInvalidationFactoryCallCount);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .TryGetValue<string>(default!, out _);
        memoryCache
            .DidNotReceiveWithAnyArgs()
            .Set<string>(default!, default!, default(TimeSpan));
    }

    [Fact]
    public void GetOrAddAsync_WhenCacheKeyDoesNotExist_ShouldCallFactoryAndCacheAndReturnResult()
    {
        // Arrange
        var memoryCache = MockFreeze<IMemoryCache>();
        var cacheStorage = MockCreate<CacheStorage>();
        var method = MockCreate<IMethod>();
        var cacheDuration = TimeSpan.FromMinutes(1);
        const string expectedResult = "Result";

        var requestFactoryCallCount = 0;
        Task<string> RequestFactory()
        {
            requestFactoryCallCount++;
            return Task.FromResult(expectedResult);
        }

        var cacheInvalidationFactoryCallCount = 0;
        Task<bool> CacheInvalidationFactory()
        {
            cacheInvalidationFactoryCallCount++;
            return Task.FromResult(false);
        }

        // Act
        var result = cacheStorage.GetOrAddAsync(
            method,
            cacheDuration,
            RequestFactory,
            CacheInvalidationFactory);

        // Assert
        Assert.Equal(expectedResult, result.Result);
        Assert.Equal(1, requestFactoryCallCount);
        Assert.Equal(1, cacheInvalidationFactoryCallCount);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .TryGetValue<string>(default!, out var cachedResult);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .Set<string>(default!, default!, default(TimeSpan));
    }
}
