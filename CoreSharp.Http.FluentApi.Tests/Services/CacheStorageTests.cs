﻿using AutoFixture.NUnit3;
using CoreSharp.Http.FluentApi.Services;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using NUnit.Framework;
using Tests.Internal.Attributes;

namespace Tests.Services;

[TestFixture]
public sealed class CacheStorageTests
{
    [Test]
    public void Instance_WhenCalled_ShouldReturnNotNulLValue()
    {
        // Act
        var cacheStorage = CacheStorage.Instance;

        // Assert
        cacheStorage.Should().NotBeNull();
    }

    [Test]
    public void Instance_WhenCalled_ShouldReturnSameInstance()
    {
        // Act
        var cacheStorage1 = CacheStorage.Instance;
        var cacheStorage2 = CacheStorage.Instance;

        // Assert
        cacheStorage1.Should().BeSameAs(cacheStorage2);
    }

    [Test]
    [AutoNSubstituteData]
    public void GetCacheKey_WhenMethodIsNull_ShouldThrowArgumentNullException(CacheStorage cacheStorage)
    {
        // Arrange 
        IMethod method = null!;

        // Act
        Action action = () => cacheStorage.GetCacheKey<string>(method);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void GetCacheKey_WhenHasQueryParameters_ShouldIncludeThemInKey(
        IMethod method,
        CacheStorage cacheStorage)
    {
        // Arrange 
        method.Endpoint!.QueryParameters["Key1"] = "Value1";
        method.Endpoint.QueryParameters["Key2"] = "Value2";

        // Act
        var cacheKey = cacheStorage.GetCacheKey<string>(method);

        // Assert
        cacheKey.Should().Contain("Key1=Value1");
        cacheKey.Should().Contain("Key2=Value2");
    }

    [Test]
    [AutoNSubstituteData]
    public void GetCacheKey_WhenHasHeaders_ShouldIncludeThemInKey(
        IMethod method,
        CacheStorage cacheStorage)
    {
        // Arrange 
        method.Endpoint!.Request!.Headers["Key1"] = "Value1";
        method.Endpoint.Request.Headers["Key2"] = "Value2";

        // Act
        var cacheKey = cacheStorage.GetCacheKey<string>(method);

        // Assert
        cacheKey.Should().Contain("Key1=Value1");
        cacheKey.Should().Contain("Key2=Value2");
    }

    [Test]
    [AutoNSubstituteData]
    public void GetCacheKey_WhenCalled_ShouldIncludeEndpointInKey(
        IMethod method,
        CacheStorage cacheStorage)
    {
        // Arrange 
        const string endpoint = @"https://www.example.com/";
        method.Endpoint!.Endpoint = endpoint;

        // Act
        var cacheKey = cacheStorage.GetCacheKey<string>(method);

        // Assert
        cacheKey.Should().Contain(endpoint);
    }

    [Test]
    [AutoNSubstituteData]
    public void GetCacheKey_WhenCalled_ShouldIncludeResponseTypeInKey(
        IMethod method,
        CacheStorage cacheStorage)
    {
        // Act
        var cacheKey = cacheStorage.GetCacheKey<string>(method);

        // Assert
        cacheKey.Should().Contain(typeof(string).FullName);
    }

    [Test]
    [AutoNSubstituteData]
    public void GetOrAddAsync_WhenMethodIsNull_ShouldThrowArgumentNullException(CacheStorage cacheStorage)
    {
        // Arrange
        IMethod method = null!;
        var cacheDuration = TimeSpan.FromMinutes(1);
        static Task<string> RequestFactory()
            => Task.FromResult("Result");
        static Task<bool> CacheInvalidationFactory()
            => Task.FromResult(false);

        // Act
        Func<Task> action = () => cacheStorage.GetOrAddAsync(
            method,
            cacheDuration,
            RequestFactory,
            CacheInvalidationFactory);

        // Assert
        action.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void GetOrAddAsync_WhenCacheInvalidationFactoryIsNull_ShouldThrowArgumentNullException(
        IMethod method,
        CacheStorage cacheStorage)
    {
        // Arrange
        var cacheDuration = TimeSpan.FromMinutes(1);
        Func<Task<bool>> cacheInvalidationFactory = null!;
        static Task<string> RequestFactory()
            => Task.FromResult("Result");

        // Act
        Func<Task> action = () => cacheStorage.GetOrAddAsync(
            method,
            cacheDuration,
            RequestFactory,
            cacheInvalidationFactory);

        // Assert
        action.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void GetOrAddAsync_WhenRequestFactoryIsNull_ShouldThrowArgumentNullException(
        IMethod method,
        CacheStorage cacheStorage)
    {
        // Arrange
        var cacheDuration = TimeSpan.FromMinutes(1);
        static Task<bool> CacheInvalidationFactory()
            => Task.FromResult(false);
        Func<Task<string>> requestFactory = null!;

        // Act
        Func<Task> action = () => cacheStorage.GetOrAddAsync(
            method,
            cacheDuration,
            requestFactory,
            CacheInvalidationFactory);

        // Assert
        action.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void GetOrAddAsync_WhenCacheDurationIsZero_ShouldCallFactoryAndReturnResult(
        IMethod method,
        [Frozen] IMemoryCache memoryCache,
        CacheStorage cacheStorage)
    {
        // Arrange
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
        result.Result.Should().Be(expectedResult);
        requestFactoryCallCount.Should().Be(1);
        cacheInvalidationFactoryCallCount.Should().Be(0);
        memoryCache
            .DidNotReceiveWithAnyArgs()
            .TryGetValue<string>(default!, out var cachedResult);
        memoryCache
            .DidNotReceiveWithAnyArgs()
            .Set<string>(default!, default!, default(TimeSpan));
    }

    [Test]
    [AutoNSubstituteData]
    public void GetOrAddAsync_WhenForceRequestIsTrue_ShouldCallFactoryAndCacheAndReturnResult(
        IMethod method,
        [Frozen] IMemoryCache memoryCache,
        CacheStorage cacheStorage)
    {
        // Arrange
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
        result.Result.Should().Be(expectedResult);
        requestFactoryCallCount.Should().Be(1);
        cacheInvalidationFactoryCallCount.Should().Be(1);
        memoryCache
            .DidNotReceiveWithAnyArgs()
            .TryGetValue<string>(default!, out var cachedResult);
        memoryCache
            .ReceivedWithAnyArgs()
            .Set<string>(default!, default!, default(TimeSpan));
    }

    [Test]
    [AutoNSubstituteData]
    public void GetOrAddAsync_WhenCacheKeyExists_ShouldReturnCachedResult(
        IMethod method,
        [Frozen] IMemoryCache memoryCache,
        CacheStorage cacheStorage)
    {
        // Arrange
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
        result.Result.Should().Be(cachedResult);
        requestFactoryCallCount.Should().Be(0);
        cacheInvalidationFactoryCallCount.Should().Be(1);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .TryGetValue<string>(default!, out _);
        memoryCache
            .DidNotReceiveWithAnyArgs()
            .Set<string>(default!, default!, default(TimeSpan));
    }

    [Test]
    [AutoNSubstituteData]
    public void GetOrAddAsync_WhenCacheKeyDoesNotExist_ShouldCallFactoryAndCacheAndReturnResult(
        IMethod method,
        [Frozen] IMemoryCache memoryCache,
        CacheStorage cacheStorage)
    {
        // Arrange
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
        result.Result.Should().Be(expectedResult);
        requestFactoryCallCount.Should().Be(1);
        cacheInvalidationFactoryCallCount.Should().Be(1);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .TryGetValue<string>(default!, out var cachedResult);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .Set<string>(default!, default!, default(TimeSpan));
    }
}
