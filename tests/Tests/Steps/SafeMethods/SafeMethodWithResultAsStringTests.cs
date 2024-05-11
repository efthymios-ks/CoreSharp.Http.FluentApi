using AutoFixture.NUnit3;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Tests.Internal.Attributes;
using Tests.Internal.HttpmessageHandlers;

namespace Tests.Steps.SafeMethods;

[TestFixture]
public sealed class SafeMethodWithResultAsStringTests
{
    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenCalled_ShouldNotThrow(ISafeMethod safeMethod)
    {
        // Act
        Action action = () => _ = new SafeMethodWithResultAsString(safeMethod);

        // Assert
        action.Should().NotThrow();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithCache_WhenCalled_ShouldReturnSafeMethodWithResultAsBytesAndCache(ISafeMethod safeMethod)
    {
        // Arrange 
        var safeMethodWithResultAsString = new SafeMethodWithResultAsString(safeMethod);
        var cacheDuration = TimeSpan.FromSeconds(1);

        // Act
        var result = safeMethodWithResultAsString.WithCache(cacheDuration);

        // Assert
        result.Should().BeOfType<SafeMethodWithResultAsStringAndCache>();
        result.CacheDuration.Should().Be(cacheDuration);
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenCalled_ShouldReturnByteArray(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        ISafeMethod safeMethod)
    {
        // Arrange
        var safeMethodWithResultAsString = new SafeMethodWithResultAsString(safeMethod);
        mockHttpMessageHandler.ResponseContent = "Dummy data";

        // Act
        var result = await safeMethodWithResultAsString.SendAsync();

        // Assert
        result.Should().BeEquivalentTo(mockHttpMessageHandler.ResponseContent);
    }
}
