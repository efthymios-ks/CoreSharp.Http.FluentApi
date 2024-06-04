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
public sealed class UnsafeMethodWithResultAsStreamTests
{
    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenCalled_ShouldNotThrow(IUnsafeMethod unsafeMethod)
    {
        // Act
        Action action = () => _ = new UnsafeMethodWithResultAsStream(unsafeMethod);

        // Assert
        action.Should().NotThrow();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenHttpResponseIsNull_ShouldReturnEmptyMemoryStream(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        IUnsafeMethod unsafeMethod)
    {
        // Arrange
        mockHttpMessageHandler.SetResponseToNull = true;
        var unsafeMethodWithResultAsStream = new UnsafeMethodWithResultAsStream(unsafeMethod);

        // Act
        using var resultAsStream = await unsafeMethodWithResultAsStream.SendAsync();

        // Assert
        resultAsStream.Should().BeOfType<MemoryStream>();
        resultAsStream.Should().HaveLength(0);
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenCalled_ShouldReturnByteArray(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        IUnsafeMethod unsafeMethod)
    {
        // Arrange
        var unsafeMethodWithResultAsStream = new UnsafeMethodWithResultAsStream(unsafeMethod);
        mockHttpMessageHandler.ResponseContent = "Dummy data";
        var expectedResultAsBytes = Encoding.UTF8.GetBytes(mockHttpMessageHandler.ResponseContent);

        // Act
        using var resultAsStream = await unsafeMethodWithResultAsStream.SendAsync();

        // Assert
        using var resultMemoryStream = new MemoryStream();
        await resultAsStream!.CopyToAsync(resultMemoryStream);
        var resultAsBytes = resultMemoryStream.ToArray();
        resultAsBytes.Should().BeEquivalentTo(expectedResultAsBytes);
    }
}
