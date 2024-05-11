using AutoFixture.NUnit3;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Tests.Internal.Attributes;
using Tests.Internal.HttpmessageHandlers;

namespace Tests.Steps.SafeMethods;

[TestFixture]
public sealed class SafeMethodWithResultAsGenericTests
{
    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenSafeMethodIsNullAndDeserializeFromStreamFunctionIsProvided_ShouldThrowArgumentNullException()
    {
        // Arrange
        ISafeMethod safeMethod = null;
        static string DeserializeStreamFunction(Stream response)
            => null;

        // Act
        Action action = () => _ = new SafeMethodWithResultAsGeneric<string>(safeMethod, DeserializeStreamFunction);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenDeserializeStreamFunctionIsNull_ShouldThrowArgumentNullException(ISafeMethod safeMethod)
    {
        // Arrange
        Func<Stream, string> deserializeStreamFunction = null!;

        // Act
        Action action = () => _ = new SafeMethodWithResultAsGeneric<string>(safeMethod, deserializeStreamFunction);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenDeserializeStreamFunctionIsNotNull_ShouldSetDeserializeStreamFunction(ISafeMethod safeMethod)
    {
        // Arrange
        Func<Stream, string> deserializeStreamFunction = response => null;

        // Act
        var safeMethodWithResultFromJson = new SafeMethodWithResultAsGeneric<string>(safeMethod, deserializeStreamFunction);

        // Assert
        var safeMethodWithResultFromJsonAsInterface = (ISafeMethodWithResultAsGeneric<string>)safeMethodWithResultFromJson;
        safeMethodWithResultFromJsonAsInterface.DeserializeStreamFunction.Should().BeSameAs(deserializeStreamFunction);
        safeMethodWithResultFromJsonAsInterface.DeserializeStringFunction.Should().BeNull();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenSafeMethodIsNullAndDeserializeFromStringFunctionIsProvided_ShouldThrowArgumentNullException()
    {
        // Arrange
        ISafeMethod safeMethod = null;
        static string DeserializeStringFunction(string response)
            => null;

        // Act
        Action action = () => _ = new SafeMethodWithResultAsGeneric<string>(safeMethod, DeserializeStringFunction);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenDeserializeStringFunctionIsNull_ShouldThrowArgumentNullException(ISafeMethod safeMethod)
    {
        // Arrange
        Func<string, string> deserializeStringFunction = null!;

        // Act
        Action action = () => _ = new SafeMethodWithResultAsGeneric<string>(safeMethod, deserializeStringFunction);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenDeserializeStringFunctionIsNotNull_ShouldSetDeserializeStringFunction(ISafeMethod safeMethod)
    {
        // Arrange
        Func<string, string> deserializeStringFunction = response => null;

        // Act
        var safeMethodWithResultFromJson = new SafeMethodWithResultAsGeneric<string>(safeMethod, deserializeStringFunction);

        // Assert
        var safeMethodWithResultFromJsonAsInterface = (ISafeMethodWithResultAsGeneric<string>)safeMethodWithResultFromJson;
        safeMethodWithResultFromJsonAsInterface.DeserializeStringFunction.Should().BeSameAs(deserializeStringFunction);
        safeMethodWithResultFromJsonAsInterface.DeserializeStreamFunction.Should().BeNull();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenSafeMethodWithResultFromJsonIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        ISafeMethodWithResultAsGeneric<DummyEntity> safeMethodWithResultFromJson = null;

        // Act
        Action action = () => _ = new DummySafeMethodWithResultFromJson(safeMethodWithResultFromJson);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenSafeMethodWithResultFromJsonIsNotNull_ShouldSetBothFunctions(ISafeMethodWithResultAsGeneric<DummyEntity> safeMethodWithResultFromJson)
    {
        // Arrange
        safeMethodWithResultFromJson.DeserializeStringFunction = response => null;
        safeMethodWithResultFromJson.DeserializeStreamFunction = response => null;

        // Act
        var dummySafeMethodWithResultFromJson = new DummySafeMethodWithResultFromJson(safeMethodWithResultFromJson);

        // Assert
        var dummySafeMethodWithResultFromJsonAsInterfaace = (ISafeMethodWithResultAsGeneric<DummyEntity>)dummySafeMethodWithResultFromJson;
        dummySafeMethodWithResultFromJsonAsInterfaace.DeserializeStringFunction.Should().BeSameAs(safeMethodWithResultFromJson.DeserializeStringFunction);
        dummySafeMethodWithResultFromJsonAsInterfaace.DeserializeStreamFunction.Should().BeSameAs(safeMethodWithResultFromJson.DeserializeStreamFunction);
    }

    [Test]
    [AutoNSubstituteData]
    public void WithCache_WhenCalled_ShouldReturnSafeMethodWithResultFromJsonAndCache(ISafeMethod safeMethod)
    {
        // Arrange
        static string DeserializeStringFunction(string response)
            => null;
        var safeMethodWithResultFromJson = new SafeMethodWithResultAsGeneric<string>(safeMethod, DeserializeStringFunction);
        var duration = TimeSpan.FromMinutes(1);

        // Act
        var safeMethodWithResultFromJsonAndCache = safeMethodWithResultFromJson.WithCache(duration);

        // Assert
        safeMethodWithResultFromJsonAndCache.Should().BeOfType<SafeMethodWithResultAsGenericAndCache<string>>();
        safeMethodWithResultFromJsonAndCache.CacheDuration.Should().Be(duration);
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

        static DummyEntity DeserializeStringFunction(string response)
            => JsonSerializer.Deserialize<DummyEntity>(response);

        var safeMethodWithResultFromJson = new SafeMethodWithResultAsGeneric<DummyEntity>(safeMethod, DeserializeStringFunction);

        // Act
        var result = await safeMethodWithResultFromJson.SendAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
    }

    private sealed class DummySafeMethodWithResultFromJson(ISafeMethodWithResultAsGeneric<DummyEntity> safeMethod)
        : SafeMethodWithResultAsGeneric<DummyEntity>(safeMethod)
    {
    }

    public sealed class DummyEntity
    {
        public string Name { get; set; }
    }
}
