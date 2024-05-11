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
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tests.Internal.Attributes;
using Tests.Internal.HttpmessageHandlers;

namespace Tests.Steps.SafeMethods;

[TestFixture]
public sealed class SafeMethodWithResultAsStreamAndCacheTests
{
    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenCalled_ShouldSetProperties(ISafeMethodWithResultAsStream safeMethodWithResultAsStream)
    {
        // Arrange
        var duration = TimeSpan.FromMinutes(1);

        // Arrange
        var safeMethodWithResultAsStreamAndCache = new SafeMethodWithResultAsStreamAndCache(safeMethodWithResultAsStream, duration);

        // Assert
        var safeMethodWithResultAsStreamAndCacheAsInterface = (ISafeMethodWithResultAsStreamAndCache)safeMethodWithResultAsStreamAndCache;
        safeMethodWithResultAsStreamAndCacheAsInterface.CacheDuration.Should().Be(duration);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithCacheInvalidation_WhenFactoryIsNull_ShouldThrowArgumentNullException(SafeMethodWithResultAsStreamAndCache safeMethodWithResultAsStreamAndCache)
    {
        // Arrange
        Func<bool> cacheInvalidationFactory = null;

        // Act
        Action action = () => safeMethodWithResultAsStreamAndCache.WithCacheInvalidation(cacheInvalidationFactory);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task WithCacheInvalidation_WhenFactoryIsNotNull_ShouldSetCacheInvalidationFactory(SafeMethodWithResultAsStreamAndCache safeMethodWithResultAsStreamAndCache)
    {
        // Arrange
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
        result.Should().BeSameAs(safeMethodWithResultAsStreamAndCache);
        cacheInvalidationFactoryCalledCount.Should().Be(1);
        shouldInvalidateCache.Should().BeTrue();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithCacheInvalidation_WhenTaskFactoryIsNull_ShouldThrowArgumentNullException(SafeMethodWithResultAsStreamAndCache safeMethodWithResultAsStreamAndCache)
    {
        // Arrange
        Func<Task<bool>> cacheInvalidationFactory = null;

        // Act
        Action action = () => safeMethodWithResultAsStreamAndCache.WithCacheInvalidation(cacheInvalidationFactory);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task WithCacheInvalidation_WhenTaskFactoryIsNotNull_ShouldSetCacheInvalidationFactory(SafeMethodWithResultAsStreamAndCache safeMethodWithResultAsStreamAndCache)
    {
        // Arrange
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
        result.Should().BeSameAs(safeMethodWithResultAsStreamAndCache);
        cacheInvalidationFactoryCalledCount.Should().Be(1);
        shouldInvalidateCache.Should().BeTrue();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenResultIsNotCached_ShouldRequestCacheAndReturn(
        [Frozen] IMemoryCache memoryCache,
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        ISafeMethodWithResultAsStream safeMethodWithResultAsStream)
    {
        // Arrange
        ICacheStorage cacheStorage = new CacheStorage(memoryCache);
        var safeMethodWithResultAsStreamAndCache = new SafeMethodWithResultAsStreamAndCache(safeMethodWithResultAsStream, TimeSpan.FromMinutes(1));

        safeMethodWithResultAsStream
            .Endpoint
            .Request
            .CacheStorage
            .Returns(cacheStorage);

        memoryCache
            .TryGetValue<byte[]>(default, out _)
            .ReturnsForAnyArgs(false);

        var expectedResult = Encoding.UTF8.GetBytes("Data");
        using var cacheEntry = Substitute.For<ICacheEntry>();
        cacheEntry.Value = expectedResult;
        memoryCache
            .CreateEntry(default)
            .ReturnsForAnyArgs(cacheEntry);

        mockHttpMessageHandler.ResponseContent = Encoding.UTF8.GetString(expectedResult);

        // Act
        using var resultAsStream = await safeMethodWithResultAsStreamAndCache.SendAsync();

        // Assert 
        var resultAsBytes = await GetBytesAsync(resultAsStream);
        resultAsBytes.Should().BeEquivalentTo(expectedResult);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .TryGetValue<byte[]>(default, out _);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .Set<byte[]>(default, default, default(TimeSpan));
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenResultIsCached_ShouldReturnCachedResult(
        [Frozen] IMemoryCache memoryCache,
        ISafeMethodWithResultAsStream safeMethodWithResultAsStream)
    {
        // Arrange
        ICacheStorage cacheStorage = new CacheStorage(memoryCache);
        var safeMethodWithResultAsStreamAndCache = new SafeMethodWithResultAsStreamAndCache(safeMethodWithResultAsStream, TimeSpan.FromMinutes(1));

        safeMethodWithResultAsStream
            .Endpoint
            .Request
            .CacheStorage
            .Returns(cacheStorage);

        var expectedResult = Encoding.UTF8.GetBytes("Data");
        memoryCache
            .TryGetValue<byte[]>(default, out _)
            .ReturnsForAnyArgs(call =>
            {
                call[1] = expectedResult;
                return true;
            });

        // Act
        using var resultAsStream = await safeMethodWithResultAsStreamAndCache.SendAsync();

        // Assert
        var resultAsBytes = await GetBytesAsync(resultAsStream);
        resultAsBytes.Should().BeEquivalentTo(expectedResult);
        memoryCache
            .ReceivedWithAnyArgs(1)
            .TryGetValue<byte[]>(default, out _);
        memoryCache
            .DidNotReceiveWithAnyArgs()
            .Set<byte[]>(default, default, default(TimeSpan));
    }

    private static async Task<byte[]> GetBytesAsync(Stream streamSource)
    {
        var streamTarget = new MemoryStream();
        await streamSource.CopyToAsync(streamTarget);
        return streamTarget.ToArray();
    }
}
