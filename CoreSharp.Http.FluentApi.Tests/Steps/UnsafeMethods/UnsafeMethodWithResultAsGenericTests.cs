using AutoFixture.NUnit3;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;
using FluentAssertions;
using NUnit.Framework;
using System.Text.Json;
using Tests.Internal.Attributes;
using Tests.Internal.HttpmessageHandlers;

namespace Tests.Steps.UnsafeMethods;

[TestFixture]
public sealed class UnsafeMethodWithResultAsGenericTests
{
    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenUnsafeMethodIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IUnsafeMethod unsafeMethod = null!;
        static Task<string?> DeserializeFunction(Stream response)
            => Task.FromResult<string?>(null!);

        // Act
        Action action = () => _ = new UnsafeMethodWithResultAsGeneric<string>(unsafeMethod, DeserializeFunction);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenDeserializeFunctionIsNull_ShouldThrowArgumentNullException(IUnsafeMethod unsafeMethod)
    {
        // Arrange
        Func<Stream, Task<string?>> deserializeFunction = null!;

        // Act
        Action action = () => _ = new UnsafeMethodWithResultAsGeneric<string>(unsafeMethod, deserializeFunction);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenDeserializeFunctionIsNotNull_ShouldSetDeserializeFunction(IUnsafeMethod unsafeMethod)
    {
        // Arrange
        Func<Stream, Task<string?>> deserializeFunction = response => null!;

        // Act
        var unsafeMethodWithResultFromJson = new UnsafeMethodWithResultAsGeneric<string>(unsafeMethod, deserializeFunction);

        // Assert
        var unsafeMethodWithResultFromJsonAsInterface = (IUnsafeMethodWithResultAsGeneric<string>)unsafeMethodWithResultFromJson;
        unsafeMethodWithResultFromJsonAsInterface.DeserializeFunction.Should().BeSameAs(deserializeFunction);
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenCancellationIsRequested_ShouldThrowTaskCancelledException(IUnsafeMethod unsafeMethod)
    {
        // Arrange 
        unsafeMethod.Endpoint!.Request!.ThrowOnError = true;
        Func<Stream, Task<string?>> deserializeFunction = response => Task.FromResult<string?>(null!);
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        var unsafeMethodWithResultFromJson = new UnsafeMethodWithResultAsGeneric<string>(unsafeMethod, deserializeFunction);

        // Act
        Func<Task> action = () => unsafeMethodWithResultFromJson.SendAsync(cancellationTokenSource.Token);

        // Assert
        await action.Should().ThrowExactlyAsync<TaskCanceledException>();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenHttpResponseIsNull_ShouldReturnNull(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        IUnsafeMethod unsafeMethod)
    {
        // Arrange 
        mockHttpMessageHandler.SetResponseToNull = true;

        Func<Stream, Task<DummyEntity?>> deserializeFunction = _ => Task.FromResult<DummyEntity?>(new()
        {
            Name = "Test"
        });

        var unsafeMethodWithResultFromJson = new UnsafeMethodWithResultAsGeneric<DummyEntity>(unsafeMethod, deserializeFunction);

        // Act
        var result = await unsafeMethodWithResultFromJson.SendAsync();

        // Assert
        result.Should().BeNull();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenCalled_ShouldReturnDeserializedObject(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        IUnsafeMethod unsafeMethod)
    {
        // Arrange
        var expectedResult = new DummyEntity
        {
            Name = "Test"
        };
        mockHttpMessageHandler.ResponseContent = JsonSerializer.Serialize(expectedResult);

        static Task<DummyEntity?> DeserializeFunction(Stream response)
        {
            var dummy = JsonSerializer.Deserialize<DummyEntity>(response);
            return Task.FromResult(dummy);
        }

        var unsafeMethodWithResultFromJson = new UnsafeMethodWithResultAsGeneric<DummyEntity>(unsafeMethod, DeserializeFunction);

        // Act
        var result = await unsafeMethodWithResultFromJson.SendAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
    }

    public sealed class DummyEntity
    {
        public required string Name { get; set; }
    }
}
