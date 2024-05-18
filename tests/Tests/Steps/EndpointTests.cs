using CoreSharp.Http.FluentApi.Steps;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Tests.Internal.Attributes;

namespace Tests.Steps;

[TestFixture]
public sealed class EndpointTests
{
    [Test]
    public void Constructor_WhenRequestIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IRequest request = null;
        const string resourceName = "resourceName";

        // Act
        Action action = () => _ = new Endpoint(request, resourceName);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenResourceNameIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var request = Substitute.For<IRequest>();
        const string resourceName = null;

        // Act
        Action action = () => _ = new Endpoint(request, resourceName);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenResourceNameIsEmpty_ShouldThrowArgumentNullException()
    {
        // Arrange
        var request = Substitute.For<IRequest>();
        const string resourceName = "";

        // Act
        Action action = () => _ = new Endpoint(request, resourceName);

        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void Constructor_WhenCalled_ShouldSetProperties()
    {
        // Arrange
        var request = Substitute.For<IRequest>();
        const string resourceName = "students";

        // Act
        var endpoint = new Endpoint(request, resourceName);

        // Assert
        var endpointInterface = (IEndpoint)endpoint;
        endpointInterface.Request.Should().BeSameAs(request);
        endpointInterface.Endpoint.Should().Be(resourceName);
        endpointInterface.QueryParameters.Should().NotBeNull().And.BeEmpty();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithQuery_WhenDictionaryIsNull_ShouldThrowArgumentNullException(Endpoint endpoint)
    {
        // Arrange
        IDictionary<string, object> parameters = null!;

        // Act 
        Action action = () => endpoint.WithQuery(parameters);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithQuery_WhenDictionaryIsNotNull_ShouldSetQueryStringFromDictionary(Endpoint endpoint)
    {
        // Arrange
        IDictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "key1", "value1" },
            { "key2", "value2" }
        };

        // Act 
        var endpointReturned = endpoint.WithQuery(parameters);

        // Assert 
        endpointReturned.Should().BeSameAs(endpoint);
        var endpointInterface = (IEndpoint)endpoint;
        endpointInterface.QueryParameters.Should().BeEquivalentTo(parameters);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithQuery_WhenGenericClassIsNull_ShouldThrowArgumentNullException(Endpoint endpoint)
    {
        // Arrange
        object parameters = null!;

        // Act 
        Action action = () => endpoint.WithQuery(parameters);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithQuery_WhenGenericClassIsDictionary_ShouldSetQueryStringFromDictionary(Endpoint endpoint)
    {
        // Arrange
        var payload = new Dictionary<string, object>
        {
            { "key1", "value1" },
            { "key2", "value2" }
        };

        // Act 
        var endpointReturned = endpoint.WithQuery(payload);

        // Assert 
        endpointReturned.Should().BeSameAs(endpoint);
        var endpointInterface = (IEndpoint)endpoint;
        endpointInterface.QueryParameters.Should().BeEquivalentTo(payload);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithQuery_WhenGenericClassIsNotDictionary_ShouldSetQueryStringFromObjectProperties(Endpoint endpoint)
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
        var endpointReturned = endpoint.WithQuery(payload);

        // Assert 
        endpointReturned.Should().BeSameAs(endpoint);
        var endpointInterface = (IEndpoint)endpoint;
        endpointInterface.QueryParameters.Should().BeEquivalentTo(expectedQueryParameters);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithQuery_WhenValueIsNull_ShouldSetQueryStringWithNullValue(Endpoint endpoint)
    {
        // Arrange
        const string key = "key";
        const string value = null;
        var expectedQueryParameters = new Dictionary<string, object>
        {
            { key, value }
        };

        // Act 
        var endpointReturned = endpoint.WithQuery(key, value);

        // Assert 
        endpointReturned.Should().BeSameAs(endpoint);
        var endpointInterface = (IEndpoint)endpoint;
        endpointInterface.QueryParameters.Should().BeEquivalentTo(expectedQueryParameters);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithQuery_WhenValueIsNotNull_ShouldSetQueryString(Endpoint endpoint)
    {
        // Arrange
        const string key = "key";
        const string value = "value";
        var expectedQueryParameters = new Dictionary<string, object>
        {
            { key, value }
        };

        // Act 
        var endpointReturned = endpoint.WithQuery(key, value);

        // Assert 
        endpointReturned.Should().BeSameAs(endpoint);
        var endpointInterface = (IEndpoint)endpoint;
        endpointInterface.QueryParameters.Should().BeEquivalentTo(expectedQueryParameters);
    }

    [Test]
    public void Get_WhenCalled_ShouldReturnSafeMethodWithResult()
    {
        // Arrange
        var request = Substitute.For<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Get();

        // Assert
        result.Should().BeOfType<SafeMethodWithResult>();
        result.HttpMethod.Should().Be(HttpMethod.Get);
    }

    [Test]
    public void Post_WhenCalled_ShouldReturnUnsafeMethodWithResult()
    {
        // Arrange
        var request = Substitute.For<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Post();

        // Assert
        result.Should().BeOfType<UnsafeMethodWithResult>();
        result.HttpMethod.Should().Be(HttpMethod.Post);
    }

    [Test]
    public void Put_WhenCalled_ShouldReturnUnsafeMethodWithResult()
    {
        // Arrange
        var request = Substitute.For<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Put();

        // Assert
        result.Should().BeOfType<UnsafeMethodWithResult>();
        result.HttpMethod.Should().Be(HttpMethod.Put);
    }

    [Test]
    public void Patch_WhenCalled_ShouldReturnUnsafeMethodWithResult()
    {
        // Arrange
        var request = Substitute.For<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Patch();

        // Assert
        result.Should().BeOfType<UnsafeMethodWithResult>();
        result.HttpMethod.Should().Be(HttpMethod.Patch);
    }

    [Test]
    public void Delete_WhenCalled_ShouldReturnUnsafeMethodWithResult()
    {
        // Arrange
        var request = Substitute.For<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Delete();

        // Assert
        result.Should().BeOfType<UnsafeMethodWithResult>();
        result.HttpMethod.Should().Be(HttpMethod.Delete);
    }

    [Test]
    public void Head_WhenCalled_ShouldReturnSafeMethodWithResult()
    {
        // Arrange
        var request = Substitute.For<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Head();

        // Assert
        result.Should().BeOfType<SafeMethodWithResult>();
        result.HttpMethod.Should().Be(HttpMethod.Head);
    }

    [Test]
    public void Options_WhenCalled_ShouldReturnSafeMethodWithResult()
    {
        // Arrange
        var request = Substitute.For<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Options();

        // Assert
        result.Should().BeOfType<SafeMethodWithResult>();
        result.HttpMethod.Should().Be(HttpMethod.Options);
    }

    [Test]
    public void Trace_WhenCalled_ShouldReturnSafeMethodWithResult()
    {
        // Arrange
        var request = Substitute.For<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Trace();

        // Assert
        result.Should().BeOfType<SafeMethodWithResult>();
        result.HttpMethod.Should().Be(HttpMethod.Trace);
    }
}
