using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Exceptions.Tests;

[TestFixture]
public class HttpOperationExceptionTests
{
    [Test]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        const string requestUrl = "https://example.com/api";
        var requestMethod = HttpMethod.Get;
        const HttpStatusCode responseStatusCode = HttpStatusCode.OK;
        var responseContent = responseStatusCode.ToString();

        // Act
        var exception = new HttpOperationException(requestUrl, requestMethod, responseStatusCode, responseContent);

        // Assert
        exception.Should().NotBeNull();
        exception.RequestUrl.Should().Be(requestUrl);
        exception.RequestMethod.Should().Be(requestMethod.Method);
        exception.ResponseStatusCode.Should().Be(responseStatusCode);
        exception.ResponseContent.Should().Be(responseContent);
        exception.Message.Should().Be(responseContent);
        exception.LogEntry.Should().Be($"{requestMethod.Method} > {requestUrl} > {(int)responseStatusCode} {responseStatusCode}");
    }

    [Test]
    public async Task CreateAsync_WhenHttpResponseMessageIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        HttpResponseMessage httpResponse = null;

        // Act
        Func<Task> action = () => HttpOperationException.CreateAsync(httpResponse);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task CreateAsync_WhenHttpResponseMessageHasValue_ShouldCreateInstanceFromHttpResponseMessage()
    {
        // Arrange
        const string requestUrl = "https://example.com/api";
        var requestMethod = HttpMethod.Get;
        const HttpStatusCode responseStatusCode = HttpStatusCode.OK;
        var responseContent = responseStatusCode.ToString();
        var httpResponseMessage = new HttpResponseMessage(responseStatusCode)
        {
            RequestMessage = new HttpRequestMessage(requestMethod, requestUrl),
            Content = new StringContent(responseContent),
        };

        // Act
        var exception = await HttpOperationException.CreateAsync(httpResponseMessage);

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
