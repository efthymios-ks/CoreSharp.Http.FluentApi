using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Tests.Internal.Attributes;

namespace Tests.Steps.UnsafeMethods;

[TestFixture]
public sealed class UnsafeMethodTests
{
    [Test]
    public void Constructor_WhenMethodIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IMethod method = null;

        // Act
        Action action = () => _ = new UnsafeMethod(method);

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
        var unsafeMethod = new UnsafeMethod(method);

        // Assert
        var unsameMethodInterface = (IUnsafeMethod)unsafeMethod;
        unsameMethodInterface.Endpoint.Should().BeSameAs(method.Endpoint);
        unsameMethodInterface.HttpMethod.Should().Be(method.HttpMethod);
    }

    [Test]
    public void Constructor_WhenEndpointIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEndpoint endpoint = null;
        var httpMethod = HttpMethod.Get;

        // Act
        Action action = () => _ = new UnsafeMethod(endpoint, httpMethod);

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
        Action action = () => _ = new UnsafeMethod(endpoint, httpMethod);

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
        var unsafeMethod = new UnsafeMethod(endpoint, httpMethod);

        // Assert
        var safeMethodInterface = (IUnsafeMethod)unsafeMethod;
        safeMethodInterface.Endpoint.Should().BeSameAs(endpoint);
        safeMethodInterface.HttpMethod.Should().Be(httpMethod);
    }

    [Test]
    [AutoNSubstituteData]
    public async Task WithJsonBody_WhenCalled_ShouldStringHttpContent(IMethod method)
    {
        // Arrange
        var unsafeMethod = new UnsafeMethod(method);
        var expectedContentAsJson = JsonSerializer.Serialize(new
        {
            Message = "Data"
        });

        // Act
        var unsafeMethodReturned = unsafeMethod.WithJsonBody(expectedContentAsJson);

        // Assert
        unsafeMethodReturned.Should().BeSameAs(unsafeMethod);
        var content = unsafeMethodReturned.HttpContent.Should().BeOfType<StringContent>().Subject;
        var contentAsJson = await content.ReadAsStringAsync();
        contentAsJson.Should().Be(expectedContentAsJson);
        content.Headers.ContentType.CharSet.Should().Contain(Encoding.UTF8.WebName);
        content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Json);
        content.Headers.ContentLength.Should().Be(expectedContentAsJson.Length);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithJsonBody_WhenObjectToSerializeIsNull_ShouldThrowArgumentNullException(IMethod method)
    {
        // Arrange
        object content = null;
        var unsafeMethod = new UnsafeMethod(method);

        // Act
        Action action = () => _ = unsafeMethod.WithJsonBody(content);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task WithJsonBody_WhenObjectToSerializeIsNotNull_ShouldSetObjectContent(IMethod method)
    {
        // Arrange
        var unsafeMethod = new UnsafeMethod(method);
        var expectedPayload = new PublicDummyClass
        {
            Message = "Data"
        };

        // Act
        var unsafeMethodReturned = unsafeMethod.WithJsonBody(expectedPayload);

        // Assert
        unsafeMethodReturned.Should().BeSameAs(unsafeMethod);
        var content = unsafeMethodReturned.HttpContent.Should().BeOfType<StreamContent>().Subject;
        var contentAsJson = await content.ReadAsStringAsync();
        var contentPayload = JsonSerializer.Deserialize<PublicDummyClass>(contentAsJson);
        contentPayload.Should().BeEquivalentTo(expectedPayload);
        content.Headers.ContentType.CharSet.Should().Contain(Encoding.UTF8.WebName);
        content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithJsonBody_WhenStreamIsNull_ShouldThrowArgumentNullException(IMethod method)
    {
        // Arrange
        var unsafeMethod = new UnsafeMethod(method);
        Stream jsonStream = null;

        // Act
        Action action = () => _ = unsafeMethod.WithJsonBody(jsonStream);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task WithJsonBody_WhenStreamIsNotNull_ShouldSetStreamContent(IMethod method)
    {
        // Arrange
        var unsafeMethod = new UnsafeMethod(method);
        var expectedContentAsJson = JsonSerializer.Serialize(new
        {
            Message = "Data"
        });
        using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(expectedContentAsJson));

        // Act
        var unsafeMethodReturned = unsafeMethod.WithJsonBody(jsonStream);

        // Assert
        unsafeMethodReturned.Should().BeSameAs(unsafeMethod);
        var content = unsafeMethodReturned.HttpContent.Should().BeOfType<StreamContent>().Subject;
        var contentAsJson = await content.ReadAsStringAsync();
        contentAsJson.Should().Be(expectedContentAsJson);
        content.Headers.ContentType.CharSet.Should().Contain(Encoding.UTF8.WebName);
        content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Json);
        content.Headers.ContentLength.Should().Be(expectedContentAsJson.Length);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithXmlBody_WhenCalled_ShouldStringHttpContent(IMethod method)
    {
        // Arrange
        var unsafeMethod = new UnsafeMethod(method);
        var expectedContentAsXml = "<Message>Data</Message>";

        // Act
        var unsafeMethodReturned = unsafeMethod.WithXmlBody(expectedContentAsXml);

        // Assert
        unsafeMethodReturned.Should().BeSameAs(unsafeMethod);
        var content = unsafeMethodReturned.HttpContent.Should().BeOfType<StringContent>().Subject;
        var contentAsXml = content.ReadAsStringAsync().Result;
        contentAsXml.Should().Be(expectedContentAsXml);
        content.Headers.ContentType.CharSet.Should().Contain(Encoding.UTF8.WebName);
        content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Xml);
        content.Headers.ContentLength.Should().Be(expectedContentAsXml.Length);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithXmlBody_WhenObjectToSerializeIsNull_ShouldThrowArgumentNullException(IMethod method)
    {
        // Arrange
        object content = null;
        var unsafeMethod = new UnsafeMethod(method);

        // Act
        Action action = () => _ = unsafeMethod.WithXmlBody(content);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithXmlBody_WhenObjectToSerializeIsNotNull_ShouldSetObjectContent(IMethod method)
    {
        // Arrange
        var unsafeMethod = new UnsafeMethod(method);
        var expectedPayload = new PublicDummyClass
        {
            Message = "Data"
        };

        // Act
        var unsafeMethodReturned = unsafeMethod.WithXmlBody(expectedPayload);

        // Assert
        unsafeMethodReturned.Should().BeSameAs(unsafeMethod);
        var content = unsafeMethodReturned.HttpContent.Should().BeOfType<StringContent>().Subject;
        var contentAsXml = content.ReadAsStringAsync().Result;
        var contentPayload = DeserializeXml<PublicDummyClass>(contentAsXml);
        contentPayload.Should().BeEquivalentTo(expectedPayload);
        content.Headers.ContentType.CharSet.Should().Contain(Encoding.UTF8.WebName);
        content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Xml);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithXmlBody_WhenStreamIsNull_ShouldThrowArgumentNullException(IMethod method)
    {
        // Arrange
        var unsafeMethod = new UnsafeMethod(method);
        Stream xmlStream = null;

        // Act
        Action action = () => _ = unsafeMethod.WithXmlBody(xmlStream);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithXmlBody_WhenStreamIsNotNull_ShouldSetStreamContent(IMethod method)
    {
        // Arrange
        var unsafeMethod = new UnsafeMethod(method);
        var expectedContentAsXml = "<Message>Data</Message>";
        using var xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(expectedContentAsXml));

        // Act
        var unsafeMethodReturned = unsafeMethod.WithXmlBody(xmlStream);

        // Assert
        unsafeMethodReturned.Should().BeSameAs(unsafeMethod);
        var content = unsafeMethodReturned.HttpContent.Should().BeOfType<StreamContent>().Subject;
        var contentAsXml = content.ReadAsStringAsync().Result;
        contentAsXml.Should().Be(expectedContentAsXml);
        content.Headers.ContentType.CharSet.Should().Contain(Encoding.UTF8.WebName);
        content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Xml);
        content.Headers.ContentLength.Should().Be(expectedContentAsXml.Length);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithBody_WhenContentAndMediaTypeAreProvided_ShouldSetStringContent(IMethod method)
    {
        // Arrange
        var unsafeMethod = new UnsafeMethod(method);
        var expectedContent = "Data";
        var expectedMediaType = MediaTypeNames.Text.Plain;

        // Act
        var unsafeMethodReturned = unsafeMethod.WithBody(expectedContent, expectedMediaType);

        // Assert
        unsafeMethodReturned.Should().BeSameAs(unsafeMethod);
        var content = unsafeMethodReturned.HttpContent.Should().BeOfType<StringContent>().Subject;
        var contentAsString = content.ReadAsStringAsync().Result;
        contentAsString.Should().Be(expectedContent);
        content.Headers.ContentType.CharSet.Should().Contain(Encoding.UTF8.WebName);
        content.Headers.ContentType.MediaType.Should().Be(expectedMediaType);
        content.Headers.ContentLength.Should().Be(expectedContent.Length);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithBody_WhenContentAndEncodingAndMediaTypeAreProvided_ShouldSetStringContent(IMethod method)
    {
        // Arrange
        var unsafeMethod = new UnsafeMethod(method);
        var expectedContent = "Data";
        var expectedEncoding = Encoding.Unicode;
        var expectedMediaType = MediaTypeNames.Text.Plain;

        // Act
        var unsafeMethodReturned = unsafeMethod.WithBody(expectedContent, expectedEncoding, expectedMediaType);

        // Assert
        unsafeMethodReturned.Should().BeSameAs(unsafeMethod);
        var content = unsafeMethodReturned.HttpContent.Should().BeOfType<StringContent>().Subject;
        var contentAsString = content.ReadAsStringAsync().Result;
        contentAsString.Should().Be(expectedContent);
        content.Headers.ContentType.CharSet.Should().Contain(expectedEncoding.WebName);
        content.Headers.ContentType.MediaType.Should().Be(expectedMediaType);
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenCalled_ShouldIncludeHttpContentI(UnsafeMethod unsafeMethod)
    {
        // Arrange 
        using var httpContent = new StringContent("Data", new MediaTypeHeaderValue(MediaTypeNames.Text.Plain));

        // Act
        var response = await unsafeMethod
            .WithBody(httpContent)
            .SendAsync();

        // Assert
        response.RequestMessage.Content.Should().BeSameAs(httpContent);
    }

    private static TResult DeserializeXml<TResult>(string xml)
    {
        using var stringReader = new StringReader(xml);
        using var reader = XmlReader.Create(stringReader);
        var serializer = new XmlSerializer(typeof(TResult));
        return (TResult)serializer.Deserialize(reader);
    }

    public sealed class PublicDummyClass
    {
        public string Message { get; set; }
    }
}
