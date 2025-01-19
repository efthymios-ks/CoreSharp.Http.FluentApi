using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace CoreSharp.Http.FluentApi.Tests.Steps.UnsafeMethods;

public sealed class UnsafeMethodTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenMethodIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IMethod method = null!;

        // Act
        void Action()
            => _ = new UnsafeMethod(method);

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
        var unsafeMethod = new UnsafeMethod(method);

        // Assert
        var unsameMethodInterface = (IUnsafeMethod)unsafeMethod;
        Assert.Same(method.Endpoint, unsameMethodInterface.Endpoint);
        Assert.Equal(method.HttpMethod, unsameMethodInterface.HttpMethod);
    }

    [Fact]
    public void Constructor_WhenEndpointIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEndpoint endpoint = null!;
        var httpMethod = HttpMethod.Get;

        // Act
        void Action()
            => _ = new UnsafeMethod(endpoint, httpMethod);

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
            => _ = new UnsafeMethod(endpoint, httpMethod);

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
        var unsafeMethod = new UnsafeMethod(endpoint, httpMethod);

        // Assert
        var safeMethodInterface = (IUnsafeMethod)unsafeMethod;
        Assert.Same(endpoint, safeMethodInterface.Endpoint);
        Assert.Equal(httpMethod, safeMethodInterface.HttpMethod);
    }

    [Fact]
    public async Task WithJsonBody_WhenCalled_ShouldStringHttpContent()
    {
        // Arrange
        var method = MockCreate<IMethod>();
        var unsafeMethod = new UnsafeMethod(method);
        var expectedContentAsJson = JsonSerializer.Serialize(new
        {
            Message = "Data"
        });

        // Act
        var unsafeMethodReturned = unsafeMethod.WithJsonBody(expectedContentAsJson);

        // Assert
        Assert.Same(unsafeMethod, unsafeMethodReturned);
        var content = Assert.IsType<StringContent>(unsafeMethodReturned.HttpContent);
        var contentAsJson = await content.ReadAsStringAsync();
        Assert.Equal(expectedContentAsJson, contentAsJson);
        Assert.Contains(Encoding.UTF8.WebName, content.Headers.ContentType!.CharSet);
        Assert.Equal(MediaTypeNames.Application.Json, content.Headers.ContentType.MediaType);
        Assert.Equal(expectedContentAsJson.Length, content.Headers.ContentLength);
    }

    [Fact]
    public void WithJsonBody_WhenObjectToSerializeIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        object content = null!;
        var method = MockCreate<IMethod>();
        var unsafeMethod = new UnsafeMethod(method);

        // Act
        void Action()
            => unsafeMethod.WithJsonBody(content);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task WithJsonBody_WhenObjectToSerializeIsNotNull_ShouldSetObjectContent()
    {
        // Arrange
        var method = MockCreate<IMethod>();
        var unsafeMethod = new UnsafeMethod(method);
        var expectedPayload = new PublicDummyClass
        {
            Message = "Data"
        };

        // Act
        var unsafeMethodReturned = unsafeMethod.WithJsonBody(expectedPayload);

        // Assert
        Assert.Same(unsafeMethod, unsafeMethodReturned);
        var content = Assert.IsType<StreamContent>(unsafeMethodReturned.HttpContent);
        var contentAsJson = await content.ReadAsStringAsync();
        var contentPayload = JsonSerializer.Deserialize<PublicDummyClass>(contentAsJson);
        Assert.Equivalent(expectedPayload, contentPayload);
        Assert.Contains(Encoding.UTF8.WebName, content.Headers.ContentType!.CharSet);
        Assert.Equal(MediaTypeNames.Application.Json, content.Headers.ContentType.MediaType);
    }

    [Fact]
    public void WithJsonBody_WhenStreamIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var method = MockCreate<IMethod>();
        var unsafeMethod = new UnsafeMethod(method);
        Stream jsonStream = null!;

        // Act
        void Action()
            => unsafeMethod.WithJsonBody(jsonStream);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task WithJsonBody_WhenStreamIsNotNull_ShouldSetStreamContent()
    {
        // Arrange
        var method = MockCreate<IMethod>();
        var unsafeMethod = new UnsafeMethod(method);
        var expectedContentAsJson = JsonSerializer.Serialize(new
        {
            Message = "Data"
        });
        using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(expectedContentAsJson));

        // Act
        var unsafeMethodReturned = unsafeMethod.WithJsonBody(jsonStream);

        // Assert
        Assert.Same(unsafeMethod, unsafeMethodReturned);
        var content = Assert.IsType<StreamContent>(unsafeMethodReturned.HttpContent);
        var contentAsJson = await content.ReadAsStringAsync();
        Assert.Equal(expectedContentAsJson, contentAsJson);
        Assert.Contains(Encoding.UTF8.WebName, content.Headers.ContentType!.CharSet);
        Assert.Equal(MediaTypeNames.Application.Json, content.Headers.ContentType.MediaType);
        Assert.Equal(expectedContentAsJson.Length, content.Headers.ContentLength);
    }

    [Fact]
    public void WithXmlBody_WhenCalled_ShouldStringHttpContent()
    {
        // Arrange
        var method = MockCreate<IMethod>();
        var unsafeMethod = new UnsafeMethod(method);
        const string expectedContentAsXml = "<Message>Data</Message>";

        // Act
        var unsafeMethodReturned = unsafeMethod.WithXmlBody(expectedContentAsXml);

        // Assert
        Assert.Same(unsafeMethod, unsafeMethodReturned);
        var content = Assert.IsType<StringContent>(unsafeMethodReturned.HttpContent);
        var contentAsXml = content.ReadAsStringAsync().Result;
        Assert.Equal(expectedContentAsXml, contentAsXml);
        Assert.Contains(Encoding.UTF8.WebName, content.Headers.ContentType!.CharSet);
        Assert.Equal(MediaTypeNames.Application.Xml, content.Headers.ContentType.MediaType);
        Assert.Equal(expectedContentAsXml.Length, content.Headers.ContentLength);
    }

    [Fact]
    public void WithXmlBody_WhenObjectToSerializeIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        object content = null!;
        var method = MockCreate<IMethod>();
        var unsafeMethod = new UnsafeMethod(method);

        // Act
        void Action()
            => unsafeMethod.WithXmlBody(content);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void WithXmlBody_WhenObjectToSerializeIsNotNull_ShouldSetObjectContent()
    {
        // Arrange
        var method = MockCreate<IMethod>();
        var unsafeMethod = new UnsafeMethod(method);
        var expectedPayload = new PublicDummyClass
        {
            Message = "Data"
        };

        // Act
        var unsafeMethodReturned = unsafeMethod.WithXmlBody(expectedPayload);

        // Assert
        Assert.Same(unsafeMethod, unsafeMethodReturned);
        var content = Assert.IsType<StringContent>(unsafeMethodReturned.HttpContent);
        var contentAsXml = content.ReadAsStringAsync().Result;
        var contentPayload = DeserializeXml<PublicDummyClass>(contentAsXml);
        Assert.Equivalent(expectedPayload, contentPayload);
        Assert.Contains(Encoding.UTF8.WebName, content.Headers.ContentType!.CharSet);
        Assert.Equal(MediaTypeNames.Application.Xml, content.Headers.ContentType.MediaType);
    }

    [Fact]
    public void WithXmlBody_WhenStreamIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var method = MockCreate<IMethod>();
        var unsafeMethod = new UnsafeMethod(method);
        Stream xmlStream = null!;

        // Act
        void Action()
            => unsafeMethod.WithXmlBody(xmlStream);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void WithXmlBody_WhenStreamIsNotNull_ShouldSetStreamContent()
    {
        // Arrange
        var method = MockCreate<IMethod>();
        var unsafeMethod = new UnsafeMethod(method);
        const string expectedContentAsXml = "<Message>Data</Message>";
        using var xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(expectedContentAsXml));

        // Act
        var unsafeMethodReturned = unsafeMethod.WithXmlBody(xmlStream);

        // Assert
        Assert.Same(unsafeMethod, unsafeMethodReturned);
        var content = Assert.IsType<StreamContent>(unsafeMethodReturned.HttpContent);
        var contentAsXml = content.ReadAsStringAsync().Result;
        Assert.Equal(expectedContentAsXml, contentAsXml);
        Assert.Contains(Encoding.UTF8.WebName, content.Headers.ContentType!.CharSet);
        Assert.Equal(MediaTypeNames.Application.Xml, content.Headers.ContentType.MediaType);
        Assert.Equal(expectedContentAsXml.Length, content.Headers.ContentLength);
    }

    [Fact]
    public void WithBody_WhenContentAndMediaTypeAreProvided_ShouldSetStringContent()
    {
        // Arrange
        var method = MockCreate<IMethod>();
        var unsafeMethod = new UnsafeMethod(method);
        const string expectedContent = "Data";
        const string expectedMediaType = MediaTypeNames.Text.Plain;

        // Act
        var unsafeMethodReturned = unsafeMethod.WithBody(expectedContent, expectedMediaType);

        // Assert
        Assert.Same(unsafeMethod, unsafeMethodReturned);
        var content = Assert.IsType<StringContent>(unsafeMethodReturned.HttpContent);
        var contentAsString = content.ReadAsStringAsync().Result;
        Assert.Equal(expectedContent, contentAsString);
        Assert.Contains(Encoding.UTF8.WebName, content.Headers.ContentType!.CharSet);
        Assert.Equal(expectedMediaType, content.Headers.ContentType.MediaType);
        Assert.Equal(expectedContent.Length, content.Headers.ContentLength);
    }

    [Fact]
    public void WithBody_WhenContentAndEncodingAndMediaTypeAreProvided_ShouldSetStringContent()
    {
        // Arrange
        var method = MockCreate<IMethod>();
        var unsafeMethod = new UnsafeMethod(method);
        const string expectedContent = "Data";
        var expectedEncoding = Encoding.Unicode;
        const string expectedMediaType = MediaTypeNames.Text.Plain;

        // Act
        var unsafeMethodReturned = unsafeMethod.WithBody(expectedContent, expectedEncoding, expectedMediaType);

        // Assert
        Assert.Same(unsafeMethod, unsafeMethodReturned);
        var content = Assert.IsType<StringContent>(unsafeMethodReturned.HttpContent);
        var contentAsString = content.ReadAsStringAsync().Result;
        Assert.Equal(expectedContent, contentAsString);
        Assert.Contains(expectedEncoding.WebName, content.Headers.ContentType!.CharSet);
        Assert.Equal(expectedMediaType, content.Headers.ContentType.MediaType);
    }

    [Fact]
    public async Task SendAsync_WhenCalled_ShouldIncludeHttpContentI()
    {
        // Arrange 
        var unsafeMethod = MockCreate<UnsafeMethod>();
        using var httpContent = new StringContent("Data", new MediaTypeHeaderValue(MediaTypeNames.Text.Plain));

        // Act
        var response = await unsafeMethod
            .WithBody(httpContent)
            .SendAsync();

        // Assert
        Assert.NotNull(response);
        Assert.Same(httpContent, response!.RequestMessage!.Content);
    }

    private static TResult? DeserializeXml<TResult>(string xml)
    {
        using var stringReader = new StringReader(xml);
        using var reader = XmlReader.Create(stringReader);
        var serializer = new XmlSerializer(typeof(TResult));
        return (TResult?)serializer.Deserialize(reader);
    }

    public sealed class PublicDummyClass
    {
        public required string Message { get; set; }
    }
}
