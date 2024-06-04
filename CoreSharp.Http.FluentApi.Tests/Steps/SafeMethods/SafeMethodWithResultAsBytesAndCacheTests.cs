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
using System.Text;
using Tests.Internal.Attributes;
using Tests.Internal.HttpmessageHandlers;

namespace Tests.Steps.SafeMethods;

[TestFixture]
public sealed class SafeMethodWithResultAsBytesAndCacheTests
{
    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenCalled_ShouldSetProperties(ISafeMethodWithResultAsBytes safeMethodWithResultAsBytes)
    {
        // Arrange
        var duration = TimeSpan.FromMinutes(1);

        // Arrange
        var safeMethodWithResultAsBytesAndCache = new SafeMethodWithResultAsBytesAndCache(safeMethodWithResultAsBytes, duration);

        // Assert
        var safeMethodWithResultAsBytesAndCacheAsInterface = (ISafeMethodWithResultAsBytesAndCache)safeMethodWithResultAsBytesAndCache;
        safeMethodWithResultAsBytesAndCacheAsInterface.CacheDuration.Should().Be(duration);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithCacheInvalidation_WhenFactoryIsNull_ShouldThrowArgumentNullException(SafeMethodWithResultAsBytesAndCache safeMethodWithResultAsBytesAndCache)
    {
        // Arrange
        Func<bool> cacheInvalidationFactory = null!;

        // Act
        Action action = () => safeMethodWithResultAsBytesAndCache.WithCacheInvalidation(cacheInvalidationFactory);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task WithCacheInvalidation_WhenFactoryIsNotNull_ShouldSetCacheInvalidationFactory(SafeMethodWithResultAsBytesAndCache safeMethodWithResultAsBytesAndCache)
    {
        // Arrange
        var cacheInvalidationFactoryCalledCount = 0;
        bool CacheInvalidationFactory()
        {
            cacheInvalidationFactoryCalledCount++;
            return true;
        }

        // Act
        var result = safeMethodWithResultAsBytesAndCache.WithCacheInvalidation(CacheInvalidationFactory);
        var safeMethodWithResultAsBytesAndCacheAsInterface = (ISafeMethodWithResultAsBytesAndCache)safeMethodWithResultAsBytesAndCache;
        var shouldInvalidateCache = await safeMethodWithResultAsBytesAndCacheAsInterface.CacheInvalidationFactory();

        // Assert
        result.Should().BeSameAs(safeMethodWithResultAsBytesAndCache);
        cacheInvalidationFactoryCalledCount.Should().Be(1);
        shouldInvalidateCache.Should().BeTrue();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithCacheInvalidation_WhenTaskFactoryIsNull_ShouldThrowArgumentNullException(SafeMethodWithResultAsBytesAndCache safeMethodWithResultAsBytesAndCache)
    {
        // Arrange
        Func<Task<bool>> cacheInvalidationFactory = null!;

        // Act
        Action action = () => safeMethodWithResultAsBytesAndCache.WithCacheInvalidation(cacheInvalidationFactory);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task WithCacheInvalidation_WhenTaskFactoryIsNotNull_ShouldSetCacheInvalidationFactory(SafeMethodWithResultAsBytesAndCache safeMethodWithResultAsBytesAndCache)
    {
        // Arrange
        var cacheInvalidationFactoryCalledCount = 0;
        Task<bool> CacheInvalidationFactory()
        {
            cacheInvalidationFactoryCalledCount++;
            return Task.FromResult(true);
        }

        // Act
        var result = safeMethodWithResultAsBytesAndCache.WithCacheInvalidation(CacheInvalidationFactory);
        var safeMethodWithResultAsBytesAndCacheAsInterface = (ISafeMethodWithResultAsBytesAndCache)safeMethodWithResultAsBytesAndCache;
        var shouldInvalidateCache = await safeMethodWithResultAsBytesAndCacheAsInterface.CacheInvalidationFactory();

        // Assert
        result.Should().BeSameAs(safeMethodWithResultAsBytesAndCache);
        cacheInvalidationFactoryCalledCount.Should().Be(1);
        shouldInvalidateCache.Should().BeTrue();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenResultIsNotCached_ShouldRequestCacheAndReturn(
        [Frozen] IMemoryCache memoryCache,
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        ISafeMethodWithResultAsBytes safeMethodWithResultAsBytes)
    {
        // Arrange
        ICacheStorage cacheStorage = new CacheStorage(memoryCache);
        var safeMethodWithResultAsBytesAndCache = new SafeMethodWithResultAsBytesAndCache(safeMethodWithResultAsBytes, TimeSpan.FromMinutes(1));

        safeMethodWithResultAsBytes
            .Endpoint
            !.Request
            !.CacheStorage
            .Returns(cacheStorage);

        memoryCache
            .TryGetValue<byte[]>(default!, out _)
            .ReturnsForAnyArgs(false);

        var expectedResult = Encoding.UTF8.GetBytes("Data");
        using var cacheEntry = Substitute.For<ICacheEntry>();
        cacheEntry.Value = expectedResult;
        memoryCache
            .CreateEntry(default!)
            .ReturnsForAnyArgs(cacheEntry);

        mockHttpMessageHandler.ResponseContent = Encoding.UTF8.GetString(expectedResult);

        // Act
        var result = await safeMethodWithResultAsBytesAndCache.SendAsync();

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
        ISafeMethodWithResultAsBytes safeMethodWithResultAsBytes)
    {
        // Arrange
        ICacheStorage cacheStorage = new CacheStorage(memoryCache);
        var safeMethodWithResultAsBytesAndCache = new SafeMethodWithResultAsBytesAndCache(safeMethodWithResultAsBytes, TimeSpan.FromMinutes(1));

        safeMethodWithResultAsBytes
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
        var result = await safeMethodWithResultAsBytesAndCache.SendAsync();

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
