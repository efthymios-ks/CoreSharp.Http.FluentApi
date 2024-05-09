using CoreSharp.Http.FluentApi.Services.Interfaces;
using CoreSharp.Http.FluentApi.Steps;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Utilities;
using FluentAssertions;
using Microsoft.Net.Http.Headers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using Tests.Internal.Attributes;

namespace Tests.Steps;

[TestFixture]
public sealed class RequestTests
{
    [Test]
    public void Constructor_WhenHttpClientIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange 
        using HttpClient httpClient = null;
        var cacheStorage = Substitute.For<ICacheStorage>();
        var httpResponseMessageDeserializer = Substitute.For<IHttpResponseMessageDeserializer>();

        // Act 
        Action action = () => _ = new Request(httpClient, cacheStorage, httpResponseMessageDeserializer);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCacheStorageIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange 
        using var httpClient = new HttpClient();
        ICacheStorage cacheStorage = null;
        var httpResponseMessageDeserializer = Substitute.For<IHttpResponseMessageDeserializer>();

        // Act 
        Action action = () => _ = new Request(httpClient, cacheStorage, httpResponseMessageDeserializer);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenHttpResponseMessageDeserializerIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange 
        using var httpClient = new HttpClient();
        ICacheStorage cacheStorage = null;
        IHttpResponseMessageDeserializer httpResponseMessageDeserializer = null;

