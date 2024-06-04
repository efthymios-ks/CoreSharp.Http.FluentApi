using CoreSharp.Http.FluentApi.Exceptions;
using CoreSharp.Http.FluentApi.Extensions;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Net;

namespace Tests.Extensions;

[TestFixture]
public sealed class HttpResponseMessageExtensionsTests
{
    [Test]
    public async Task EnsureSuccessAsync_WhenHttpResponseMessageIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        using HttpResponseMessage response = null!;

        // Act
        Func<Task> action = response.EnsureSuccessAsync;

        // Assert
        await action.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Test]
    public async Task EnsureSuccessAsync_WhenStatusIsSuccessful_ShouldNotThrowException()
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Get, @"http://www.example.com/api");
        using var content = new StringContent("Data");
        using var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            RequestMessage = request,
            Content = content
        };

        // Act
        Func<Task> action = response.EnsureSuccessAsync;

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Test]
    public async Task EnsureSuccessAsync_WhenStatusIsNotSuccessful_ShouldThrowHttpOperationException()
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Get, @"http://www.example.com/api");
        using var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            RequestMessage = request
        };
        response.Content = null;

        // Act
        Func<Task> action = response.EnsureSuccessAsync;

        // Assert
        await action.Should().ThrowExactlyAsync<HttpOperationException>();
    }

    [Test]
    public async Task EnsureSuccessAsync_WhenStatusIsNotSuccessful_ShouldDisposeContent()
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Get, @"http://www.example.com/api");
        using var content = Substitute.For<HttpContent>();
        using var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            RequestMessage = request,
            Content = content
        };

        // Act
        try
        {
            await response.EnsureSuccessAsync();
        }
        catch
        {
            // Ignore
        }

        // Assert 
        content
            .Received(1)
            .Dispose();
    }
}
