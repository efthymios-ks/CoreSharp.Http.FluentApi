using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using System.Text;
using Tests.Common.Mocks;

namespace CoreSharp.Http.FluentApi.Tests.Steps.SafeMethods;

public sealed class SafeMethodWithResultAsStreamTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenCalled_ShouldNotThrow()
    {
        // Arrange
        var safeMethod = MockCreate<ISafeMethod>();

        // Act
        void Action()
            => _ = new SafeMethodWithResultAsStream(safeMethod);

        // Assert
        var exception = Record.Exception(Action);
        Assert.Null(exception);
    }

    [Fact]
    public void WithCache_WhenCalled_ShouldReturnSafeMethodWithResultAsStreamAndCache()
    {
        // Arrange 
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResultAsStream = new SafeMethodWithResultAsStream(safeMethod);
        const int cacheDurationSeconds = 1;
        var cacheDuration = TimeSpan.FromSeconds(cacheDurationSeconds);

        // Act
        var result = safeMethodWithResultAsStream.WithCache(cacheDuration);

        // Assert
        Assert.IsType<SafeMethodWithResultAsStreamAndCache>(result);
        Assert.Equal(cacheDuration, result.CacheDuration);
    }

    [Fact]
    public async Task SendAsync_WhenHttpResponseIsNull_ShouldReturnEmptyMemoryStream()
    {
        // Arrange
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        mockHttpMessageHandler.HttpResponseMessageFactory = () => null!;
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResultAsStream = new SafeMethodWithResultAsStream(safeMethod);

        // Act
        using var resultAsStream = await safeMethodWithResultAsStream.SendAsync();

        // Assert
        Assert.NotNull(resultAsStream);
        Assert.IsType<MemoryStream>(resultAsStream);
        Assert.Equal(0, resultAsStream.Length);
    }

    [Fact]
    public async Task SendAsync_WhenCalled_ShouldReturnByteArray()
    {
        // Arrange
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var safeMethod = MockCreate<ISafeMethod>();
        var safeMethodWithResultAsStream = new SafeMethodWithResultAsStream(safeMethod);
        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent("Dummy data")
        };
        var expectedResultAsBytes = Encoding.UTF8.GetBytes("Dummy data");

        // Act
        using var resultAsStream = await safeMethodWithResultAsStream.SendAsync();

        // Assert
        using var resultMemoryStream = new MemoryStream();
        await resultAsStream!.CopyToAsync(resultMemoryStream);
        var resultAsBytes = resultMemoryStream.ToArray();
        Assert.Equivalent(expectedResultAsBytes, resultAsBytes);
    }
}
