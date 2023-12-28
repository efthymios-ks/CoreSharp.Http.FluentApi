using CoreSharp.Http.FluentApi.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Extensions.Tests;

[TestFixture]
public class HttpResponseMessageExtensionsTests
{
    // Methods
    [Test]
    public async Task EnsureSuccessAsync_WhenHttpResponseMessageIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        HttpResponseMessage response = null;

        // Act
        Func<Task> action = response.EnsureSuccessAsync;

        // Assert
        await action.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Test]
    public async Task EnsureSuccessAsync_WhenStatusIsSuccessful_ShouldNotThrowException()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        // Act
        Func<Task> action = response.EnsureSuccessAsync;

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Test]
    public async Task EnsureSuccessAsync_WhenStatusIsSuccessful_ShouldThrowHttpOperationException()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);

        // Act
        Func<Task> action = response.EnsureSuccessAsync;

        // Assert
        await action.Should().ThrowExactlyAsync<HttpOperationException>();
    }
}
