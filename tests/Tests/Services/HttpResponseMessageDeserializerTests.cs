using CoreSharp.Http.FluentApi.Services;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Services;

[TestFixture]
public sealed class HttpResponseMessageDeserializerTests
{
    [Test]
    public async Task DeserializeAsync_WhenBothFunctionsAreNull_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var httpResponseMessageDeserializer = new HttpResponseMessageDeserializer();
        using var response = CreateHttpResponseMessage("Data");

        // Act
        Func<Task> action = () => _ = httpResponseMessageDeserializer.DeserializeAsync<string>(response, null, null, CancellationToken.None);

        // Assert
        await action
            .Should()
            .ThrowExactlyAsync<InvalidOperationException>()
            .WithMessage("No deserialization function has been provided.");
    }

    [Test]
    public async Task DeserializeAsync_WhenDeserializeFromStreamIsProvided_ShouldReturnResult()
    {
        // Arrange
        var httpResponseMessageDeserializer = new HttpResponseMessageDeserializer();
        var expectedResult = new { Message = "Data" };
        using var response = CreateHttpResponseMessage(expectedResult.Message);

        var deserializeFromStreamCallCount = 0;
        object DeserializeStreamFunction(Stream _)
        {
            deserializeFromStreamCallCount++;
            return expectedResult;
        }

        // Act
        var result = await httpResponseMessageDeserializer.DeserializeAsync(response, DeserializeStreamFunction, null, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        deserializeFromStreamCallCount.Should().Be(1);
    }

    [Test]
    public async Task DeserializeAsync_WhenDeserializeFromStringIsProvided_ShouldReturnResult()
    {
        // Arrange
        var httpResponseMessageDeserializer = new HttpResponseMessageDeserializer();
        var expectedResult = new { Message = "Data" };
        using var response = CreateHttpResponseMessage(expectedResult.Message);

        var deserializeStringFunctionCallCount = 0;
        object DeserializeStringFunction(string _)
        {
            deserializeStringFunctionCallCount++;
            return expectedResult;
        }

        // Act
        var result = await httpResponseMessageDeserializer.DeserializeAsync(response, null, DeserializeStringFunction, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        deserializeStringFunctionCallCount.Should().Be(1);
    }

    private static HttpResponseMessage CreateHttpResponseMessage(string content)
        => new(HttpStatusCode.OK)
        {
            Content = new StringContent(content),
        };
}
