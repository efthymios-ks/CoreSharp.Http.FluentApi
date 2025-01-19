using CoreSharp.Http.FluentApi.Extensions;
using System.Text;
using Tests.Common.Mocks;

namespace CoreSharp.Http.FluentApi.Tests.Extensions;

public sealed class StreamExtensionsTests : ProjectTestsBase
{
    // Methods
    [Fact]
    public void GetBytes_WhenStreamIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        using Stream stream = null!;

        // Act
        void Action()
            => stream.GetBytes();

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task GetBytes_WhenStreamIsMemoryStream_ShouldReturnByteArrayForMemoryStream()
    {
        // Arrange 
        const string data = "Data";
        var buffer = Encoding.UTF8.GetBytes(data);
        await using var stream = MockCreate<MemoryStream>();
        stream.ToArray().Returns(buffer);

        // Act
        var streamReturned = stream.GetBytes();

        // Assert
        Assert.NotNull(streamReturned);
        Assert.Equivalent(buffer, streamReturned);
        stream.Received(1).ToArray();
    }

    [Fact]
    public async Task GetBytes_WhenStreamIsNotMemoryStream_ShouldReturnByteArray()
    {
        // Arrange
        const string data = "Data";
        var buffer = Encoding.UTF8.GetBytes(data);
        await using var stream = new MockStream(buffer);

        // Act
        var streamReturned = stream.GetBytes();

        // Assert
        Assert.NotNull(streamReturned);
        Assert.Equivalent(buffer, streamReturned);
    }
}
