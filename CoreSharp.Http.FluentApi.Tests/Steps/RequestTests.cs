using CoreSharp.Http.FluentApi.Services.Interfaces;
using CoreSharp.Http.FluentApi.Steps;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;

namespace CoreSharp.Http.FluentApi.Tests.Steps;

public sealed class RequestTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenHttpClientIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange 
        using HttpClient httpClient = null!;
        var cacheStorage = MockCreate<ICacheStorage>();

        // Act 
        void Action()
            => _ = new Request(httpClient, cacheStorage);

        // Assert 
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenCacheStorageIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange 
        using var httpClient = new HttpClient();
        ICacheStorage cacheStorage = null!;

        // Act 
        void Action()
            => _ = new Request(httpClient, cacheStorage);

        // Assert 
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenCalled_ShouldSetProperties()
    {
        // Arrange 
        using var httpClient = new HttpClient();
        var cacheStorage = MockCreate<ICacheStorage>();

        // Act 
        var request = new Request(httpClient, cacheStorage);

        // Assert 
        var requestInterface = (IRequest)request;
        Assert.Same(cacheStorage, requestInterface.CacheStorage);
        Assert.Same(httpClient, requestInterface.HttpClient);
        Assert.NotNull(requestInterface.Headers);
        Assert.Empty(requestInterface.Headers);
        Assert.True(requestInterface.ThrowOnError);
        Assert.Equal(HttpCompletionOption.ResponseHeadersRead, requestInterface.HttpCompletionOption);
        Assert.Null(requestInterface.Timeout);
    }

    [Fact]
    public void WithHeaders_WhenHeadersIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var request = MockCreate<Request>();
        IDictionary<string, string> headers = null!;

        // Act 
        void Action()
            => request.WithHeaders(headers);

        // Assert 
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void WithHeaders_WhenCalled_ShouldSetHeaders()
    {
        // Arrange
        var request = MockCreate<Request>();
        var headers = new Dictionary<string, string>
                {
                    { "key1", "value1" },
                    { "key2", "value2" }
                };

        // Act 
        var requestReturned = request.WithHeaders(headers);

        // Assert 
        var requestInterface = (IRequest)request;
        Assert.Equivalent(headers, requestInterface.Headers);
        Assert.Same(request, requestReturned);
    }

    [Fact]
    public void WithHeader_WhenKeyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var request = MockCreate<Request>();
        const string key = null!;
        const string value = "value";

        // Act 
        void Action()
            => request.WithHeader(key, value);

        // Assert 
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void WithHeader_WhenKeyIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var request = MockCreate<Request>();
        const string key = "";
        const string value = "value";

        // Act 
        void Action()
            => request.WithHeader(key, value);

        // Assert 
        Assert.Throws<ArgumentException>(Action);
    }

    [Fact]
    public void WithHeader_WhenCalled_ShouldSetHeader()
    {
        // Arrange
        var request = MockCreate<Request>();
        const string key = "key";
        const string value = "value";

        // Act 
        var requestReturned = request.WithHeader(key, value);

        // Assert 
        Assert.Same(request, requestReturned);
        var requestInterface = (IRequest)request;
        Assert.True(requestInterface.Headers.ContainsKey(key));
        Assert.Equal(value, requestInterface.Headers[key]);
    }

    [Fact]
    public void Accept_WhenCalled_ShouldSetAcceptHeader()
    {
        // Arrange
        var request = MockCreate<Request>();
        var mediaTypeName = MediaTypeNames.Application.Json;

        // Act 
        var requestReturned = request.Accept(mediaTypeName);

        // Assert 
        Assert.Same(request, requestReturned);
        var requestInterface = (IRequest)request;
        Assert.Contains(requestInterface.Headers, header
            => header.Key == HeaderNames.Accept
            && header.Value == mediaTypeName);
    }

    [Fact]
    public void AcceptJson_WhenCalled_ShouldSetAcceptJsonHeader()
    {
        // Arrange
        var request = MockCreate<Request>();

        // Act 
        var requestReturned = request.AcceptJson();

        // Assert 
        Assert.Same(request, requestReturned);
        var requestInterface = (IRequest)request;
        Assert.Contains(requestInterface.Headers, header
            => header.Key == HeaderNames.Accept
            && header.Value == MediaTypeNames.Application.Json);
    }

    [Fact]
    public void AcceptXml_WhenCalled_ShouldSetAcceptXmlHeader()
    {
        // Arrange
        var request = MockCreate<Request>();

        // Act 
        var requestReturned = request.AcceptXml();

        // Assert 
        Assert.Same(request, requestReturned);
        var requestInterface = (IRequest)request;
        Assert.Contains(requestInterface.Headers, header
            => header.Key == HeaderNames.Accept
            && header.Value == MediaTypeNames.Application.Xml);
    }

    [Fact]
    public void WithAuthorization_WhenCalled_ShouldSetAuthorizationHeader()
    {
        // Arrange
        var request = MockCreate<Request>();
        var authorization = "authorization";

        // Act 
        var requestReturned = request.WithAuthorization(authorization);

        // Assert 
        Assert.Same(request, requestReturned);
        var requestInterface = (IRequest)request;
        Assert.Contains(requestInterface.Headers, header
            => header.Key == HeaderNames.Authorization
            && header.Value == authorization);
    }

    [Fact]
    public void WithBearerToken_WhenCalled_ShouldSetBearerTokenHeader()
    {
        // Arrange
        var request = MockCreate<Request>();
        var accessToken = "accessToken";

        // Act 
        var requestReturned = request.WithBearerToken(accessToken);

        // Assert 
        Assert.Same(request, requestReturned);
        var requestInterface = (IRequest)request;
        Assert.Contains(requestInterface.Headers, header
            => header.Key == HeaderNames.Authorization
            && header.Value == $"Bearer {accessToken}");
    }

    [Fact]
    public void IgnoreError_WhenCalled_ShouldSetThrowOnErrorToFalse()
    {
        // Arrange
        var request = MockCreate<Request>();

        // Act 
        var requestReturned = request.IgnoreError();

        // Assert 
        Assert.Same(request, requestReturned);
        var requestInterface = (IRequest)request;
        Assert.False(requestInterface.ThrowOnError);
    }

    [Fact]
    public void WithCompletionOption_WhenCalled_ShouldSetCompletionOption()
    {
        // Arrange
        var request = MockCreate<Request>();
        var completionOption = HttpCompletionOption.ResponseContentRead;

        // Act 
        var requestReturned = request.WithCompletionOption(completionOption);

        // Assert 
        Assert.Same(request, requestReturned);
        var requestInterface = (IRequest)request;
        Assert.Equal(completionOption, requestInterface.HttpCompletionOption);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void WithTimeout_WhenTimeoutIsZeroOrLess_ShouldThrowArgumentOutOfRangeException(int timeoutSeconds)
    {
        // Arrange 
        using var httpClient = new HttpClient();
        var cacheStorage = MockCreate<ICacheStorage>();
        var request = new Request(httpClient, cacheStorage);
        var timeout = TimeSpan.FromSeconds(timeoutSeconds);

        // Act 
        void Action()
            => request.WithTimeout(timeout);

        // Assert 
        Assert.Throws<ArgumentOutOfRangeException>(Action);
    }

    [Fact]
    public void WithTimeout_WhenTimeoutIsInfinite_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var request = MockCreate<Request>();
        var timeout = Timeout.InfiniteTimeSpan;

        // Act 
        void Action()
            => request.WithTimeout(timeout);

        // Assert 
        Assert.Throws<ArgumentOutOfRangeException>(Action);
    }

    [Fact]
    public void WithTimeout_WhenCalled_ShouldSetTimeout()
    {
        // Arrange
        var request = MockCreate<Request>();
        var timeout = TimeSpan.FromSeconds(10);

        // Act 
        var requestReturned = request.WithTimeout(timeout);

        // Assert 
        Assert.Same(request, requestReturned);
        var requestInterface = (IRequest)request;
        Assert.Equal(timeout, requestInterface.Timeout);
    }

    [Fact]
    public void WithEndpoint_WhenIntKeyIsProvided_ShouldSetEndpoint()
    {
        // Arrange
        var request = MockCreate<Request>();
        const string resourceName = "students";
        const int key = 1;
        var expectedResourceName = $"{resourceName}/{key}/";

        // Act 
        var endpoint = request.WithEndpoint(resourceName, key);

        // Assert 
        Assert.NotNull(endpoint);
        Assert.IsType<Endpoint>(endpoint);
        Assert.Equal(expectedResourceName, endpoint.Endpoint);
    }

    [Fact]
    public void WithEndpoint_WhenLongKeyisprovided_ShouldSetEndpoint()
    {
        // Arrange
        var request = MockCreate<Request>();
        const string resourceName = "students";
        const long key = 1;
        var expectedResourceName = $"{resourceName}/{key}/";

        // Act 
        var endpoint = request.WithEndpoint(resourceName, key);

        // Assert 
        Assert.NotNull(endpoint);
        Assert.IsType<Endpoint>(endpoint);
        Assert.Equal(expectedResourceName, endpoint.Endpoint);
    }

    [Fact]
    public void WithEndpoint_WhenGuidKeyIsProvided_ShouldSetEndpoint()
    {
        // Arrange
        var request = MockCreate<Request>();
        const string resourceName = "students";
        var key = Guid.NewGuid();
        var expectedResourceName = $"{resourceName}/{key}/";

        // Act 
        var endpoint = request.WithEndpoint(resourceName, key);

        // Assert 
        Assert.NotNull(endpoint);
        Assert.IsType<Endpoint>(endpoint);
        Assert.Equal(expectedResourceName, endpoint.Endpoint);
    }

    [Fact]
    public void WithEndpoint_WhenStringKeyIsProvided_ShouldSetEndpoint()
    {
        // Arrange
        var request = MockCreate<Request>();
        const string resourceName = "students";
        const string key = "1";
        var expectedResourceName = $"{resourceName}/{key}/";

        // Act 
        var endpoint = request.WithEndpoint(resourceName, key);

        // Assert 
        Assert.NotNull(endpoint);
        Assert.IsType<Endpoint>(endpoint);
        Assert.Equal(expectedResourceName, endpoint.Endpoint);
    }

    [Fact]
    public void WithEndpoint_WhenSegmentsIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var request = MockCreate<Request>();
        IEnumerable<string> segments = null!;

        // Act 
        void Action()
            => request.WithEndpoint(segments);

        // Assert 
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void WithEndpoint_WhenSegmentsIsNotNull_ShouldSetEndpoint()
    {
        // Arrange
        var request = MockCreate<Request>();
        var segments = new[] { "/students/", "/1//" };
        var expectedResourceName = "students/1/";

        // Act 
        var endpoint = request.WithEndpoint(segments);

        // Assert 
        Assert.NotNull(endpoint);
        Assert.IsType<Endpoint>(endpoint);
        Assert.Equal(expectedResourceName, endpoint.Endpoint);
    }

    [Fact]
    public void WithEndpoint_WhenResourceNameIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var request = MockCreate<Request>();
        string resourceName = null!;

        // Act 
        void Action()
            => request.WithEndpoint(resourceName);

        // Assert 
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void WithEndpoint_WhenResourceNameIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var request = MockCreate<Request>();
        const string resourceName = "";

        // Act 
        void Action()
            => request.WithEndpoint(resourceName);

        // Assert 
        Assert.Throws<ArgumentException>(Action);
    }

    [Fact]
    public void WithEndpoint_WhenResourceNameIsNotNullOrEmpty_ShouldSetEndpoint()
    {
        // Arrange
        var request = MockCreate<Request>();
        const string resourceName = "students";
        const string expectedResourceName = "students/";

        // Act 
        var endpoint = request.WithEndpoint(resourceName);

        // Assert 
        Assert.NotNull(endpoint);
        Assert.IsType<Endpoint>(endpoint);
        Assert.Equal(expectedResourceName, endpoint.Endpoint);
    }
}
