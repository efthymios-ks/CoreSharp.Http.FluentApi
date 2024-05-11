using AutoFixture.NUnit3;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tests.Internal.Attributes;
using Tests.Internal.HttpmessageHandlers;

namespace Tests.Steps.SafeMethods;

[TestFixture]
public sealed class SafeMethodWithResultAsStreamTests
{
    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenCalled_ShouldNotThrow(ISafeMethod safeMethod)
    {
        // Act
        Action action = () => _ = new SafeMethodWithResultAsStream(safeMethod);

        // Assert
        action.Should().NotThrow();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithCache_WhenCalled_ShouldReturnSafeMethodWithResultAsStreamAndCache(ISafeMethod safeMethod)
    {
        // Arrange 
        var safeMethodWithResultAsStream = new SafeMethodWithResultAsStream(safeMethod);
        var cacheDuration = TimeSpan.FromSeconds(1);

        // Act
        var result = safeMethodWithResultAsStream.WithCache(cacheDuration);

        // Assert
        result.Should().BeOfType<SafeMethodWithResultAsStreamAndCache>();
        result.CacheDuration.Should().Be(cacheDuration);
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenCalled_ShouldReturnByteArray(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        ISafeMethod safeMethod)
    {
        // Arrange
        var safeMethodWithResultAsStream = new SafeMethodWithResultAsStream(safeMethod);
        mockHttpMessageHandler.ResponseContent = "Dummy data";
        var expectedResultAsBytes = Encoding.UTF8.GetBytes(mockHttpMessageHandler.ResponseContent);

        // Act
        using var resultAsStream = await safeMethodWithResultAsStream.SendAsync();

        // Assert
        using var resultMemoryStream = new MemoryStream();
        await resultAsStream.CopyToAsync(resultMemoryStream);
        var resultAsBytes = resultMemoryStream.ToArray();
        resultAsBytes.Should().BeEquivalentTo(expectedResultAsBytes);
    }
}
