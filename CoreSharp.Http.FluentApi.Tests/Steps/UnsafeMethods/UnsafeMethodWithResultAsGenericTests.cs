using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;
using System.Text.Json;
using Tests.Common.Mocks;

namespace CoreSharp.Http.FluentApi.Tests.Steps.UnsafeMethods;

public sealed class UnsafeMethodWithResultAsGenericTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenUnsafeMethodIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IUnsafeMethod unsafeMethod = null!;
        static Task<string?> DeserializeFunction(Stream response)
            => Task.FromResult<string?>(null!);

        // Act
        void Action()
            => _ = new UnsafeMethodWithResultAsGeneric<string>(unsafeMethod, DeserializeFunction);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenDeserializeFunctionIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        Func<Stream, Task<string?>> deserializeFunction = null!;

        // Act
        void Action()
            => _ = new UnsafeMethodWithResultAsGeneric<string>(unsafeMethod, deserializeFunction);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenDeserializeFunctionIsNotNull_ShouldSetDeserializeFunction()
    {
        // Arrange
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        Func<Stream, Task<string?>> deserializeFunction = response => null!;

        // Act
        var unsafeMethodWithResultFromJson = new UnsafeMethodWithResultAsGeneric<string>(unsafeMethod, deserializeFunction);

        // Assert
        var unsafeMethodWithResultFromJsonAsInterface = (IUnsafeMethodWithResultAsGeneric<string>)unsafeMethodWithResultFromJson;
        Assert.Same(deserializeFunction, unsafeMethodWithResultFromJsonAsInterface.DeserializeFunction);
    }

    [Fact]
    public async Task SendAsync_WhenCancellationIsRequested_ShouldThrowTaskCancelledException()
    {
        // Arrange 
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        unsafeMethod.Endpoint!.Request!.ThrowOnError = true;
        Task<string?> Deserialize(Stream response)
            => Task.FromResult<string?>(null!);

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        var unsafeMethodWithResultFromJson = new UnsafeMethodWithResultAsGeneric<string>(unsafeMethod, Deserialize);

        // Act
        async Task Action()
            => await unsafeMethodWithResultFromJson.SendAsync(cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(Action);
    }

    [Fact]
    public async Task SendAsync_WhenHttpResponseIsNull_ShouldReturnNull()
    {
        // Arrange 
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        mockHttpMessageHandler.HttpResponseMessageFactory = () => null!;
        var unsafeMethod = MockCreate<IUnsafeMethod>();

        static Task<DummyEntity?> Deserialize(Stream _) => Task.FromResult<DummyEntity?>(new()
        {
            Name = "Test"
        });

        var unsafeMethodWithResultFromJson = new UnsafeMethodWithResultAsGeneric<DummyEntity>(unsafeMethod, Deserialize);

        // Act
        var result = await unsafeMethodWithResultFromJson.SendAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SendAsync_WhenCalled_ShouldReturnDeserializedObject()
    {
        // Arrange
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var unsafeMethod = MockCreate<IUnsafeMethod>();
        var expectedResult = new DummyEntity
        {
            Name = "Test"
        };
        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent(JsonSerializer.Serialize(expectedResult))
        };

        static Task<DummyEntity?> DeserializeFunction(Stream response)
        {
            var dummy = JsonSerializer.Deserialize<DummyEntity>(response);
            return Task.FromResult(dummy);
        }

        var unsafeMethodWithResultFromJson = new UnsafeMethodWithResultAsGeneric<DummyEntity>(unsafeMethod, DeserializeFunction);

        // Act
        var result = await unsafeMethodWithResultFromJson.SendAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(expectedResult, result);
    }

    public sealed class DummyEntity
    {
        public required string Name { get; set; }
    }
}
