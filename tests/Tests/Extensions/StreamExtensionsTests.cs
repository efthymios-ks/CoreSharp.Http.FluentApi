using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace CoreSharp.Http.FluentApi.Extensions.Tests;

[TestFixture]
public class StreamExtensionsTests
{
    // Methods
    [Test]
    public void GetBytes_WhenStreamIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        Stream stream = null;

        // Act
        Action action = () => stream.GetBytes();

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void GetBytes_WhenCalled_ShouldReturnByteArray()
    {
        // Arrange
        const string inputAsString = "This is a test data.";
        var inputAsBytes = Encoding.UTF8.GetBytes(inputAsString);
        using Stream stream = new MemoryStream(
            inputAsBytes,
            index: 0,
            count: inputAsBytes.Length,
            writable: false,
            publiclyVisible: true);

        // Act
        var result = stream.GetBytes();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(inputAsBytes);
    }

    [Test]
    public void GetBytes_WhenStreamIsMemoryStream_ShouldReturnByteArrayForMemoryStream()
    {
        // Arrange
        const string inputAsString = "This is a test data.";
        var inputAsBytes = Encoding.UTF8.GetBytes(inputAsString);
        var streamMock = Substitute.For<MemoryStream>();
        streamMock.GetBuffer().Returns(inputAsBytes);

        // Act
        var result = streamMock.GetBytes();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(inputAsBytes);
        streamMock.Received(1).GetBuffer();
    }
}
