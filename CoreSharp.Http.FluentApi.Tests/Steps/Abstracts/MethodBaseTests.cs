using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using CoreSharp.Http.FluentApi.Steps.Methods.Abstracts;
using Tests.Common.Mocks;

namespace CoreSharp.Http.FluentApi.Tests.Steps.Abstracts;

public sealed class MethodBaseTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenEndpointIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEndpoint endpoint = null!;
        var httpMethod = HttpMethod.Get;

        // Act
        void Action()
            => _ = new DummyMethod(endpoint, httpMethod);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenHttpMethodIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var endpoint = MockCreate<IEndpoint>();
        HttpMethod httpMethod = null!;

        // Act
        void Action()
            => _ = new DummyMethod(endpoint, httpMethod);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenCalled_ShouldSetProperties()
    {
        // Arrange
        var endpoint = MockCreate<IEndpoint>();
        var httpMethod = HttpMethod.Get;

        // Act
        var method = new DummyMethod(endpoint, httpMethod);

        // Assert
        var methodInterface = (IMethod)method;
        Assert.Same(endpoint, methodInterface.Endpoint);
        Assert.Same(httpMethod, methodInterface.HttpMethod);
    }

    [Fact]
    public async Task SendAsync_WhenHasQueryParameters_ShouldAppendQueryParametersToEndpoint()
    {
        // Arrange 
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var endpoint = MockCreate<IEndpoint>();
        var httpMethod = HttpMethod.Get;
        var method = new DummyMethod(endpoint, httpMethod);
        endpoint.QueryParameters.Add("key1", "value1");

        // Act
        await method.SendAsync();

        // Assert
        var capturedUrl = mockHttpMessageHandler.CapturedRequest!.RequestUri!.AbsoluteUri;
        Assert.Contains("key1=value1", capturedUrl);
    }

    [Fact]
    public async Task SendAsync_WhenHasHeaders_ShouldAddHeadersToRequest()
    {
        // Arrange 
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var request = MockFreeze<IRequest>();
        var endpoint = MockCreate<IEndpoint>();
        var httpMethod = HttpMethod.Get;
        var method = new DummyMethod(endpoint, httpMethod);
        request.Headers.Add("key1", "value1");

        // Act
        await method.SendAsync();

        // Assert
        var capturedHeaders = mockHttpMessageHandler.CapturedRequest!.Headers;
        Assert.Contains(capturedHeaders, header =>
            header.Key == "key1"
            && header.Value.FirstOrDefault() == "value1");
    }

    [Fact]
    public void SendAsync_WhenCancellationIsRequested_ShouldThrowTaskCancelledException()
    {
        // Arrange 
        var endpoint = MockCreate<IEndpoint>();
        var httpMethod = HttpMethod.Get;
        var method = new DummyMethod(endpoint, httpMethod);
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act
        Task Action()
            => method.SendAsync(cancellationTokenSource.Token);

        // Assert
        Assert.ThrowsAsync<TaskCanceledException>(Action);
    }

    [Fact]
    public void SendAsync_WhenTimedOut_ShouldThrowTimeoutException()
    {
        // Arrange 
        var endpoint = MockCreate<IEndpoint>();
        var httpMethod = HttpMethod.Get;
        var method = new DummyMethod(endpoint, httpMethod);
        endpoint.Request!.Timeout = TimeSpan.FromTicks(1);

        // Act
        Task Action()
            => method.SendAsync();

        // Assert
        Assert.ThrowsAsync<TimeoutException>(Action);
    }

    [Fact]
    public async Task SendAsync_WhenIgnoreErrorIsFalseAndThrowsException_ShouldThrowException()
    {
        // Arrange 
        var endpoint = MockCreate<IEndpoint>();
        var httpMethod = HttpMethod.Get;
        var method = new DummyMethod(endpoint, httpMethod);
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
        endpoint.Request!.ThrowOnError = true;

        // Act
        Task<HttpResponseMessage?> Action()
            => method.SendAsync(cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(Action);
    }

    [Fact]
    public async Task SendAsync_WhenIgnoreErrorIsTrueAndThrowsException_ShouldNotThrowExceptionAndReturnNull()
    {
        // Arrange 
        var endpoint = MockCreate<IEndpoint>();
        var httpMethod = HttpMethod.Get;
        var method = new DummyMethod(endpoint, httpMethod);
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
        endpoint.Request!.ThrowOnError = false;

        // Act
        Task<HttpResponseMessage?> Action()
            => method.SendAsync(cancellationTokenSource.Token);

        // Assert
        var response = await Record.ExceptionAsync(Action);
        Assert.Null(response);
    }

    private sealed class DummyMethod(IEndpoint endpoint, HttpMethod httpMethod)
        : MethodBase(endpoint, httpMethod)
    {
    }
}
