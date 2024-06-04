using AutoFixture.NUnit3;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;
using FluentAssertions;
using NUnit.Framework;
using System.Text;
using Tests.Internal.Attributes;
using Tests.Internal.HttpmessageHandlers;

namespace Tests.Steps.UnsafeMethods;

[TestFixture]
public sealed class UnsafeMethodWithResultAsBytesTests
{
    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenCalled_ShouldNotThrow(IUnsafeMethod unsafeMethod)
    {
        // Act
        Action action = () => _ = new UnsafeMethodWithResultAsBytes(unsafeMethod);

        // Assert
        action.Should().NotThrow();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenHttpResponseIsNull_ShouldReturnEmptyArray(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        IUnsafeMethod unsafeMethod)
    {
        // Arrange
        mockHttpMessageHandler.SetResponseToNull = true;
        var safeMethodWithResultAsBytes = new UnsafeMethodWithResultAsBytes(unsafeMethod);

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
        IUnsafeMethod unsafeMethod)
    {
        // Arrange
        var safeMethodWithResultAsBytes = new UnsafeMethodWithResultAsBytes(unsafeMethod);
        mockHttpMessageHandler.ResponseContent = "Dummy data";
        var expectedResult = Encoding.UTF8.GetBytes(mockHttpMessageHandler.ResponseContent);

        // Act
        var result = await safeMethodWithResultAsBytes.SendAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}
