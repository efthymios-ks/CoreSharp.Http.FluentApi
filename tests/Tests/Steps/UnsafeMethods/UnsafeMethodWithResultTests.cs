using AutoFixture.NUnit3;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using Tests.Internal.Attributes;
using Tests.Internal.HttpmessageHandlers;
using JsonNet = Newtonsoft.Json;
using TextJson = System.Text.Json;

namespace Tests.Steps.UnsafeMethods;

[TestFixture]
public sealed class UnsafeMethodWithResultTests
{
    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenCalled_ShouldNotThrw(IUnsafeMethod unsafeMethod)
    {
        // Act
        Action action = () => _ = new UnsafeMethodWithResult(unsafeMethod);

        // Assert 
        action.Should().NotThrow();
    }

    [Test]
    [AutoNSubstituteData]
    public void ToBytes_WhenCalled_ShouldReturnUnsafeMethodWithResultAsBytes(IUnsafeMethod unsafeMethod)
    {
        // Arrange 
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);

        // Act
        var result = unsafeMethodWithResult.ToBytes();

        // Assert
        result.Should().BeOfType<UnsafeMethodWithResultAsBytes>();
        result.Endpoint.Should().BeSameAs(unsafeMethod.Endpoint);
        result.HttpMethod.Should().Be(unsafeMethod.HttpMethod);
    }

    [Test]
    [AutoNSubstituteData]
    public void ToStream_WhenCalled_ShouldReturnUnsafeMethodWithResultAsStream(IUnsafeMethod unsafeMethod)
    {
        // Arrange 
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);

        // Act
        var result = unsafeMethodWithResult.ToStream();

        // Assert
        result.Should().BeOfType<UnsafeMethodWithResultAsStream>();
        result.Endpoint.Should().BeSameAs(unsafeMethod.Endpoint);
        result.HttpMethod.Should().Be(unsafeMethod.HttpMethod);
    }

    [Test]
    [AutoNSubstituteData]
    public void ToString_WhenCalled_ShouldReturnUnsafeMethodWithResultAsString(IUnsafeMethod unsafeMethod)
    {
        // Arrange 
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);

        // Act
        var result = unsafeMethodWithResult.ToString();

        // Assert
        result.Should().BeOfType<UnsafeMethodWithResultAsString>();
        result.Endpoint.Should().BeSameAs(unsafeMethod.Endpoint);
        result.HttpMethod.Should().Be(unsafeMethod.HttpMethod);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithJsonDeserialize_WhenCalled_ShouldReturnUnsafeMethodWithResultFromJson(IUnsafeMethod unsafeMethod)
    {
        // Arrange 
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);

        // Act
        var result = unsafeMethodWithResult.WithJsonDeserialize<object>();

        // Assert
        result.Should().BeOfType<UnsafeMethodWithResultAsGeneric<object>>();
        result.Endpoint.Should().BeSameAs(unsafeMethod.Endpoint);
        result.HttpMethod.Should().Be(unsafeMethod.HttpMethod);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithJsonDeserialize_WhenJsonSerializerSettingsIsNull_ShouldThrowArgumentNullException(IUnsafeMethod unsafeMethod)
    {
        // Arrange
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        JsonNet.JsonSerializerSettings jsonSerializerSettings = null;

        // Act
        Action action = () => _ = unsafeMethodWithResult.WithJsonDeserialize<string>(jsonSerializerSettings);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithJsonDeserialize_WhenJsonSerializerSettingsIsNotNull_ShouldReturnUnsafeMethodWithResultFromJson(IUnsafeMethod unsafeMethod)
    {
        // Arrange
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        var jsonSerializerSettings = new JsonNet.JsonSerializerSettings();

        // Act
        var result = unsafeMethodWithResult.WithJsonDeserialize<string>(jsonSerializerSettings);

        // Assert
        result.Should().BeOfType<UnsafeMethodWithResultAsGeneric<string>>();
        result.Endpoint.Should().BeSameAs(unsafeMethod.Endpoint);
        result.HttpMethod.Should().Be(unsafeMethod.HttpMethod);
    }

    [Test]
    [AutoNSubstituteData]
    public async Task WithJsonDeserialize_WhenJsonSerializerSettingsIsNotNull_ShouldDeserializeResponseFromJson(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        IUnsafeMethod unsafeMethod)
    {
        // Arrange
        var jsonSerializerSettings = new JsonNet.JsonSerializerSettings();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        var expectedResponse = new PublicDummyClass
        {
            Message = "Data"
        };
        mockHttpMessageHandler.ResponseContent = JsonNet.JsonConvert.SerializeObject(expectedResponse);

        // Act
        var response = await unsafeMethodWithResult
            .WithJsonDeserialize<PublicDummyClass>(jsonSerializerSettings)
            .SendAsync();

        // Assert
        response.Should().BeEquivalentTo(expectedResponse);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithJsonDeserialize_WhenJsonSerializerOptionsIsNull_ShouldThrowArgumentNullException(IUnsafeMethod unsafeMethod)
    {
        // Arrange
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        TextJson.JsonSerializerOptions jsonSerializerOptions = null;

        // Act
        Action action = () => _ = unsafeMethodWithResult.WithJsonDeserialize<string>(jsonSerializerOptions);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithJsonDeserialize_WhenJsonSerializerOptionsIsNotNull_ShouldReturnUnsafeMethodWithResultFromJson(IUnsafeMethod unsafeMethod)
    {
        // Arrange
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        var jsonSerializerOptions = TextJson.JsonSerializerOptions.Default;

        // Act
        var result = unsafeMethodWithResult.WithJsonDeserialize<string>(jsonSerializerOptions);

        // Assert
        result.Should().BeOfType<UnsafeMethodWithResultAsGeneric<string>>();
        result.Endpoint.Should().BeSameAs(unsafeMethod.Endpoint);
        result.HttpMethod.Should().Be(unsafeMethod.HttpMethod);
    }

    [Test]
    [AutoNSubstituteData]
    public async Task WithJsonDeserialize_WhenJsonSerializerOptionsIsNotNull_ShouldDeserializeResponseFromJson(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        IUnsafeMethod unsafeMethod)
    {
        // Arrange
        var jsonSerializerOptions = TextJson.JsonSerializerOptions.Default;
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        var expectedResponse = new PublicDummyClass
        {
            Message = "Data"
        };
        mockHttpMessageHandler.ResponseContent = TextJson.JsonSerializer.Serialize(expectedResponse);

        // Act
        var response = await unsafeMethodWithResult
            .WithJsonDeserialize<PublicDummyClass>(jsonSerializerOptions)
            .SendAsync();

        // Assert
        response.Should().BeEquivalentTo(expectedResponse);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithXmlDeserialize_WhenCalled_ShouldReturnUnsafeMethodWithResultFromXml(IUnsafeMethod unsafeMethod)
    {
        // Arrange
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);

        // Act
        var result = unsafeMethodWithResult.WithXmlDeserialize<object>();

        // Assert
        result.Should().BeOfType<UnsafeMethodWithResultAsGeneric<object>>();
        result.Endpoint.Should().BeSameAs(unsafeMethod.Endpoint);
        result.HttpMethod.Should().Be(unsafeMethod.HttpMethod);
    }

    [Test]
    [AutoNSubstituteData]
    public async Task WithXmlDeserialize_WhenCalled_ShouldDeserializeResponseFromXml(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        IUnsafeMethod unsafeMethod)
    {
        // Arrange 
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        var expectedResponse = new PublicDummyClass
        {
            Message = "Data"
        };

        mockHttpMessageHandler.ResponseContent = @"
        <PublicDummyClass xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
            <Message>Data</Message>
        </PublicDummyClass>";

        // Act
        var response = await unsafeMethodWithResult
            .WithXmlDeserialize<PublicDummyClass>()
            .SendAsync();

        // Assert
        response.Should().BeEquivalentTo(expectedResponse);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithGenericDeserialize_WhenDeserializeStringFunctionIsNull_ShouldThrowArgumentNullException(IUnsafeMethod unsafeMethod)
    {
        // Arrange
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        Func<string, string> deserializeStringFunction = null;

        // Act
        Action action = () => _ = unsafeMethodWithResult.WithGenericDeserialize(deserializeStringFunction);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithGenericDeserialize_WhenDeserializeStringFunctionIsNotNull_ShouldReturnUnsafeMethodWithResultFromJson(IUnsafeMethod unsafeMethod)
    {
        // Arrange
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        static string DeserializeStringFunction(string value)
            => null;

        // Act
        var result = unsafeMethodWithResult.WithGenericDeserialize(DeserializeStringFunction);

        // Assert
        result.Should().BeOfType<UnsafeMethodWithResultAsGeneric<string>>();
        result.Endpoint.Should().BeSameAs(unsafeMethod.Endpoint);
        result.HttpMethod.Should().Be(unsafeMethod.HttpMethod);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithGenericDeserialize_WhenDeserializeStreamFunctionIsNull_ShouldThrowArgumentNullException(IUnsafeMethod unsafeMethod)
    {
        // Arrange
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        Func<Stream, string> deserializeStringFunction = null;

        // Act
        Action action = () => _ = unsafeMethodWithResult.WithGenericDeserialize(deserializeStringFunction);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void WithGenericDeserialize_WhenDeserializeStreamFunctionIsNotNull_ShouldReturnUnsafeMethodWithResultFromJson(IUnsafeMethod unsafeMethod)
    {
        // Arrange
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        static string DeserializeStreamFunction(Stream stream)
            => null;

        // Act
        var result = unsafeMethodWithResult.WithGenericDeserialize(DeserializeStreamFunction);

        // Assert
        result.Should().BeOfType<UnsafeMethodWithResultAsGeneric<string>>();
        result.Endpoint.Should().BeSameAs(unsafeMethod.Endpoint);
        result.HttpMethod.Should().Be(unsafeMethod.HttpMethod);
    }

    public sealed class PublicDummyClass
    {
        public string Message { get; set; }
    }
}
