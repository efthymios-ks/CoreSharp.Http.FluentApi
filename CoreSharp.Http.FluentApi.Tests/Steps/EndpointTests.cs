using CoreSharp.Http.FluentApi.Steps;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;

namespace CoreSharp.Http.FluentApi.Tests.Steps;

public sealed class EndpointTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenRequestIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IRequest request = null!;
        const string resourceName = "resourceName";

        // Act
        void Action()
            => _ = new Endpoint(request, resourceName);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenResourceNameIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var request = MockCreate<IRequest>();
        const string resourceName = null!;

        // Act
        void Action()
            => _ = new Endpoint(request, resourceName);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenResourceNameIsEmpty_ShouldThrowArgumentNullException()
    {
        // Arrange
        var request = MockCreate<IRequest>();
        const string resourceName = "";

        // Act
        void Action()
            => _ = new Endpoint(request, resourceName);

        // Assert
        Assert.Throws<ArgumentException>(Action);
    }

    [Fact]
    public void Constructor_WhenCalled_ShouldSetProperties()
    {
        // Arrange
        var request = MockCreate<IRequest>();
        const string resourceName = "students";

        // Act
        var endpoint = new Endpoint(request, resourceName);

        // Assert
        var endpointInterface = (IEndpoint)endpoint;
        Assert.Same(request, endpointInterface.Request);
        Assert.Equal(resourceName, endpointInterface.Endpoint);
        Assert.NotNull(endpointInterface.QueryParameters);
        Assert.Empty(endpointInterface.QueryParameters);
    }

    [Fact]
    public void WithQuery_WhenDictionaryIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var endpoint = MockCreate<Endpoint>();
        IDictionary<string, object> parameters = null!;

        // Act 
        void Action()
            => endpoint.WithQuery(parameters);

        // Assert 
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void WithQuery_WhenDictionaryIsNotNull_ShouldSetQueryStringFromDictionary()
    {
        // Arrange
        var endpoint = MockCreate<Endpoint>();
        var parameters = new Dictionary<string, object>
                {
                    { "key1", "value1" },
                    { "key2", "value2" }
                };

        // Act 
        var endpointReturned = endpoint.WithQuery(parameters);

        // Assert 
        Assert.Same(endpoint, endpointReturned);
        var endpointInterface = (IEndpoint)endpoint;
        Assert.Equivalent(parameters, endpointInterface.QueryParameters);
    }

    [Fact]
    public void WithQuery_WhenGenericClassIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var endpoint = MockCreate<Endpoint>();
        object parameters = null!;

        // Act 
        void Action()
            => endpoint.WithQuery(parameters);

        // Assert 
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void WithQuery_WhenGenericClassIsDictionary_ShouldSetQueryStringFromDictionary()
    {
        // Arrange
        var endpoint = MockCreate<Endpoint>();
        var payload = new Dictionary<string, object>
                {
                    { "key1", "value1" },
                    { "key2", "value2" }
                };

        // Act 
        var endpointReturned = endpoint.WithQuery(payload);

        // Assert 
        Assert.Same(endpoint, endpointReturned);
        var endpointInterface = (IEndpoint)endpoint;
        Assert.Equivalent(payload, endpointInterface.QueryParameters);
    }

    [Fact]
    public void WithQuery_WhenGenericClassIsNotDictionary_ShouldSetQueryStringFromObjectProperties()
    {
        // Arrange
        var endpoint = MockCreate<Endpoint>();
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
        Assert.Same(endpoint, endpointReturned);
        var endpointInterface = (IEndpoint)endpoint;
        Assert.Equivalent(expectedQueryParameters, endpointInterface.QueryParameters);
    }

    [Fact]
    public void WithQuery_WhenValueIsNull_ShouldSetQueryStringWithEmptyString()
    {
        // Arrange
        var endpoint = MockCreate<Endpoint>();
        const string key = "key";
        const string value = null!;
        var expectedQueryParameters = new Dictionary<string, object>
                {
                    { key, string.Empty }
                };

        // Act 
        var endpointReturned = endpoint.WithQuery(key, value);

        // Assert 
        Assert.Same(endpoint, endpointReturned);
        var endpointInterface = (IEndpoint)endpoint;
        Assert.Equivalent(expectedQueryParameters, endpointInterface.QueryParameters);
    }

    [Fact]
    public void WithQuery_WhenValueIsNotNull_ShouldSetQueryString()
    {
        // Arrange
        var endpoint = MockCreate<Endpoint>();
        const string key = "key";
        const string value = "value";
        var expectedQueryParameters = new Dictionary<string, object>
                {
                    { key, value }
                };

        // Act 
        var endpointReturned = endpoint.WithQuery(key, value);

        // Assert 
        Assert.Same(endpoint, endpointReturned);
        var endpointInterface = (IEndpoint)endpoint;
        Assert.Equivalent(expectedQueryParameters, endpointInterface.QueryParameters);
    }

    [Fact]
    public void Get_WhenCalled_ShouldReturnSafeMethodWithResult()
    {
        // Arrange
        var request = MockCreate<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Get();

        // Assert
        Assert.IsType<SafeMethodWithResult>(result);
        Assert.Equal(HttpMethod.Get, result.HttpMethod);
    }

    [Fact]
    public void Post_WhenCalled_ShouldReturnUnsafeMethodWithResult()
    {
        // Arrange
        var request = MockCreate<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Post();

        // Assert
        Assert.IsType<UnsafeMethodWithResult>(result);
        Assert.Equal(HttpMethod.Post, result.HttpMethod);
    }

    [Fact]
    public void Put_WhenCalled_ShouldReturnUnsafeMethodWithResult()
    {
        // Arrange
        var request = MockCreate<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Put();

        // Assert
        Assert.IsType<UnsafeMethodWithResult>(result);
        Assert.Equal(HttpMethod.Put, result.HttpMethod);
    }

    [Fact]
    public void Patch_WhenCalled_ShouldReturnUnsafeMethodWithResult()
    {
        // Arrange
        var request = MockCreate<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Patch();

        // Assert
        Assert.IsType<UnsafeMethodWithResult>(result);
        Assert.Equal(HttpMethod.Patch, result.HttpMethod);
    }

    [Fact]
    public void Delete_WhenCalled_ShouldReturnUnsafeMethodWithResult()
    {
        // Arrange
        var request = MockCreate<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Delete();

        // Assert
        Assert.IsType<UnsafeMethodWithResult>(result);
        Assert.Equal(HttpMethod.Delete, result.HttpMethod);
    }

    [Fact]
    public void Head_WhenCalled_ShouldReturnSafeMethodWithResult()
    {
        // Arrange
        var request = MockCreate<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Head();

        // Assert
        Assert.IsType<SafeMethodWithResult>(result);
        Assert.Equal(HttpMethod.Head, result.HttpMethod);
    }

    [Fact]
    public void Options_WhenCalled_ShouldReturnSafeMethodWithResult()
    {
        // Arrange
        var request = MockCreate<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Options();

        // Assert
        Assert.IsType<SafeMethodWithResult>(result);
        Assert.Equal(HttpMethod.Options, result.HttpMethod);
    }

    [Fact]
    public void Trace_WhenCalled_ShouldReturnSafeMethodWithResult()
    {
        // Arrange
        var request = MockCreate<IRequest>();
        const string resourceName = "students";
        var endpoint = new Endpoint(request, resourceName);

        // Act
        var result = endpoint.Trace();

        // Assert
        Assert.IsType<SafeMethodWithResult>(result);
        Assert.Equal(HttpMethod.Trace, result.HttpMethod);
    }
}