        // Act 
        Action action = () => _ = new Request(httpClient, cacheStorage, httpResponseMessageDeserializer);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenCalled_ShouldSetProperties()
    {
        // Arrange 
        using var httpClient = new HttpClient();
        var cacheStorage = Substitute.For<ICacheStorage>();
        var httpResponseMessageDeserializer = Substitute.For<IHttpResponseMessageDeserializer>();

        // Act 
        var request = new Request(httpClient, cacheStorage, httpResponseMessageDeserializer);

        // Assert 
        var requestInterface = (IRequest)request;
        requestInterface.CacheStorage.Should().BeSameAs(cacheStorage);
        requestInterface.HttpResponseMessageDeserializer.Should().BeSameAs(httpResponseMessageDeserializer);
        requestInterface.HttpClient.Should().BeSameAs(httpClient);
        requestInterface.QueryParameters.Should().NotBeNull().And.BeEmpty();
        requestInterface.Headers.Should().NotBeNull().And.BeEmpty();
        requestInterface.ThrowOnError.Should().BeTrue();
        requestInterface.HttpCompletionOption.Should().Be(HttpCompletionOption.ResponseHeadersRead);
        requestInterface.Timeout.Should().BeNull();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithHeaders_WhenHeadersIsNull_ShouldThrowArgumentNullException(Request request)
    {
        // Arrange
        IDictionary<string, string> headers = null!;

        // Act 
        Action action = () => request.WithHeaders(headers);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithHeaders_WhenCalled_ShouldSetHeaders(Request request)
    {
        // Arrange
        var headers = new Dictionary<string, string>
        {
            { "key1", "value1" },
            { "key2", "value2" }
        };

        // Act 
        var requestReturned = request.WithHeaders(headers);

        // Assert 
        var requestInterface = (IRequest)request;
        requestInterface.Headers.Should().BeEquivalentTo(headers);
        requestReturned.Should().BeSameAs(request);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithHeader_WhenKeyIsNull_ShouldThrowArgumentNullException(Request request)
    {
        // Arrange
        const string key = null;
        const string value = "value";

        // Act 
        Action action = () => request.WithHeader(key, value);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithHeader_WhenKeyIsEmpty_ShouldThrowArgumentException(Request request)
    {
        // Arrange
        const string key = "";
        const string value = "value";

        // Act 
        Action action = () => request.WithHeader(key, value);

        // Assert 
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithHeader_WhenCalled_ShouldSetHeader(Request request)
    {
        // Arrange
        const string key = "key";
        const string value = "value";

        // Act 
        var requestReturned = request.WithHeader(key, value);

        // Assert 
        requestReturned.Should().BeSameAs(request);
        var requestInterface = (IRequest)request;
        requestInterface.Headers.Should().ContainKey(key).And.ContainValue(value);
    }

    [Test]
    [AutoNSubstituteData]
    public void Accept_WhenCalled_ShouldSetAcceptHeader(Request request)
    {
        // Arrange
        var mediaTypeName = MediaTypeNames.Application.Json;

        // Act 
        var requestReturned = request.Accept(mediaTypeName);

        // Assert 
        requestReturned.Should().BeSameAs(request);
        var requestInterface = (IRequest)request;
        requestInterface.Headers.Should().Contain(header
            => header.Key == HeaderNames.Accept && header.Value == mediaTypeName);
    }

    [Test]
    [AutoNSubstituteData]
    public void AcceptJson_WhenCalled_ShouldSetAcceptJsonHeader(Request request)
    {
        // Act 
        var requestReturned = request.AcceptJson();

        // Assert 
        requestReturned.Should().BeSameAs(request);
        var requestInterface = (IRequest)request;
        requestInterface.Headers.Should().Contain(header
            => header.Key == HeaderNames.Accept && header.Value == MediaTypeNames.Application.Json);
    }

    [Test]
    [AutoNSubstituteData]
    public void AcceptXml_WhenCalled_ShouldSetAcceptXmlHeader(Request request)
    {
        // Act 
        var requestReturned = request.AcceptXml();

        // Assert 
        requestReturned.Should().BeSameAs(request);
        var requestInterface = (IRequest)request;
        requestInterface.Headers.Should().Contain(header
            => header.Key == HeaderNames.Accept && header.Value == MediaTypeNames.Application.Xml);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithAuthorization_WhenCalled_ShouldSetAuthorizationHeader(Request request)
    {
        // Arrange
        var authorization = "authorization";

        // Act 
        var requestReturned = request.WithAuthorization(authorization);

        // Assert 
        requestReturned.Should().BeSameAs(request);
        var requestInterface = (IRequest)request;
        requestInterface.Headers.Should().Contain(header
            => header.Key == HeaderNames.Authorization && header.Value == authorization);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithBearerToken_WhenCalled_ShouldSetBearerTokenHeader(Request request)
    {
        // Arrange
        var accessToken = "accessToken";

        // Act 
        var requestReturned = request.WithBearerToken(accessToken);

        // Assert 
        requestReturned.Should().BeSameAs(request);
        var requestInterface = (IRequest)request;
        requestInterface.Headers.Should().Contain(header
             => header.Key == HeaderNames.Authorization && header.Value == $"Bearer {accessToken}");
    }

    [Test]
    [AutoNSubstituteData]
    public void WithQuery_WhenDictionaryIsNull_ShouldThrowArgumentNullException(Request request)
    {
        // Arrange
        IDictionary<string, object> parameters = null!;

        // Act 
        Action action = () => request.WithQuery(parameters);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithQuery_WhenDictionaryIsNotNull_ShouldSetQueryStringFromDictionary(Request request)
    {
        // Arrange
        IDictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "key1", "value1" },
            { "key2", "value2" }
        };

        // Act 
        var requestReturned = request.WithQuery(parameters);

        // Assert 
        requestReturned.Should().BeSameAs(request);
        var requestInterface = (IRequest)request;
        requestInterface.QueryParameters.Should().BeEquivalentTo(parameters);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithQuery_WhenGenericClassIsNull_ShouldThrowArgumentNullException(Request request)
    {
        // Arrange
        object parameters = null!;

        // Act 
        Action action = () => request.WithQuery(parameters);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithQuery_WhenGenericClassIsDictionary_ShouldSetQueryStringFromDictionary(Request request)
    {
        // Arrange
        var payload = new Dictionary<string, object>
        {
            { "key1", "value1" },
            { "key2", "value2" }
        };

        // Act 
        var requestReturned = request.WithQuery(payload);

        // Assert 
        requestReturned.Should().BeSameAs(request);
        var requestInterface = (IRequest)request;
        requestInterface.QueryParameters.Should().BeEquivalentTo(payload);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithQuery_WhenGenericClassIsNotDictionary_ShouldSetQueryStringFromObjectProperties(Request request)
    {
        // Arrange
        var payload = new
        {
            Key1 = "Value1",
            Key2 = "Value2"
        };
        var expectedQueryParameters = new Dictionary<string, object>
        {
            { "Key1", "Value1" },
            { "Key2", "Value2" }
        };

        // Act 
        var requestReturned = request.WithQuery(payload);

        // Assert 
        requestReturned.Should().BeSameAs(request);
        var requestInterface = (IRequest)request;
        requestInterface.QueryParameters.Should().BeEquivalentTo(expectedQueryParameters);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithQuery_WhenValueIsNull_ShouldSetQueryStringWithNullValue(Request request)
    {
        // Arrange
        const string key = "key";
        const string value = null;
        var expectedQueryParameters = new Dictionary<string, object>
        {
            { key, value }
        };

        // Act 
        var requestReturned = request.WithQuery(key, value);

        // Assert 
        requestReturned.Should().BeSameAs(request);
        var requestInterface = (IRequest)request;
        requestInterface.QueryParameters.Should().BeEquivalentTo(expectedQueryParameters);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithQuery_WhenValueIsNotNull_ShouldSetQueryString(Request request)
    {
        // Arrange
        const string key = "key";
        const string value = "value";
        var expectedQueryParameters = new Dictionary<string, object>
        {
            { key, value }
        };

        // Act 
        var requestReturned = request.WithQuery(key, value);

        // Assert 
        requestReturned.Should().BeSameAs(request);
        var requestInterface = (IRequest)request;
        requestInterface.QueryParameters.Should().BeEquivalentTo(expectedQueryParameters);
    }

    [Test]
    [AutoNSubstituteData]
    public void IgnoreError_WhenCalled_ShouldSetThrowOnErrorToFalse(Request request)
    {
        // Act 
        var requestReturned = request.IgnoreError();

        // Assert 
        requestReturned.Should().BeSameAs(request);
        var requestInterface = (IRequest)request;
        requestInterface.ThrowOnError.Should().BeFalse();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithCompletionOption_WhenCalled_ShouldSetCompletionOption(Request request)
    {
        // Arrange
        var completionOption = HttpCompletionOption.ResponseContentRead;

        // Act 
        var requestReturned = request.WithCompletionOption(completionOption);

        // Assert 
        requestReturned.Should().BeSameAs(request);
        var requestInterface = (IRequest)request;
        requestInterface.HttpCompletionOption.Should().Be(completionOption);
    }

    [TestCase(-1)]
    [TestCase(0)]
    public void WithTimeout_WhenTimeoutIsZeroOrLess_ShouldThrowArgumentOutOfRangeException(int timeoutSeconds)
    {
        // Arrange 
        using var httpClient = new HttpClient();
        var cacheStorage = Substitute.For<ICacheStorage>();
        var httpResponseMessageDeserializer = Substitute.For<IHttpResponseMessageDeserializer>();
        var request = new Request(httpClient, cacheStorage, httpResponseMessageDeserializer);
        var timeout = TimeSpan.FromSeconds(timeoutSeconds);

        // Act 
        Action action = () => _ = request.WithTimeout(timeout);

        // Assert 
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithTimeout_WhenTimeoutIsInfinite_ShouldThrowArgumentOutOfRangeException(Request request)
    {
        // Arrange
        var timeout = Timeout.InfiniteTimeSpan;

        // Act 
        Action action = () => _ = request.WithTimeout(timeout);

        // Assert 
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithTimeout_WhenCalled_ShouldSetTimeout(Request request)
    {
        // Arrange
        var timeout = TimeSpan.FromSeconds(10);

        // Act 
        var requestReturned = request.WithTimeout(timeout);

        // Assert 
        requestReturned.Should().BeSameAs(request);
        var requestInterface = (IRequest)request;
        requestInterface.Timeout.Should().Be(timeout);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithEndpoint_WhenIntKeyIsProvided_ShouldSetEndpoint(Request request)
    {
        // Arrange
        const string resourceName = "students";
        const int key = 1;
        var expectedResourceName = $"{resourceName}/{key}/";

        // Act 
        var endpoint = request.WithEndpoint(resourceName, key);

        // Assert 
        endpoint.Should().NotBeNull();
        endpoint.Should().BeOfType<Endpoint>();
        endpoint.Endpoint.Should().Be(expectedResourceName);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithEndpoint_WhenLongKeyisprovided_ShouldSetEndpoint(Request request)
    {
        // Arrange
        const string resourceName = "students";
        const long key = 1;
        var expectedResourceName = $"{resourceName}/{key}/";

        // Act 
        var endpoint = request.WithEndpoint(resourceName, key);

        // Assert 
        endpoint.Should().NotBeNull();
        endpoint.Should().BeOfType<Endpoint>();
        endpoint.Endpoint.Should().Be(expectedResourceName);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithEndpoint_WhenGuidKeyIsProvided_ShouldSetEndpoint(Request request)
    {
        // Arrange
        const string resourceName = "students";
        var key = Guid.NewGuid();
        var expectedResourceName = $"{resourceName}/{key}/";

        // Act 
        var endpoint = request.WithEndpoint(resourceName, key);

        // Assert 
        endpoint.Should().NotBeNull();
        endpoint.Should().BeOfType<Endpoint>();
        endpoint.Endpoint.Should().Be(expectedResourceName);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithEndpoint_WhenStringKeyIsProvided_ShouldSetEndpoint(Request request)
    {
        // Arrange
        const string resourceName = "students";
        const string key = "1";
        var expectedResourceName = $"{resourceName}/{key}/";

        // Act 
        var endpoint = request.WithEndpoint(resourceName, key);

        // Assert 
        endpoint.Should().NotBeNull();
        endpoint.Should().BeOfType<Endpoint>();
        endpoint.Endpoint.Should().Be(expectedResourceName);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithEndpoint_WhenSegmentsIsNull_ShouldThrowArgumentNullException(Request request)
    {
        // Arrange
        IEnumerable<string> segments = null;

        // Act 
        Action action = () => request.WithEndpoint(segments);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithEndpoint_WhenSegmentsIsNotNull_ShouldSetEndpoint(Request request)
    {
        // Arrange
        var segments = new[] { "/students/", "/1//" };
        var expectedResourceName = "students/1/";

        // Act 
        var endpoint = request.WithEndpoint(segments);

        // Assert 
        endpoint.Should().NotBeNull();
        endpoint.Should().BeOfType<Endpoint>();
        endpoint.Endpoint.Should().Be(expectedResourceName);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithEndpoint_WhenResourceNameIsNull_ShouldThrowArgumentNullException(Request request)
    {
        // Arrange
        string resourceName = null;

        // Act 
        Action action = () => request.WithEndpoint(resourceName);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithEndpoint_WhenResourceNameIsEmpty_ShouldThrowArgumentException(Request request)
    {
        // Arrange
        const string resourceName = "";

        // Act 
        Action action = () => request.WithEndpoint(resourceName);

        // Assert 
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithEndpoint_WhenResourceNameIsNotNullOrEmpty_ShouldSetEndpoint(Request request)
    {
        // Arrange
        const string resourceName = "students";
        const string expectedResourceName = "students/";

        // Act 
        var endpoint = request.WithEndpoint(resourceName);

        // Assert 
        endpoint.Should().NotBeNull();
        endpoint.Should().BeOfType<Endpoint>();
        endpoint.Endpoint.Should().Be(expectedResourceName);
    }
}
