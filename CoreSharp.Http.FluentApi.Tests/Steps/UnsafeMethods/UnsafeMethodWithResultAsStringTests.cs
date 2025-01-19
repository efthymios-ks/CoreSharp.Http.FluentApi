using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;
using Tests.Common.Mocks;

namespace CoreSharp.Http.FluentApi.Tests.Steps.UnsafeMethods;

public sealed class UnsafeMethodWithResultAsStringTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenCalled_ShouldNotThrow()
    {
        var unsafeMethod = MockCreate<IUnsafeMethod>();

        void Action()
            => _ = new UnsafeMethodWithResultAsString(unsafeMethod);

        // Assert
        var exception = Record.Exception(Action);
        Assert.Null(exception);
    }

    [Fact]
    public async Task SendAsync_WhenHttpResponseIsNull_ShouldReturnNull()
    {
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var unsafeMethod = MockCreate<IUnsafeMethod>();

        // Arrange
        mockHttpMessageHandler.HttpResponseMessageFactory = () => null!;
        var unsafeMethodWithResultAsString = new UnsafeMethodWithResultAsString(unsafeMethod);

        // Act
        var result = await unsafeMethodWithResultAsString.SendAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SendAsync_WhenCalled_ShouldReturnString()
    {
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var unsafeMethod = MockCreate<IUnsafeMethod>();

        // Arrange
        var unsafeMethodWithResultAsString = new UnsafeMethodWithResultAsString(unsafeMethod);
        const string responseContent = "Dummy data";
        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent(responseContent)
        };

        // Act
        var result = await unsafeMethodWithResultAsString.SendAsync();

        // Assert
        Assert.Equivalent(responseContent, result);
    }
}
