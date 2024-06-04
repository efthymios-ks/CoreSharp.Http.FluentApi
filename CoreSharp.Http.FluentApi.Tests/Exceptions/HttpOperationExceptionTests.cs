using CoreSharp.Http.FluentApi.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Reflection;

namespace Tests.Exceptions;

[TestFixture]
public sealed class HttpOperationExceptionTests2
{
    [Test]
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
        exception.Should().NotBeNull();
        exception.RequestUrl.Should().Be(requestUrl);
        exception.RequestMethod.Should().Be(requestMethod.Method);
        exception.ResponseStatusCode.Should().Be(responseStatusCode);
        exception.ResponseContent.Should().Be(responseContent);
        exception.Message.Should().Be(responseContent);
        exception.LogEntry.Should().Be($"{requestMethod.Method} > {requestUrl} > {(int)responseStatusCode} {responseStatusCode}");
        exception.InnerException.Should().BeSameAs(innerException);
    }

    [Test]
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

        // Arrange
        debuggerDisplay.Should().Be(exception.LogEntry);
    }

    [Test]
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

        // Arrange
        toStringValue.Should().Be(exception.LogEntry);
    }

    [Test]
    public async Task CreateAsync_WhenHttpResponseMessageIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        using HttpResponseMessage httpResponse = null!;

        // Act
        Func<Task> action = () => HttpOperationException.CreateAsync(httpResponse);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
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
        exception.Should().NotBeNull();
        exception.RequestUrl.Should().Be(requestUrl);
        exception.RequestMethod.Should().Be(requestMethod.Method);
        exception.ResponseStatusCode.Should().Be(responseStatusCode);
        exception.ResponseContent.Should().Be(responseContent);
        exception.Message.Should().Be(responseContent);
        exception.LogEntry.Should().Be($"{requestMethod.Method} > {requestUrl} > {(int)responseStatusCode} {responseStatusCode}");
    }
}
