using AutoFixture.NUnit3;
using CoreSharp.Http.FluentApi.Services;
using CoreSharp.Http.FluentApi.Services.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using Tests.Internal.Attributes;
using Tests.Internal.HttpmessageHandlers;

namespace Tests.Steps.SafeMethods;

[TestFixture]
public sealed class SafeMethodWithResultAsStringAndCacheTests
{
    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenCalled_ShouldSetProperties(ISafeMethodWithResultAsString safeMethodWithResultAsString)
    {
        // Arrange
        var duration = TimeSpan.FromMinutes(1);

        // Arrange
        var safeMethodWithResultAsStringAndCache = new SafeMethodWithResultAsStringAndCache(safeMethodWithResultAsString, duration);

        // Assert
        var safeMethodWithResultAsStringAndCacheAsInterface = (ISafeMethodWithResultAsStringAndCache)safeMethodWithResultAsStringAndCache;
        safeMethodWithResultAsStringAndCacheAsInterface.CacheDuration.Should().Be(duration);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithCacheInvalidation_WhenFactoryIsNull_ShouldThrowArgumentNullException(SafeMethodWithResultAsStringAndCache safeMethodWithResultAsStringAndCache)
    {
        // Arrange
        Func<bool> cacheInvalidationFactory = null!;

        // Act
        Action action = () => safeMethodWithResultAsStringAndCache.WithCacheInvalidation(cacheInvalidationFactory);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task WithCacheInvalidation_WhenFactoryIsNotNull_ShouldSetCacheInvalidationFactory(SafeMethodWithResultAsStringAndCache safeMethodWithResultAsStringAndCache)
    {
        // Arrange
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
        result.Should().BeSameAs(safeMethodWithResultAsStringAndCache);
        cacheInvalidationFactoryCalledCount.Should().Be(1);
        shouldInvalidateCache.Should().BeTrue();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithCacheInvalidation_WhenTaskFactoryIsNull_ShouldThrowArgumentNullException(SafeMethodWithResultAsStringAndCache safeMethodWithResultAsStringAndCache)
    {
        // Arrange
        Func<Task<bool>> cacheInvalidationFactory = null!;

        // Act
        Action action = () => safeMethodWithResultAsStringAndCache.WithCacheInvalidation(cacheInvalidationFactory);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task WithCacheInvalidation_WhenTaskFactoryIsNotNull_ShouldSetCacheInvalidationFactory(SafeMethodWithResultAsStringAndCache safeMethodWithResultAsStringAndCache)
    {
        // Arrange
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
        result.Should().BeSameAs(safeMethodWithResultAsStringAndCache);
        cacheInvalidationFactoryCalledCount.Should().Be(1);
        shouldInvalidateCache.Should().BeTrue();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenResultIsNotCached_ShouldRequestCacheAndReturn(
        [Frozen] IMemoryCache memoryCache,
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        ISafeMethodWithResultAsString safeMethodWithResultAsString)
    {
        // Arrange
        ICacheStorage cacheStorage = new CacheStorage(memoryCache);
        var safeMethodWithResultAsStringAndCache = new SafeMethodWithResultAsStringAndCache(safeMethodWithResultAsString, TimeSpan.FromMinutes(1));

        safeMethodWithResultAsString
            .Endpoint
            !.Request
            !.CacheStorage
            .Returns(cacheStorage);

        memoryCache
            .TryGetValue<byte[]>(default!, out _)
            .ReturnsForAnyArgs(false);

        const string expectedResult = "Data";
        using var cacheEntry = Substitute.For<ICacheEntry>();
        cacheEntry.Value = expectedResult;
        memoryCache
            .CreateEntry(default!)
            .ReturnsForAnyArgs(cacheEntry);

        mockHttpMessageHandler.ResponseContent = expectedResult;

        // Act
        var result = await safeMethodWithResultAsStringAndCache.SendAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .TryGetValue<byte[]>(default!, out _);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .Set<byte[]>(default!, default!, default(TimeSpan));
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenResultIsCached_ShouldReturnCachedResult(
        [Frozen] IMemoryCache memoryCache,
        ISafeMethodWithResultAsString safeMethodWithResultAsString)
    {
        // Arrange
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
        result.Should().BeEquivalentTo(expectedResult);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .TryGetValue<byte[]>(default!, out _);
        memoryCache
            .DidNotReceiveWithAnyArgs()
            .Set<byte[]>(default!, default!, default(TimeSpan));
    }
}
