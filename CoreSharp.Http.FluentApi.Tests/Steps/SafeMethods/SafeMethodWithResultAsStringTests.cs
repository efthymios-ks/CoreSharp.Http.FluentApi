using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using Tests.Common.Mocks;

namespace CoreSharp.Http.FluentApi.Tests.Steps.SafeMethods;

public sealed class SafeMethodWithResultAsStringTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenCalled_ShouldNotThrow()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();

        // Act
        void Action()
            => _ = new SafeMethodWithResultAsString(safeMethod);

        // Assert
        var exception = Record.Exception(Action);
        Assert.Null(exception);
    }

    [Fact]
    public void WithCache_WhenCalled_ShouldReturnSafeMethodWithResultAsBytesAndCache()
    {
        // Arrange 
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResultAsString = new SafeMethodWithResultAsString(safeMethod);
        const int cacheDurationSeconds = 1;
        var cacheDuration = TimeSpan.FromSeconds(cacheDurationSeconds);

        // Act
        var result = safeMethodWithResultAsString.WithCache(cacheDuration);

        // Assert
        Assert.IsType<SafeMethodWithResultAsStringAndCache>(result);
        Assert.Equal(cacheDuration, result.CacheDuration);
    }

    [Fact]
    public async Task SendAsync_WhenHttpResponseIsNull_ShouldReturnNull()
    {
        // Arrange
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        mockHttpMessageHandler.HttpResponseMessageFactory = () => null!;
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResultAsString = new SafeMethodWithResultAsString(safeMethod);

        // Act
        var result = await safeMethodWithResultAsString.SendAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SendAsync_WhenCalled_ShouldReturnString()
    {
        // Arrange
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResultAsString = new SafeMethodWithResultAsString(safeMethod);
        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent("Dummy data")
        };

        // Act
        var result = await safeMethodWithResultAsString.SendAsync();

        // Assert
        Assert.Equivalent("Dummy data", result);
    }
}
