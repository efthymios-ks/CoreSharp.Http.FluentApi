using CoreSharp.Http.FluentApi.Extensions;
using CoreSharp.Http.FluentApi.Steps;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace Tests.Extensions;

[TestFixture]
public sealed class HttpClientExtensionsTests
{
    // Methods
    [Test]
    public void Request_WhenHttpClientIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        using HttpClient httpClient = null;

        // Act
        Action action = () => _ = httpClient.Request();

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Request_WhenCalled_ShouldReturnIRequest()
    {
        // Arrange
        using var httpClient = new HttpClient();

        // Act
        var request = httpClient.Request();

        // Assert
        request.Should().NotBeNull();
        request.Should().BeOfType<Request>();
    }
}
