using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;
using System.Text;
using Tests.Common.Mocks;
using JsonNet = Newtonsoft.Json;
using TextJson = System.Text.Json;

namespace CoreSharp.Http.FluentApi.Tests.Steps.UnsafeMethods;

public sealed class UnsafeMethodWithResultTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenCalled_ShouldNotThrow()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();

        // Act
        void Action()
            => _ = new UnsafeMethodWithResult(unsafeMethod);

        // Assert 
        var exception = Record.Exception(Action);
        Assert.Null(exception);
    }

    [Fact]
    public void ToBytes_WhenCalled_ShouldReturnUnsafeMethodWithResultAsBytes()
    {
        // Arrange 
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);

        // Act
        var result = unsafeMethodWithResult.ToBytes();

        // Assert
        Assert.IsType<UnsafeMethodWithResultAsBytes>(result);
        Assert.Same(unsafeMethod.Endpoint, result.Endpoint);
        Assert.Equal(unsafeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public void ToStream_WhenCalled_ShouldReturnUnsafeMethodWithResultAsStream()
    {
        // Arrange 
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);

        // Act
        var result = unsafeMethodWithResult.ToStream();

        // Assert
        Assert.IsType<UnsafeMethodWithResultAsStream>(result);
        Assert.Same(unsafeMethod.Endpoint, result.Endpoint);
        Assert.Equal(unsafeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public void ToString_WhenCalled_ShouldReturnUnsafeMethodWithResultAsString()
    {
        // Arrange 
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);

        // Act
        var result = unsafeMethodWithResult.ToString();

        // Assert
        Assert.IsType<UnsafeMethodWithResultAsString>(result);
        Assert.Same(unsafeMethod.Endpoint, result.Endpoint);
        Assert.Equal(unsafeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public void WithJsonDeserialize_WhenCalled_ShouldReturnUnsafeMethodWithResultFromJson()
    {
        // Arrange 
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);

        // Act
        var result = unsafeMethodWithResult.WithJsonDeserialize<object>();

        // Assert
        Assert.IsType<UnsafeMethodWithResultAsGeneric<object>>(result);
        Assert.Same(unsafeMethod.Endpoint, result.Endpoint);
        Assert.Equal(unsafeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public void WithJsonDeserialize_WhenJsonSerializerSettingsIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        JsonNet.JsonSerializerSettings jsonSerializerSettings = null!;

        // Act
        void Action()
            => unsafeMethodWithResult.WithJsonDeserialize<string>(jsonSerializerSettings);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void WithJsonDeserialize_WhenJsonSerializerSettingsIsNotNull_ShouldReturnUnsafeMethodWithResultFromJson()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        var jsonSerializerSettings = new JsonNet.JsonSerializerSettings();

        // Act
        var result = unsafeMethodWithResult.WithJsonDeserialize<string>(jsonSerializerSettings);

        // Assert
        Assert.IsType<UnsafeMethodWithResultAsGeneric<string>>(result);
        Assert.Same(unsafeMethod.Endpoint, result.Endpoint);
        Assert.Equal(unsafeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public async Task WithJsonDeserialize_WhenJsonSerializerSettingsIsNotNull_ShouldDeserializeResponseFromJson()
    {
        // Arrange
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var jsonSerializerSettings = new JsonNet.JsonSerializerSettings();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        var expectedResponse = new PublicDummyClass
        {
            Message = "Data"
        };
        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent(JsonNet.JsonConvert.SerializeObject(expectedResponse))
        };

        // Act
        var response = await unsafeMethodWithResult
            .WithJsonDeserialize<PublicDummyClass>(jsonSerializerSettings)
            .SendAsync();

        // Assert
        Assert.Equivalent(expectedResponse, response);
    }

    [Fact]
    public void WithJsonDeserialize_WhenJsonSerializerOptionsIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        TextJson.JsonSerializerOptions jsonSerializerOptions = null!;

        // Act
        void Action()
            => unsafeMethodWithResult.WithJsonDeserialize<string>(jsonSerializerOptions);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void WithJsonDeserialize_WhenJsonSerializerOptionsIsNotNull_ShouldReturnUnsafeMethodWithResultFromJson()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        var jsonSerializerOptions = TextJson.JsonSerializerOptions.Default;

        // Act
        var result = unsafeMethodWithResult.WithJsonDeserialize<string>(jsonSerializerOptions);

        // Assert
        Assert.IsType<UnsafeMethodWithResultAsGeneric<string>>(result);
        Assert.Same(unsafeMethod.Endpoint, result.Endpoint);
        Assert.Equal(unsafeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public async Task WithJsonDeserialize_WhenJsonSerializerOptionsIsNotNull_ShouldDeserializeResponseFromJson()
    {
        // Arrange
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var jsonSerializerOptions = TextJson.JsonSerializerOptions.Default;
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        var expectedResponse = new PublicDummyClass
        {
            Message = "Data"
        };
        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent(TextJson.JsonSerializer.Serialize(expectedResponse))
        };

        // Act
        var response = await unsafeMethodWithResult
            .WithJsonDeserialize<PublicDummyClass>(jsonSerializerOptions)
            .SendAsync();

        // Assert
        Assert.Equivalent(expectedResponse, response);
    }

    [Fact]
    public void WithXmlDeserialize_WhenCalled_ShouldReturnUnsafeMethodWithResultFromXml()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);

        // Act
        var result = unsafeMethodWithResult.WithXmlDeserialize<object>();

        // Assert
        Assert.IsType<UnsafeMethodWithResultAsGeneric<object>>(result);
        Assert.Same(unsafeMethod.Endpoint, result.Endpoint);
        Assert.Equal(unsafeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public async Task WithXmlDeserialize_WhenCalled_ShouldDeserializeResponseFromXml()
    {
        // Arrange 
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        var expectedResponse = new PublicDummyClass
        {
            Message = "Data"
        };

        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent(@"<PublicDummyClass xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                    <Message>Data</Message>
                </PublicDummyClass>")
        };

        // Act
        var response = await unsafeMethodWithResult
            .WithXmlDeserialize<PublicDummyClass>()
            .SendAsync();

        // Assert
        Assert.Equivalent(expectedResponse, response);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeStringFunctionIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        Func<string, string?> deserializeFunction = null!;

        // Act
        void Action()
            => unsafeMethodWithResult.WithGenericDeserialize(deserializeFunction);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeStringFunctionIsNotNull_ShouldReturnUnsafeMethodWithResultFromJson()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        static string? Deserialize(string response)
            => null!;

        // Act
        var result = unsafeMethodWithResult.WithGenericDeserialize(Deserialize);

        // Assert
        Assert.IsType<UnsafeMethodWithResultAsGeneric<string>>(result);
        Assert.Same(unsafeMethod.Endpoint, result.Endpoint);
        Assert.Equal(unsafeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public async Task WithGenericDeserialize_WhenDeserializeStringFunctionIsNotNull_ShouldDeserializeResponse()
    {
        // Arrange 
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent("Data")
        };

        string? deserializeFunctionCapturedResponse = null;
        var deserializeFunctionCallCount = 0;
        string Deserialize(string response)
        {
            deserializeFunctionCapturedResponse = response;
            deserializeFunctionCallCount++;
            return response;
        }

        // Act
        var response = await new UnsafeMethodWithResult(unsafeMethod)
            .WithGenericDeserialize(Deserialize)
            .SendAsync();

        // Assert
        Assert.Equivalent("Data", response);
        Assert.Equal(1, deserializeFunctionCallCount);
        Assert.Equal("Data", deserializeFunctionCapturedResponse);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeStreamFunctionIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        Func<Stream, string?> deserializeFunction = null!;

        // Act
        void Action()
            => unsafeMethodWithResult.WithGenericDeserialize(deserializeFunction);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeStreamFunctionIsNotNull_ShouldReturnUnsafeMethodWithResultFromJson()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        static string? Deserialize(Stream response)
            => null!;

        // Act
        var result = unsafeMethodWithResult.WithGenericDeserialize(Deserialize);

        // Assert
        Assert.IsType<UnsafeMethodWithResultAsGeneric<string>>(result);
        Assert.Same(unsafeMethod.Endpoint, result.Endpoint);
        Assert.Equal(unsafeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public async Task WithGenericDeserialize_WhenDeserializeStreamFunctionIsNotNull_ShouldDeserializeResponse()
    {
        // Arrange 
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent("Data")
        };

        string? deserializeFunctionCapturedResponse = null;
        var deserializeFunctionCallCount = 0;
        string Deserialize(Stream response)
        {
            using var memoryStream = new MemoryStream();
            response.CopyTo(memoryStream);
            var responseAsBytes = memoryStream.ToArray();
            var responseAsString = Encoding.UTF8.GetString(responseAsBytes);

            deserializeFunctionCapturedResponse = responseAsString;
            deserializeFunctionCallCount++;
            return responseAsString;
        }

        // Act
        var response = await new UnsafeMethodWithResult(unsafeMethod)
            .WithGenericDeserialize(Deserialize)
            .SendAsync();

        // Assert
        Assert.Equivalent("Data", response);
        Assert.Equal(1, deserializeFunctionCallCount);
        Assert.Equal("Data", deserializeFunctionCapturedResponse);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeTaskStringFunctionIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        Func<string, Task<string?>> deserializeFunction = null!;

        // Act
        void Action()
            => unsafeMethodWithResult.WithGenericDeserialize(deserializeFunction);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeTaskStringFunctionIsNotNull_ShouldReturnUnsafeMethodWithResultFromJson()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        static Task<string?> Deserialize(string response)
            => Task.FromResult<string?>(null!);

        // Act
        var result = unsafeMethodWithResult.WithGenericDeserialize(Deserialize);

        // Assert
        Assert.IsType<UnsafeMethodWithResultAsGeneric<string>>(result);
        Assert.Same(unsafeMethod.Endpoint, result.Endpoint);
        Assert.Equal(unsafeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public async Task WithGenericDeserialize_WhenDeserializeTaskStringFunctionIsNotNull_ShouldDeserializeResponse()
    {
        // Arrange 
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent("Data")
        };

        string? deserializeFunctionCapturedResponse = null;
        var deserializeFunctionCallCount = 0;
        Task<string?> Deserialize(string response)
        {
            deserializeFunctionCapturedResponse = response;
            deserializeFunctionCallCount++;
            return Task.FromResult<string?>(response);
        }

        // Act
        var response = await new UnsafeMethodWithResult(unsafeMethod)
            .WithGenericDeserialize(Deserialize)
            .SendAsync();

        // Assert
        Assert.Equivalent("Data", response);
        Assert.Equal(1, deserializeFunctionCallCount);
        Assert.Equal("Data", deserializeFunctionCapturedResponse);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeTaskStreamFunctionIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        Func<Stream, Task<string?>> deserializeFunction = null!;

        // Act
        void Action()
            => unsafeMethodWithResult.WithGenericDeserialize(deserializeFunction);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeTaskStreamFunctionIsNotNull_ShouldReturnUnsafeMethodWithResultFromJson()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var unsafeMethodWithResult = new UnsafeMethodWithResult(unsafeMethod);
        static Task<string?> Deserialize(Stream stream)
            => Task.FromResult<string?>(null!);

        // Act
        var result = unsafeMethodWithResult.WithGenericDeserialize(Deserialize);

        // Assert
        Assert.IsType<UnsafeMethodWithResultAsGeneric<string>>(result);
        Assert.Same(unsafeMethod.Endpoint, result.Endpoint);
        Assert.Equal(unsafeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public async Task WithGenericDeserialize_WhenDeserializeTaskStreamFunctionIsNotNull_ShouldDeserializeResponse()
    {
        // Arrange 
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent("Data")
        };

        string? deserializeFunctionCapturedResponse = null;
        var deserializeFunctionCallCount = 0;
        async Task<string?> Deserialize(Stream response)
        {
            await using var memoryStream = new MemoryStream();
            await response.CopyToAsync(memoryStream);
            var responseAsBytes = memoryStream.ToArray();
            var responseAsString = Encoding.UTF8.GetString(responseAsBytes);

            deserializeFunctionCapturedResponse = responseAsString;
            deserializeFunctionCallCount++;
            return responseAsString;
        }

        // Act
        var response = await new UnsafeMethodWithResult(unsafeMethod)
            .WithGenericDeserialize(Deserialize)
            .SendAsync();

        // Assert
        Assert.Equivalent("Data", response);
        Assert.Equal(1, deserializeFunctionCallCount);
        Assert.Equal("Data", deserializeFunctionCapturedResponse);
    }

    /// <summary>
    /// Needs to public for XML deserialization.
    /// </summary>
    public sealed class PublicDummyClass
    {
        public required string Message { get; set; }
    }
}
