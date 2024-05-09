using CoreSharp.Http.FluentApi.Extensions;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Extensions;

[TestFixture]
public sealed class StreamExtensionsTests
{
    // Methods
    [Test]
    public void GetBytes_WhenStreamIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        using Stream stream = null;

        // Act
        Action action = () => _ = stream.GetBytes();

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void GetBytes_WhenStreamIsMemoryStream_ShouldReturnByteArrayForMemoryStream()
    {
        // Arrange 
        var buffer = Encoding.UTF8.GetBytes("Data");
        using var stream = Substitute.For<MemoryStream>();
        stream.ToArray().Returns(buffer);

        // Act
        var streamReturned = stream.GetBytes();

        // Assert
        streamReturned.Should().NotBeNull();
        streamReturned.Should().BeEquivalentTo(buffer);
        stream.Received(1).ToArray();
    }

    [Test]
    public void GetBytes_WhenStreamIsNotMemoryStream_ShouldReturnByteArray()
    {
        // Arrange
        var buffer = Encoding.UTF8.GetBytes("Data");
        using var stream = new DummyStream(buffer);

        // Act
        var streamReturned = stream.GetBytes();

        // Assert
        streamReturned.Should().NotBeNull();
        streamReturned.Should().BeEquivalentTo(buffer);
        stream.BytesRead.Should().BeEquivalentTo(buffer);
    }

    private sealed class DummyStream : Stream
    {
        private readonly MemoryStream _stream;
        private readonly List<byte> _bytesRead;

        public DummyStream(byte[] buffer)
        {
            ArgumentNullException.ThrowIfNull(buffer);

            _stream = new MemoryStream(buffer);
            _bytesRead = [];
        }

        public IEnumerable<byte> BytesRead
            => _bytesRead;

        public override bool CanRead
            => _stream.CanRead;

        public override bool CanWrite
            => _stream.CanWrite;

        public override bool CanSeek
            => _stream.CanSeek;

        public override long Length
            => _stream.Length;

        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var bytesRead = _stream.Read(buffer, offset, count);
            _bytesRead.AddRange(buffer.Skip(offset).Take(bytesRead));
            return bytesRead;
        }

        public override void Write(byte[] buffer, int offset, int count)
            => _stream.Write(buffer, offset, count);

        public override void Flush()
            => _stream.Flush();

        public override Task FlushAsync(CancellationToken cancellationToken)
            => _stream.FlushAsync(cancellationToken);

        public override long Seek(long offset, SeekOrigin origin)
            => _stream.Seek(offset, origin);

        public override void SetLength(long value)
            => _stream.SetLength(value);
    }
}
