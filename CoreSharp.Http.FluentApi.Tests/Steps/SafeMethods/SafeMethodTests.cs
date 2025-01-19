using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

namespace CoreSharp.Http.FluentApi.Tests.Steps.SafeMethods;

public sealed class SafeMethodTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenMethodIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IMethod method = null!;

        // Act
        void Action()
            => _ = new SafeMethod(method);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenMethodIsNotNull_ShouldSetProperties()
    {
        // Arrange
        var method = MockCreate<IMethod>();
        method.Endpoint = MockCreate<IEndpoint>();
        method.HttpMethod = HttpMethod.Get;

        // Act
        var safeMethod = new SafeMethod(method);

        // Assert
        var sameMethodInterface = (ISafeMethod)safeMethod;
        Assert.Same(method.Endpoint, sameMethodInterface.Endpoint);
        Assert.Equal(method.HttpMethod, sameMethodInterface.HttpMethod);
    }

    [Fact]
    public void Constructor_WhenEndpointIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEndpoint? endpoint = null;
        var httpMethod = HttpMethod.Get;

        // Act
        void Action()
            => _ = new SafeMethod(endpoint, httpMethod);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenHttpMethodIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var endpoint = MockCreate<IEndpoint>();
        HttpMethod? httpMethod = null;

        // Act
        void Action()
            => _ = new SafeMethod(endpoint, httpMethod);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenEndpointAndHttpMethodAreNotNull_ShouldSetProperties()
    {
        // Arrange
        var endpoint = MockCreate<IEndpoint>();
        var httpMethod = HttpMethod.Get;

        // Act
        var safeMethod = new SafeMethod(endpoint, httpMethod);

        // Assert
        var safeMethodInterface = (ISafeMethod)safeMethod;
        Assert.Same(endpoint, safeMethodInterface.Endpoint);
        Assert.Equal(httpMethod, safeMethodInterface.HttpMethod);
    }
}
