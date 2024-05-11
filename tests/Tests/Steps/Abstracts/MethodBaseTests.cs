using AutoFixture.NUnit3;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using CoreSharp.Http.FluentApi.Steps.Methods.Abstracts;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tests.Internal.Attributes;
using Tests.Internal.HttpmessageHandlers;

namespace Tests.Steps.Abstracts;

[TestFixture]
public sealed class MethodBaseTests
{
    [Test]
    public void Constructor_WhenEndpointIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEndpoint endpoint = null;
        var httpMethod = HttpMethod.Get;

        // Act
        Action action = () => _ = new DummyMethod(endpoint, httpMethod);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenHttpMethodIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var endpoint = Substitute.For<IEndpoint>();
        HttpMethod httpMethod = null;

        // Act
        Action action = () => _ = new DummyMethod(endpoint, httpMethod);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalled_ShouldSetProperties()
    {
        // Arrange
        var endpoint = Substitute.For<IEndpoint>();
        var httpMethod = HttpMethod.Get;

        // Act
        var method = new DummyMethod(endpoint, httpMethod);

        // Assert
        var methodInterface = (IMethod)method;
        methodInterface.Endpoint.Should().BeSameAs(endpoint);
        methodInterface.HttpMethod.Should().BeSameAs(httpMethod);
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenHasQueryParameters_ShouldAppendQueryParametersToEndpoint(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        [Frozen] IRequest request,
        IEndpoint endpoint)
    {
        // Arrange 
        var httpMethod = HttpMethod.Get;
        var method = new DummyMethod(endpoint, httpMethod);
        request.QueryParameters.Add("key1", "value1");

        // Act
        _ = await method.SendAsync();

        // Assert
        var capturedUrl = mockHttpMessageHandler.CapturedRequest.RequestUri.AbsoluteUri;
        capturedUrl.Should().Contain("key1=value1");
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenHasHeaders_ShouldAddHeadersToRequest(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        [Frozen] IRequest request,
        IEndpoint endpoint)
    {
        // Arrange 
        var httpMethod = HttpMethod.Get;
        var method = new DummyMethod(endpoint, httpMethod);
        request.Headers.Add("key1", "value1");

        // Act
        _ = await method.SendAsync();

        // Assert
        var capturedHeaders = mockHttpMessageHandler.CapturedRequest.Headers;
        capturedHeaders.Should().Contain(header =>
            header.Key == "key1"
            && header.Value.FirstOrDefault() == "value1");
    }

    [Test]
    [AutoNSubstituteData]
    public void SendAsync_WhenCancellationIsRequested_ShouldThrowTaskCancelledException(IEndpoint endpoint)
    {
        // Arrange 
        var httpMethod = HttpMethod.Get;
        var method = new DummyMethod(endpoint, httpMethod);
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act
        Func<Task> action = () => _ = method.SendAsync(cancellationTokenSource.Token);

        // Assert
        action.Should().ThrowExactlyAsync<TaskCanceledException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void SendAsync_WhenTimedOut_ShouldThrowTimeoutException(IEndpoint endpoint)
    {
        // Arrange 
        var httpMethod = HttpMethod.Get;
        var method = new DummyMethod(endpoint, httpMethod);
        endpoint.Request.Timeout = TimeSpan.FromTicks(1);

        // Act
        Func<Task> action = () => _ = method.SendAsync();

        // Assert
        action.Should().ThrowExactlyAsync<TimeoutException>();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenIgnoreErrorIsFalseAndThrowsException_ShouldThrowException(IEndpoint endpoint)
    {
        // Arrange 
        var httpMethod = HttpMethod.Get;
        var method = new DummyMethod(endpoint, httpMethod);
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
        endpoint.Request.ThrowOnError = true;

        // Act
        Func<Task<HttpResponseMessage>> action = () => _ = method.SendAsync(cancellationTokenSource.Token);

        // Assert
        await action.Should().ThrowExactlyAsync<TaskCanceledException>();

    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenIgnoreErrorIsTrueAndThrowsException_ShouldNotThrowExceptionAndReturnNull(IEndpoint endpoint)
    {
        // Arrange 
        var httpMethod = HttpMethod.Get;
        var method = new DummyMethod(endpoint, httpMethod);
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
        endpoint.Request.ThrowOnError = false;

        // Act
        Func<Task<HttpResponseMessage>> action = () => _ = method.SendAsync(cancellationTokenSource.Token);

        // Assert
        var response = (await action.Should().NotThrowAsync()).Subject;
        response.Should().BeNull();

    }

    private sealed class DummyMethod(IEndpoint endpoint, HttpMethod httpMethod)
        : MethodBase(endpoint, httpMethod)
    {
    }
}
