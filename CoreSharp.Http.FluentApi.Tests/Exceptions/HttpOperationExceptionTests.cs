using CoreSharp.Http.FluentApi.Exceptions;
using System.Net;
using System.Reflection;

namespace CoreSharp.Http.FluentApi.Tests.Exceptions;

public sealed class HttpOperationExceptionTests
{
    [Fact]
    public void Constructor_WhenCalled_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        const string requestUrl = "https://example.com/api";
        var requestMethod = HttpMethod.Get;
        const HttpStatusCode responseStatusCode = HttpStatusCode.OK;
        var responseContent = responseStatusCode.ToString();
        var innerException = new Exception("Inner");

        // Act
        var exception = new HttpOperationException(
            requestUrl,
            requestMethod,
            responseStatusCode,
            responseContent,
            innerException);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(requestUrl, exception.RequestUrl);
        Assert.Equal(requestMethod.Method, exception.RequestMethod);
        Assert.Equal(responseStatusCode, exception.ResponseStatusCode);
        Assert.Equal(responseContent, exception.ResponseContent);
        Assert.Equal(responseContent, exception.Message);
        Assert.Equal($"{requestMethod.Method} > {requestUrl} > {(int)responseStatusCode} {responseStatusCode}", exception.LogEntry);
        Assert.Same(innerException, exception.InnerException);
    }

    [Fact]
    public void DebuggerDisplay_WhenCalled_ShouldReturnLogEntry()
    {
        // Act
        var exception = new HttpOperationException(
            "https://example.com/api",
            HttpMethod.Get,
            HttpStatusCode.OK,
            "Content");

        // Act
        var debuggerDisplay = (string?)exception
            .GetType()
            .GetProperty("DebuggerDisplay", BindingFlags.Instance | BindingFlags.NonPublic)
            ?.GetValue(exception);

        // Assert
        Assert.Equal(exception.LogEntry, debuggerDisplay);
    }

    [Fact]
    public void ToString_WhenCalled_ShouldReturnLogEntry()
    {
        // Act
        var exception = new HttpOperationException(
            "https://example.com/api",
            HttpMethod.Get,
            HttpStatusCode.OK,
            "Content");

        // Act
        var toStringValue = exception.ToString();

        // Assert
        Assert.Equal(exception.LogEntry, toStringValue);
    }

    [Fact]
    public async Task CreateAsync_WhenHttpResponseMessageIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        using HttpResponseMessage httpResponse = null!;

        // Act
        async Task Action()
            => await HttpOperationException.CreateAsync(httpResponse);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task CreateAsync_WhenHttpResponseMessageIsNotNull_ShouldCreateInstanceFromHttpResponseMessage()
    {
        // Arrange 
        const string requestUrl = "https://example.com/api";
        var requestMethod = HttpMethod.Get;
        const HttpStatusCode responseStatusCode = HttpStatusCode.OK;
        var responseContent = responseStatusCode.ToString();

        using var request = new HttpRequestMessage(requestMethod, requestUrl);
        using var content = new StringContent(responseContent);
        using var response = new HttpResponseMessage(responseStatusCode)
        {
            RequestMessage = request,
            Content = content
        };

        // Act
        var exception = await HttpOperationException.CreateAsync(response);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(requestUrl, exception.RequestUrl);
        Assert.Equal(requestMethod.Method, exception.RequestMethod);
        Assert.Equal(responseStatusCode, exception.ResponseStatusCode);
        Assert.Equal(responseContent, exception.ResponseContent);
        Assert.Equal(responseContent, exception.Message);
        Assert.Equal($"{requestMethod.Method} > {requestUrl} > {(int)responseStatusCode} {responseStatusCode}", exception.LogEntry);
    }
}
