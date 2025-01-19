using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;
using System.Text;
using Tests.Common.Mocks;
using Xunit.Abstractions;

namespace CoreSharp.Http.FluentApi.Tests.Steps.UnsafeMethods;

public sealed class UnsafeMethodWithResultAsBytesTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenCalled_ShouldNotThrow()
    {
        var unsafeMethod = MockCreate<IUnsafeMethod>();

        void Action()
            => _ = new UnsafeMethodWithResultAsBytes(unsafeMethod);

        var exception = Record.Exception(Action);
        Assert.Null(exception);
    }

    [Fact]
    public async Task SendAsync_WhenHttpResponseIsNull_ShouldReturnEmptyArray()
    {
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var unsafeMethod = MockCreate<IUnsafeMethod>();

        // Arrange
        mockHttpMessageHandler.HttpResponseMessageFactory = () => null!;
        var safeMethodWithResultAsBytes = new UnsafeMethodWithResultAsBytes(unsafeMethod);

        // Act
        var result = await safeMethodWithResultAsBytes.SendAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task SendAsync_WhenCalled_ShouldReturnByteArray()
    {
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var unsafeMethod = MockCreate<IUnsafeMethod>();

        // Arrange
        var safeMethodWithResultAsBytes = new UnsafeMethodWithResultAsBytes(unsafeMethod);
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
