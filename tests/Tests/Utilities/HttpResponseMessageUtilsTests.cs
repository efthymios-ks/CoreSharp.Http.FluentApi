using CoreSharp.Http.FluentApi.Utilities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Tests.Utilities;

[TestFixture]
public class HttpResponseMessageUtilsTests
{
    // Methods
    [Test]
    public async Task DeserializeAsync_WhenBothFunctionsAreNull_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var httpResponse = CreateHttpResponseMessage(HttpStatusCode.OK, "Content");

        // Act
        Func<Task> action = () => HttpResponseMessageUtils.DeserialeAsync<string>(httpResponse, null, null, CancellationToken.None);

        // Assert
        await action
            .Should()
            .ThrowExactlyAsync<InvalidOperationException>()
            .WithMessage("No deserialization function has been provided.");
    }

    [Test]
    public async Task DeserializeAsync_ShouldReturnResultFromStreamFunction()
    {
        // Arrange
        var expectedResult = new { Message = "Success" };
        var httpResponse = CreateHttpResponseMessage(HttpStatusCode.OK, "Stream Content");
        object DeserializeStreamFunction(Stream _) => expectedResult;

        // Act
        var result = await HttpResponseMessageUtils.DeserialeAsync(httpResponse, DeserializeStreamFunction, null, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task DeserializeAsync_ShouldReturnResultFromStringFunction()
    {
        // Arrange
        var expectedResult = new { Message = "Success" };
        var httpResponse = CreateHttpResponseMessage(HttpStatusCode.OK, "String Content");
        object DeserializeStringFunction(string _) => expectedResult;

        // Act
        var result = await HttpResponseMessageUtils.DeserialeAsync(httpResponse, null, DeserializeStringFunction, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    private static HttpResponseMessage CreateHttpResponseMessage(HttpStatusCode statusCode, string content)
        => new(statusCode)
        {
            Content = new StringContent(content),
        };
}
