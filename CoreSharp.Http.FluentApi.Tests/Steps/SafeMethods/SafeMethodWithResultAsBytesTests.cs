using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using System.Text;
using Tests.Common.Mocks;

namespace CoreSharp.Http.FluentApi.Tests.Steps.SafeMethods;

public sealed class SafeMethodWithResultAsBytesTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenCalled_ShouldNotThrow()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();

        // Act
        void Action()
            => _ = new SafeMethodWithResultAsBytes(safeMethod);

        // Assert
        var exception = Record.Exception(Action);
        Assert.Null(exception);
    }

    [Fact]
    public void WithCache_WhenCalled_ShouldReturnSafeMethodWithResultAsBytesAndCache()
    {
        // Arrange 
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResultAsBytes = new SafeMethodWithResultAsBytes(safeMethod);
        const int cacheDurationSeconds = 1;
        var cacheDuration = TimeSpan.FromSeconds(cacheDurationSeconds);

        // Act
        var result = safeMethodWithResultAsBytes.WithCache(cacheDuration);

        // Assert
        Assert.IsType<SafeMethodWithResultAsBytesAndCache>(result);
        Assert.Equal(cacheDuration, result.CacheDuration);
    }

    [Fact]
    public async Task SendAsync_WhenHttpResponseIsNull_ShouldReturnEmptyArray()
    {
        // Arrange
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        mockHttpMessageHandler.HttpResponseMessageFactory = () => null!;
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResultAsBytes = new SafeMethodWithResultAsBytes(safeMethod);

        // Act
        var result = await safeMethodWithResultAsBytes.SendAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task SendAsync_WhenCalled_ShouldReturnByteArray()
    {
        // Arrange
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResultAsBytes = new SafeMethodWithResultAsBytes(safeMethod);
        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent("Dummy data")
        };

        var expectedResult = Encoding.UTF8.GetBytes("Dummy data");

        // Act
        var result = await safeMethodWithResultAsBytes.SendAsync();

        // Assert
        Assert.Equivalent(expectedResult, result);
    }
}
