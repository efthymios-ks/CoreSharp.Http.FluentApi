using System.IO;
using System.Net.Http;
using System.Text;

namespace CoreSharp.Http.FluentApi.Contracts;

public interface IContentMethod : IMethodWithResponse
{
    // Properties
    internal HttpContent HttpContent { get; set; }

    // Methods
    /// <inheritdoc cref="Content(string, string)" />
    public IContentMethod JsonContent(string json);

    /// <inheritdoc cref="Content(HttpContent)" />
    public IContentMethod JsonContent(Stream stream);

    /// <inheritdoc cref="Content(HttpContent)" />
    public IContentMethod JsonContent(object content);

    /// <inheritdoc cref="Content(string, string)" />
    public IContentMethod XmlContent(string xml);

    /// <inheritdoc cref="Content(string, string)" />
    public IContentMethod XmlContent(Stream stream);

    /// <inheritdoc cref="Content(HttpContent)" />
    public IContentMethod XmlContent(object content);

    /// <inheritdoc cref="Content(string, Encoding, string)" />
    public IContentMethod Content(string content, string mediaTypeName);

    /// <inheritdoc cref="Content(HttpContent)" />
    public IContentMethod Content(string content, Encoding encoding, string mediaTypeName);

    /// <summary>
    /// Set <see cref="HttpRequestMessage.Content"/>.
    /// </summary>
    public IContentMethod Content(HttpContent httpContent);
}
