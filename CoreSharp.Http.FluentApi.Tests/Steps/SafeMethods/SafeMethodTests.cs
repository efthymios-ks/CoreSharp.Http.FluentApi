using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Steps.SafeMethods;

[TestFixture]
public sealed class SafeMethodTests
{
    [Test]
    public void Constructor_WhenMethodIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IMethod method = null!;

        // Act
        Action action = () => _ = new SafeMethod(method);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenMethodIsNotNull_ShouldSetProperties()
    {
        // Arrange
        var method = Substitute.For<IMethod>();
        method.Endpoint = Substitute.For<IEndpoint>();
        method.HttpMethod = HttpMethod.Get;

        // Act
        var safeMethod = new SafeMethod(method);

        // Assert
        var sameMethodInterface = (ISafeMethod)safeMethod;
        sameMethodInterface.Endpoint.Should().BeSameAs(method.Endpoint);
        sameMethodInterface.HttpMethod.Should().Be(method.HttpMethod);
    }

    [Test]
    public void Constructor_WhenEndpointIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEndpoint? endpoint = null;
        var httpMethod = HttpMethod.Get;

        // Act
        Action action = () => _ = new SafeMethod(endpoint, httpMethod);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenHttpMethodIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var endpoint = Substitute.For<IEndpoint>();
        HttpMethod? httpMethod = null;

        // Act
        Action action = () => _ = new SafeMethod(endpoint, httpMethod);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenEndpointAndHttpMethodAreNotNull_ShouldSetProperties()
    {
        // Arrange
        var endpoint = Substitute.For<IEndpoint>();
        var httpMethod = HttpMethod.Get;

        // Act
        var safeMethod = new SafeMethod(endpoint, httpMethod);

        // Assert
        var safeMethodInterface = (ISafeMethod)safeMethod;
        safeMethodInterface.Endpoint.Should().BeSameAs(endpoint);
        safeMethodInterface.HttpMethod.Should().Be(httpMethod);
    }
}
