using AutoFixture.NUnit3;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Tests.Internal.Attributes;
using Tests.Internal.HttpmessageHandlers;

namespace Tests.Steps.UnsafeMethods;

[TestFixture]
public sealed class UnsafeMethodWithResultAsGenericTests
{
    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenUnsafeMethodIsNullAndDeserializeFromStreamFunctionIsProvided_ShouldThrowArgumentNullException()
    {
        // Arrange
        IUnsafeMethod unsafeMethod = null;
        static string DeserializeStreamFunction(Stream response)
            => null;

        // Act
        Action action = () => _ = new UnsafeMethodWithResultAsGeneric<string>(unsafeMethod, DeserializeStreamFunction);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenDeserializeStreamFunctionIsNull_ShouldThrowArgumentNullException(IUnsafeMethod unsafeMethod)
    {
        // Arrange
        Func<Stream, string> deserializeStreamFunction = null!;

        // Act
        Action action = () => _ = new UnsafeMethodWithResultAsGeneric<string>(unsafeMethod, deserializeStreamFunction);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenDeserializeStreamFunctionIsNotNull_ShouldSetDeserializeStreamFunction(IUnsafeMethod unsafeMethod)
    {
        // Arrange
        Func<Stream, string> deserializeStreamFunction = response => null;

        // Act
        var unsafeMethodWithResultFromJson = new UnsafeMethodWithResultAsGeneric<string>(unsafeMethod, deserializeStreamFunction);

        // Assert
        var unsafeMethodWithResultFromJsonAsInterface = (IUnsafeMethodWithResultAsGeneric<string>)unsafeMethodWithResultFromJson;
        unsafeMethodWithResultFromJsonAsInterface.DeserializeStreamFunction.Should().BeSameAs(deserializeStreamFunction);
        unsafeMethodWithResultFromJsonAsInterface.DeserializeStringFunction.Should().BeNull();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenUnsafeMethodIsNullAndDeserializeFromStringFunctionIsProvided_ShouldThrowArgumentNullException()
    {
        // Arrange
        IUnsafeMethod unsafeMethod = null;
        static string DeserializeStringFunction(string response)
            => null;

        // Act
        Action action = () => _ = new UnsafeMethodWithResultAsGeneric<string>(unsafeMethod, DeserializeStringFunction);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenDeserializeStringFunctionIsNull_ShouldThrowArgumentNullException(IUnsafeMethod unsafeMethod)
    {
        // Arrange
        Func<string, string> deserializeStringFunction = null!;

        // Act
        Action action = () => _ = new UnsafeMethodWithResultAsGeneric<string>(unsafeMethod, deserializeStringFunction);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenDeserializeStringFunctionIsNotNull_ShouldSetDeserializeStringFunction(IUnsafeMethod unsafeMethod)
    {
        // Arrange
        Func<string, string> deserializeStringFunction = response => null;

        // Act
        var unsafeMethodWithResultFromJson = new UnsafeMethodWithResultAsGeneric<string>(unsafeMethod, deserializeStringFunction);

        // Assert
        var unsafeMethodWithResultFromJsonAsInterface = (IUnsafeMethodWithResultAsGeneric<string>)unsafeMethodWithResultFromJson;
        unsafeMethodWithResultFromJsonAsInterface.DeserializeStringFunction.Should().BeSameAs(deserializeStringFunction);
        unsafeMethodWithResultFromJsonAsInterface.DeserializeStreamFunction.Should().BeNull();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenSafeMethodWithResultFromJsonIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IUnsafeMethodWithResultAsGeneric<DummyEntity> unsafeMethodWithResultFromJson = null;

        // Act
        Action action = () => _ = new DummyUnsafeMethodWithResultFromJson(unsafeMethodWithResultFromJson);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [AutoNSubstituteData]
    public void Constructor_WhenSafeMethodWithResultFromJsonIsNotNull_ShouldSetBothFunctions(IUnsafeMethodWithResultAsGeneric<DummyEntity> unsafeMethodWithResultFromJson)
    {
        // Arrange
        unsafeMethodWithResultFromJson.DeserializeStringFunction = response => null;
        unsafeMethodWithResultFromJson.DeserializeStreamFunction = response => null;

        // Act
        var dummySafeMethodWithResultFromJson = new DummyUnsafeMethodWithResultFromJson(unsafeMethodWithResultFromJson);

        // Assert
        var dummySafeMethodWithResultFromJsonAsInterfaace = (IUnsafeMethodWithResultAsGeneric<DummyEntity>)dummySafeMethodWithResultFromJson;
        dummySafeMethodWithResultFromJsonAsInterfaace.DeserializeStringFunction.Should().BeSameAs(unsafeMethodWithResultFromJson.DeserializeStringFunction);
        dummySafeMethodWithResultFromJsonAsInterfaace.DeserializeStreamFunction.Should().BeSameAs(unsafeMethodWithResultFromJson.DeserializeStreamFunction);
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

        static DummyEntity DeserializeStringFunction(string response)
            => JsonSerializer.Deserialize<DummyEntity>(response);

        var unsafeMethodWithResultFromJson = new UnsafeMethodWithResultAsGeneric<DummyEntity>(unsafeMethod, DeserializeStringFunction);

        // Act
        var result = await unsafeMethodWithResultFromJson.SendAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
    }

    private sealed class DummyUnsafeMethodWithResultFromJson(IUnsafeMethodWithResultAsGeneric<DummyEntity> unsafeMethod)
        : UnsafeMethodWithResultAsGeneric<DummyEntity>(unsafeMethod)
    {
    }

    public sealed class DummyEntity
    {
        public string Name { get; set; }
    }
}
