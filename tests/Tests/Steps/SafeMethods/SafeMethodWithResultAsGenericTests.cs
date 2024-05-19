using AutoFixture.NUnit3;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Tests.Internal.Attributes;
using Tests.Internal.HttpmessageHandlers;

namespace Tests.Steps.SafeMethods;

[TestFixture]
public sealed class SafeMethodWithResultAsGenericTests
{
    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenSafeMethodIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        ISafeMethod safeMethod = null;
        Func<Stream, Task<string>> deserializeFunction = null;

        // Act
        Action action = () => _ = new SafeMethodWithResultAsGeneric<string>(safeMethod, deserializeFunction);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenDeserializeFunctionIsNull_ShouldThrowArgumentNullException(ISafeMethod safeMethod)
    {
        // Arrange
        Func<Stream, Task<string>> deserializeFunction = null;

        // Act
        Action action = () => _ = new SafeMethodWithResultAsGeneric<string>(safeMethod, deserializeFunction);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenCalled_ShouldSetProperties(ISafeMethod safeMethod)
    {
        // Arrange
        Func<Stream, Task<string>> deserializeFunction = response => Task.FromResult<string>(null);

        // Act
        var safeMethodWithResultFromJson = new SafeMethodWithResultAsGeneric<string>(safeMethod, deserializeFunction);

        // Assert
        var safeMethodWithResultFromJsonAsInterface = (ISafeMethodWithResultAsGeneric<string>)safeMethodWithResultFromJson;
        safeMethodWithResultFromJsonAsInterface.DeserializeFunction.Should().BeSameAs(deserializeFunction);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithCache_WhenCalled_ShouldReturnSafeMethodWithResultFromJsonAndCache(ISafeMethod safeMethod)
    {
        // Arrange
        Func<Stream, Task<string>> deserializeFunction = _ => Task.FromResult<string>(null);
        var safeMethodWithResultFromJson = new SafeMethodWithResultAsGeneric<string>(safeMethod, deserializeFunction);
        var duration = TimeSpan.FromMinutes(1);

        // Act
        var safeMethodWithResultFromJsonAndCache = safeMethodWithResultFromJson.WithCache(duration);

        // Assert
        safeMethodWithResultFromJsonAndCache.Should().BeOfType<SafeMethodWithResultAsGenericAndCache<string>>();
        safeMethodWithResultFromJsonAndCache.CacheDuration.Should().Be(duration);
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenCancellationIsRequested_ShouldThrowTaskCancelledException(ISafeMethod safeMethod)
    {
        // Arrange 
        safeMethod.Endpoint.Request.ThrowOnError = true;
        Func<Stream, Task<string>> deserializeFunction = response => Task.FromResult<string>(null);
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        var safeMethodWithResultFromJson = new SafeMethodWithResultAsGeneric<string>(safeMethod, deserializeFunction);

        // Act
        Func<Task> action = () => safeMethodWithResultFromJson.SendAsync(cancellationTokenSource.Token);

        // Assert
        await action.Should().ThrowExactlyAsync<TaskCanceledException>();
    }

    [Test]
    [AutoNSubstituteData]
    public async Task SendAsync_WhenCalled_ShouldReturnDeserializedObject(
        [Frozen] MockHttpMessageHandler mockHttpMessageHandler,
        ISafeMethod safeMethod)
    {
        // Arrange
        var expectedResult = new DummyEntity
        {
            Name = "Test"
        };
        mockHttpMessageHandler.ResponseContent = JsonSerializer.Serialize(expectedResult);

        Func<Stream, Task<DummyEntity>> deserializeFunction = response =>
        {
            var dummy = JsonSerializer.Deserialize<DummyEntity>(response);
            return Task.FromResult(dummy);
        };

        var safeMethodWithResultFromJson = new SafeMethodWithResultAsGeneric<DummyEntity>(safeMethod, deserializeFunction);

        // Act
        var result = await safeMethodWithResultFromJson.SendAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
    }

    public sealed class DummyEntity
    {
        public string Name { get; set; }
    }
}
