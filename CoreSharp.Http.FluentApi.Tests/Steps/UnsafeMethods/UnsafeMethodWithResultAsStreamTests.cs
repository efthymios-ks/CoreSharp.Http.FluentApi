using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;
using System.Text;
using Tests.Common.Mocks;

namespace CoreSharp.Http.FluentApi.Tests.Steps.UnsafeMethods;

public sealed class UnsafeMethodWithResultAsStreamTests : ProjectTestsBase
{
    [Fact]
    public void Constructor_WhenCalled_ShouldNotThrow()
    {
        var unsafeMethod = MockCreate<IUnsafeMethod>();

        void Action()
            => _ = new UnsafeMethodWithResultAsStream(unsafeMethod);

        Assert.Null(Record.Exception(Action));
    }

    [Fact]
    public async Task SendAsync_WhenHttpResponseIsNull_ShouldReturnEmptyMemoryStream()
    {
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var unsafeMethod = MockCreate<IUnsafeMethod>();

        // Arrange
        mockHttpMessageHandler.HttpResponseMessageFactory = () => null!;
        var unsafeMethodWithResultAsStream = new UnsafeMethodWithResultAsStream(unsafeMethod);

        // Act
        using var resultAsStream = await unsafeMethodWithResultAsStream.SendAsync();

        // Assert
        Assert.IsType<MemoryStream>(resultAsStream);
        Assert.Equal(0, resultAsStream.Length);
    }

    [Fact]
    public async Task SendAsync_WhenCalled_ShouldReturnByteArray()
    {
        var mockHttpMessageHandler = MockFreeze<MockHttpMessageHandler>();
        var unsafeMethod = MockCreate<IUnsafeMethod>();

        // Arrange
        var unsafeMethodWithResultAsStream = new UnsafeMethodWithResultAsStream(unsafeMethod);
        mockHttpMessageHandler.HttpResponseMessageFactory = () => new()
        {
            Content = new StringContent("Dummy data")
        };
        var expectedResultAsBytes = Encoding.UTF8.GetBytes("Dummy data");

        // Act
        using var resultAsStream = await unsafeMethodWithResultAsStream.SendAsync();

        // Assert
        using var resultMemoryStream = new MemoryStream();
        await resultAsStream!.CopyToAsync(resultMemoryStream);
        var resultAsBytes = resultMemoryStream.ToArray();
        Assert.Equivalent(expectedResultAsBytes, resultAsBytes);
    }
}
