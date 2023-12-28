using CoreSharp.Http.FluentApi.Steps.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace CoreSharp.Http.FluentApi.Extensions.Tests;

[TestFixture]
public class HttpClientExtensionsTests
{
    // Methods
    [Test]
    public void Request_WhenHttpClientIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        HttpClient httpClient = null;

        // Act
        Action action = () => httpClient.Request();

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Request_WhenCalled_ShouldReturnIRequest()
    {
        // Arrange
        var httpClient = new HttpClient();

        // Act
        var request = httpClient.Request();

        // Assert
        request.Should().NotBeNull();
        request.Should().BeAssignableTo<IRequest>();
    }
}
