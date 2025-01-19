using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using System.Text.Json;
using Tests.Common.Mocks;

namespace CoreSharp.Http.FluentApi.Tests.Steps.SafeMethods;

public sealed class SafeMethodWithResultAsGenericTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenSafeMethodIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        ISafeMethod safeMethod = null!;
        Func<Stream, Task<string?>> deserializeFunction = null!;

        // Act
        void Action()
            => _ = new SafeMethodWithResultAsGeneric<string>(safeMethod, deserializeFunction);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenDeserializeFunctionIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();
        Func<Stream, Task<string?>> deserializeFunction = null!;

        // Act
        void Action()
            => _ = new SafeMethodWithResultAsGeneric<string>(safeMethod, deserializeFunction);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenCalled_ShouldSetProperties()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();
        Func<Stream, Task<string?>> deserializeFunction = response => Task.FromResult<string?>(null!);

        // Act
        var safeMethodWithResultFromJson = new SafeMethodWithResultAsGeneric<string>(safeMethod, deserializeFunction);

        // Assert
        var safeMethodWithResultFromJsonAsInterface = (ISafeMethodWithResultAsGeneric<string>)safeMethodWithResultFromJson;
        Assert.Same(deserializeFunction, safeMethodWithResultFromJsonAsInterface.DeserializeFunction);
    }

    [Fact]
    public void WithCache_WhenCalled_ShouldReturnSafeMethodWithResultFromJsonAndCache()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();

        static Task<string?> DeserializeFunction(Stream _)
            => Task.FromResult<string?>(null!);

        var safeMethodWithResultFromJson = new SafeMethodWithResultAsGeneric<string>(safeMethod, DeserializeFunction);
        var duration = TimeSpan.FromMinutes(1);

        // Act
        var safeMethodWithResultFromJsonAndCache = safeMethodWithResultFromJson.WithCache(duration);

        // Assert
        Assert.IsType<SafeMethodWithResultAsGenericAndCache<string>>(safeMethodWithResultFromJsonAndCache);
        Assert.Equal(duration, safeMethodWithResultFromJsonAndCache.CacheDuration);
    }

    [Fact]
    public async Task SendAsync_WhenCancellationIsRequested_ShouldThrowTaskCancelledException()
    {
        // Arrange 
        var safeMethod = MockCreate<ISafeMethod>();
        safeMethod.Endpoint!.Request!.ThrowOnError = true;

        Task<string?> Deserialize(Stream response)
            => Task.FromResult<string?>(null!);

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        var safeMethodWithResultFromJson = new SafeMethodWithResultAsGeneric<string>(safeMethod, Deserialize);

        // Act
        async Task Action()
            => await safeMethodWithResultFromJson.SendAsync(cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(Action);
    }

    [Fact]
    public async Task SendAsync_WhenHttpResponseIsNull_ShouldReturnNull()
    {
        // Arrange 
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        mockHttpMessageHandler.HttpResponseMessageFactory = () => null!;
        var safeMethod = MockCreate<ISafeMethod>();

        static Task<DummyEntity?> Deserialize(Stream _) => Task.FromResult<DummyEntity?>(new()
        {
            Name = "Test"
        });

        var safeMethodWithResultFromJson = new SafeMethodWithResultAsGeneric<DummyEntity>(safeMethod, Deserialize);

        // Act
        var result = await safeMethodWithResultFromJson.SendAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SendAsync_WhenCalled_ShouldReturnDeserializedObject()
    {
        // Arrange
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var safeMethod = MockCreate<ISafeMethod>();
        var expectedResult = new DummyEntity
        {
            Name = "Test"
        };

        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent(JsonSerializer.Serialize(expectedResult))
        };

        static Task<DummyEntity?> Deserialize(Stream response)
        {
            var dummy = JsonSerializer.Deserialize<DummyEntity>(response);
            return Task.FromResult(dummy);
        }

        var safeMethodWithResultFromJson = new SafeMethodWithResultAsGeneric<DummyEntity>(safeMethod, Deserialize);

        // Act
        var result = await safeMethodWithResultFromJson.SendAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(expectedResult, result);
    }

    public sealed class DummyEntity
    {
        public required string Name { get; set; }
    }
}
