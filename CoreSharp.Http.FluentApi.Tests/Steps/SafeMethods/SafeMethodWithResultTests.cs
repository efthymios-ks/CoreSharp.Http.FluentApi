using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using System.Text;
using Tests.Common.Mocks;
using JsonNet = Newtonsoft.Json;
using TextJson = System.Text.Json;

namespace CoreSharp.Http.FluentApi.Tests.Steps.SafeMethods;

public sealed class SafeMethodWithResultTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenCalled_ShouldNotThrw()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();

        // Act
        void Action()
            => _ = new SafeMethodWithResult(safeMethod);

        // Assert 
        var exception = Record.Exception(Action);
        Assert.Null(exception);
    }

    [Fact]
    public void ToBytes_WhenCalled_ShouldReturnSafeMethodWithResultAsBytes()
    {
        // Arrange 
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);

        // Act
        var result = safeMethodWithResult.ToBytes();

        // Assert
        Assert.IsType<SafeMethodWithResultAsBytes>(result);
        Assert.Same(safeMethod.Endpoint, result.Endpoint);
        Assert.Equal(safeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public void ToStream_WhenCalled_ShouldReturnSafeMethodWithResultAsStream()
    {
        // Arrange 
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);

        // Act
        var result = safeMethodWithResult.ToStream();

        // Assert
        Assert.IsType<SafeMethodWithResultAsStream>(result);
        Assert.Same(safeMethod.Endpoint, result.Endpoint);
        Assert.Equal(safeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public void ToString_WhenCalled_ShouldReturnSafeMethodWithResultAsString()
    {
        // Arrange 
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);

        // Act
        var result = safeMethodWithResult.ToString();

        // Assert
        Assert.IsType<SafeMethodWithResultAsString>(result);
        Assert.Same(safeMethod.Endpoint, result.Endpoint);
        Assert.Equal(safeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public void WithJsonDeserialize_WhenCalled_ShouldReturnSafeMethodWithResultFromJson()
    {
        // Arrange 
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);

        // Act
        var result = safeMethodWithResult.WithJsonDeserialize<object>();

        // Assert
        Assert.IsType<SafeMethodWithResultAsGeneric<object>>(result);
        Assert.Same(safeMethod.Endpoint, result.Endpoint);
        Assert.Equal(safeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public void WithJsonDeserialize_WhenJsonSerializerSettingsIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);
        JsonNet.JsonSerializerSettings jsonSerializerSettings = null!;

        // Act
        void Action()
            => safeMethodWithResult.WithJsonDeserialize<string>(jsonSerializerSettings);

        // Assert
        var exception = Record.Exception(Action);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void WithJsonDeserialize_WhenJsonSerializerSettingsIsNotNull_ShouldReturnSafeMethodWithResultFromJson()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);
        var jsonSerializerSettings = new JsonNet.JsonSerializerSettings();

        // Act
        var result = safeMethodWithResult.WithJsonDeserialize<string>(jsonSerializerSettings);

        // Assert
        Assert.IsType<SafeMethodWithResultAsGeneric<string>>(result);
        Assert.Same(safeMethod.Endpoint, result.Endpoint);
        Assert.Equal(safeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public async Task WithJsonDeserialize_WhenJsonSerializerSettingsIsNotNull_ShouldDeserializeResponseFromJson()
    {
        // Arrange
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var safeMethod = MockCreate<ISafeMethod>();
        var jsonSerializerSettings = new JsonNet.JsonSerializerSettings();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);
        var expectedResponse = new PublicDummyClass
        {
            Message = "Data"
        };
        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent(JsonNet.JsonConvert.SerializeObject(expectedResponse))
        };

        // Act
        var response = await safeMethodWithResult
            .WithJsonDeserialize<PublicDummyClass>(jsonSerializerSettings)
            .SendAsync();

        // Assert
        Assert.Equivalent(expectedResponse, response);
    }

    [Fact]
    public void WithJsonDeserialize_WhenJsonSerializerOptionsIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);
        TextJson.JsonSerializerOptions jsonSerializerOptions = null!;

        // Act
        void Action()
            => safeMethodWithResult.WithJsonDeserialize<string>(jsonSerializerOptions);

        // Assert
        var exception = Record.Exception(Action);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void WithJsonDeserialize_WhenJsonSerializerOptionsIsNotNull_ShouldReturnSafeMethodWithResultFromJson()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);
        var jsonSerializerOptions = TextJson.JsonSerializerOptions.Default;

        // Act
        var result = safeMethodWithResult.WithJsonDeserialize<string>(jsonSerializerOptions);

        // Assert
        Assert.IsType<SafeMethodWithResultAsGeneric<string>>(result);
        Assert.Same(safeMethod.Endpoint, result.Endpoint);
        Assert.Equal(safeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public async Task WithJsonDeserialize_WhenJsonSerializerOptionsIsNotNull_ShouldDeserializeResponseFromJson()
    {
        // Arrange
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var safeMethod = MockCreate<ISafeMethod>();
        var jsonSerializerOptions = TextJson.JsonSerializerOptions.Default;
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);
        var expectedResponse = new PublicDummyClass
        {
            Message = "Data"
        };
        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent(TextJson.JsonSerializer.Serialize(expectedResponse))
        };

        // Act
        var response = await safeMethodWithResult
            .WithJsonDeserialize<PublicDummyClass>(jsonSerializerOptions)
            .SendAsync();

        // Assert
        Assert.Equivalent(expectedResponse, response);
    }

    [Fact]
    public void WithXmlDeserialize_WhenCalled_ShouldReturnSafeMethodWithResultFromXml()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);

        // Act
        var result = safeMethodWithResult.WithXmlDeserialize<object>();

        // Assert
        Assert.IsType<SafeMethodWithResultAsGeneric<object>>(result);
        Assert.Same(safeMethod.Endpoint, result.Endpoint);
        Assert.Equal(safeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public async Task WithXmlDeserialize_WhenCalled_ShouldDeserializeResponseFromXml()
    {
        // Arrange 
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);
        var expectedResponse = new PublicDummyClass
        {
            Message = "Data"
        };

        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent(@"
                <PublicDummyClass xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                    <Message>Data</Message>
                </PublicDummyClass>")
        };

        // Act
        var response = await safeMethodWithResult
            .WithXmlDeserialize<PublicDummyClass>()
            .SendAsync();

        // Assert
        Assert.Equivalent(expectedResponse, response);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeStringFunctionIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);
        Func<string, string?> deserializeFunction = null!;

        // Act
        void Action()
            => safeMethodWithResult.WithGenericDeserialize(deserializeFunction);

        // Assert
        var exception = Record.Exception(Action);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeStringFunctionIsNotNull_ShouldReturnSafeMethodWithResultFromJson()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);
        static string? Deserialize(string response)
            => null!;

        // Act
        var result = safeMethodWithResult.WithGenericDeserialize(Deserialize);

        // Assert
        Assert.IsType<SafeMethodWithResultAsGeneric<string>>(result);
        Assert.Same(safeMethod.Endpoint, result.Endpoint);
        Assert.Equal(safeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public async Task WithGenericDeserialize_WhenDeserializeStringFunctionIsNotNull_ShouldDeserializeResponse()
    {
        // Arrange 
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var safeMethod = MockCreate<ISafeMethod>();
        const string expectedResponse = "Data";
        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent("Data")
        };

        string? deserializeFunctionCapturedResponse = null;
        var deserializeFunctionCallCount = 0;
        string? Deserialize(string response)
        {
            deserializeFunctionCapturedResponse = response;
            deserializeFunctionCallCount++;
            return response;
        }

        // Act
        var response = await new SafeMethodWithResult(safeMethod)
            .WithGenericDeserialize(Deserialize)
            .SendAsync();

        // Assert
        Assert.Equivalent(expectedResponse, response);
        Assert.Equal(1, deserializeFunctionCallCount);
        Assert.Equal("Data", deserializeFunctionCapturedResponse);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeStreamFunctionIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);
        Func<Stream, string?> deserializeFunction = null!;

        // Act
        void Action()
            => safeMethodWithResult.WithGenericDeserialize(deserializeFunction);

        // Assert
        var exception = Record.Exception(Action);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeStreamFunctionIsNotNull_ShouldReturnSafeMethodWithResultFromJson()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);
        static string? Deserialize(Stream response)
            => null!;

        // Act
        var result = safeMethodWithResult.WithGenericDeserialize(Deserialize);

        // Assert
        Assert.IsType<SafeMethodWithResultAsGeneric<string>>(result);
        Assert.Same(safeMethod.Endpoint, result.Endpoint);
        Assert.Equal(safeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public async Task WithGenericDeserialize_WhenDeserializeStreamFunctionIsNotNull_ShouldDeserializeResponse()
    {
        // Arrange 
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var safeMethod = MockCreate<ISafeMethod>();
        const string expectedResponse = "Data";
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
        var response = await new SafeMethodWithResult(safeMethod)
            .WithGenericDeserialize(Deserialize)
            .SendAsync();

        // Assert
        Assert.Equivalent(expectedResponse, response);
        Assert.Equal(1, deserializeFunctionCallCount);
        Assert.Equal("Data", deserializeFunctionCapturedResponse);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeTaskStringFunctionIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);
        Func<string, Task<string?>> deserializeFunction = null!;

        // Act
        void Action()
            => safeMethodWithResult.WithGenericDeserialize(deserializeFunction);

        // Assert
        var exception = Record.Exception(Action);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeTaskStringFunctionIsNotNull_ShouldReturnSafeMethodWithResultFromJson()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);
        static Task<string?> Deserialize(string response)
            => null!;

        // Act
        var result = safeMethodWithResult.WithGenericDeserialize(Deserialize);

        // Assert
        Assert.IsType<SafeMethodWithResultAsGeneric<string>>(result);
        Assert.Same(safeMethod.Endpoint, result.Endpoint);
        Assert.Equal(safeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public async Task WithGenericDeserialize_WhenDeserializeTaskStringFunctionIsNotNull_ShouldDeserializeResponse()
    {
        // Arrange 
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var safeMethod = MockCreate<ISafeMethod>();
        const string expectedResponse = "Data";
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
        var response = await new SafeMethodWithResult(safeMethod)
            .WithGenericDeserialize(Deserialize)
            .SendAsync();

        // Assert
        Assert.Equivalent(expectedResponse, response);
        Assert.Equal(1, deserializeFunctionCallCount);
        Assert.Equal("Data", deserializeFunctionCapturedResponse);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeTaskStreamFunctionIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);
        Func<Stream, Task<string?>> deserializeFunction = null!;

        // Act
        void Action()
            => safeMethodWithResult.WithGenericDeserialize(deserializeFunction);

        // Assert
        var exception = Record.Exception(Action);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void WithGenericDeserialize_WhenDeserializeTaskStreamFunctionIsNotNull_ShouldReturnSafeMethodWithResultFromJson()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResult = new SafeMethodWithResult(safeMethod);
        static Task<string?> Deserialize(Stream response)
            => null!;

        // Act
        var result = safeMethodWithResult.WithGenericDeserialize(Deserialize);

        // Assert
        Assert.IsType<SafeMethodWithResultAsGeneric<string>>(result);
        Assert.Same(safeMethod.Endpoint, result.Endpoint);
        Assert.Equal(safeMethod.HttpMethod, result.HttpMethod);
    }

    [Fact]
    public async Task WithGenericDeserialize_WhenDeserializeTaskStreamFunctionIsNotNull_ShouldDeserializeResponse()
    {
        // Arrange 
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var safeMethod = MockCreate<ISafeMethod>();
        const string expectedResponse = "Data";
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
        var response = await new SafeMethodWithResult(safeMethod)
            .WithGenericDeserialize(Deserialize)
            .SendAsync();

        // Assert
        Assert.Equivalent(expectedResponse, response);
        Assert.Equal(1, deserializeFunctionCallCount);
        Assert.Equal("Data", deserializeFunctionCapturedResponse);
    }

    public sealed class PublicDummyClass
    {
        public required string Message { get; set; }
    }
}
