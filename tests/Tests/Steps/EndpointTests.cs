using CoreSharp.Http.FluentApi.Steps;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Net.Http;

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
