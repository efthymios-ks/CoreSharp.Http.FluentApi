using AutoFixture.NUnit3;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;
using FluentAssertions;
using NUnit.Framework;
using Tests.Internal.Attributes;
using Tests.Internal.HttpmessageHandlers;

namespace Tests.Steps.UnsafeMethods;

[TestFixture]
public sealed class UnsafeMethodWithResultAsStringTests
{
    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenCalled_ShouldNotThrow(IUnsafeMethod unsafeMethod)
    {
        // Act
        Action action = () => _ = new UnsafeMethodWithResultAsString(unsafeMethod);

        // Assert
        action.Should().NotThrow();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenHttpResponseIsNull_ShouldReturnNull(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        IUnsafeMethod unsafeMethod)
    {
        // Arrange
        mockHttpMessageHandler.SetResponseToNull = true;
        var unsafeMethodWithResultAsString = new UnsafeMethodWithResultAsString(unsafeMethod);

        // Act
        var result = await unsafeMethodWithResultAsString.SendAsync();

        // Assert
        result.Should().BeNull();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenCalled_ShouldReturnByteArray(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        IUnsafeMethod unsafeMethod)
    {
        // Arrange
        var unsafeMethodWithResultAsString = new UnsafeMethodWithResultAsString(unsafeMethod);
        mockHttpMessageHandler.ResponseContent = "Dummy data";

        // Act
        var result = await unsafeMethodWithResultAsString.SendAsync();

        // Assert
        result.Should().BeEquivalentTo(mockHttpMessageHandler.ResponseContent);
    }
}
