namespace CoreSharp.Http.FluentApi.Extensions;

/// <summary>
/// <see cref="Stream"/> extensions.
/// </summary>
internal static class StreamExtensions
{
    /// <summary>
    /// Read <see cref="Stream"/> to <see cref="byte"/> array.
    /// </summary>
    public static byte[] GetBytes(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (stream is MemoryStream memoryStream)
        {
            return memoryStream.ToArray();
        }

        using var binaryReader = new BinaryReader(stream);
        return binaryReader.ReadBytes((int)stream.Length);
    }
}
