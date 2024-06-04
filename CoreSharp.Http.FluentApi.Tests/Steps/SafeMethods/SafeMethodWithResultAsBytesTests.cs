using AutoFixture.NUnit3;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using FluentAssertions;
using NUnit.Framework;
using System.Text;
using Tests.Internal.Attributes;
using Tests.Internal.HttpmessageHandlers;

namespace Tests.Steps.SafeMethods;

[TestFixture]
public sealed class SafeMethodWithResultAsBytesTests
{
    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenCalled_ShouldNotThrow(ISafeMethod safeMethod)
    {
        // Act
        Action action = () => _ = new SafeMethodWithResultAsBytes(safeMethod);

        // Assert
        action.Should().NotThrow();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithCache_WhenCalled_ShouldReturnSafeMethodWithResultAsBytesAndCache(ISafeMethod safeMethod)
    {
        // Arrange 
        var safeMethodWithResultAsBytes = new SafeMethodWithResultAsBytes(safeMethod);
        var cacheDuration = TimeSpan.FromSeconds(1);

        // Act
        var result = safeMethodWithResultAsBytes.WithCache(cacheDuration);

        // Assert
        result.Should().BeOfType<SafeMethodWithResultAsBytesAndCache>();
        result.CacheDuration.Should().Be(cacheDuration);
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenHttpResponseIsNull_ShouldReturnEmptyArray(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        ISafeMethod safeMethod)
    {
        // Arrange
        mockHttpMessageHandler.SetResponseToNull = true;
        var safeMethodWithResultAsBytes = new SafeMethodWithResultAsBytes(safeMethod);

        // Act
        var result = await safeMethodWithResultAsBytes.SendAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenCalled_ShouldReturnByteArray(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        ISafeMethod safeMethod)
    {
        // Arrange
        var safeMethodWithResultAsBytes = new SafeMethodWithResultAsBytes(safeMethod);
        mockHttpMessageHandler.ResponseContent = "Dummy data";
        var expectedResult = Encoding.UTF8.GetBytes(mockHttpMessageHandler.ResponseContent);

        // Act
        var result = await safeMethodWithResultAsBytes.SendAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}
