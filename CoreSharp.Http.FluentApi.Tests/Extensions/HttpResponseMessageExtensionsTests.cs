using CoreSharp.Http.FluentApi.Exceptions;
using CoreSharp.Http.FluentApi.Extensions;
using System.Net;
using Xunit.Abstractions;

namespace CoreSharp.Http.FluentApi.Tests.Extensions;

public sealed class HttpResponseMessageExtensionsTests(ITestOutputHelper testOutputHelper)
    : ProjectTestsBase(testOutputHelper)
{
    [Fact]
    public async Task EnsureSuccessAsync_WhenHttpResponseMessageIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        using HttpResponseMessage response = null!;

        // Act
        Task Action()
            => response.EnsureSuccessAsync();

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
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
        Task Action()
            => response.EnsureSuccessAsync();

        // Assert
        var exception = await Record.ExceptionAsync(Action);
        Assert.Null(exception);
    }

    [Fact]
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
        Task Action()
            => response.EnsureSuccessAsync();

        // Assert
        await Assert.ThrowsAsync<HttpOperationException>(Action);
    }

    [Fact]
    public async Task EnsureSuccessAsync_WhenStatusIsNotSuccessful_ShouldDisposeContent()
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Get, @"http://www.example.com/api");
        using var content = MockCreate<HttpContent>();
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
