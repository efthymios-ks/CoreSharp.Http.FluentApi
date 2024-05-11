using Microsoft.Net.Http.Headers;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;

/// <summary>
/// An HTTP method is unsafe if it alters the state of the server.
/// HTTP methods that are safe: POST, PUT, PATCH, DELETE.
/// </summary>
public interface IUnsafeMethod : IMethod
{
    // Properties
    internal HttpContent HttpContent { get; set; }

    // Methods
    /// <inheritdoc cref="WithJsonBody(Stream)" />
    IUnsafeMethod WithJsonBody(string content);

    /// <inheritdoc cref="WithJsonBody(Stream)" />
    IUnsafeMethod WithJsonBody(object content);

    /// <summary>
    /// Set <see cref="HttpRequestMessage.Content"/>
    /// and <see cref="HeaderNames.ContentEncoding"/>
    /// to <see cref="MediaTypeNames.Application.Json"/>.
    /// </summary>
    IUnsafeMethod WithJsonBody(Stream content);

    /// <inheritdoc cref="WithXmlBody(Stream)" />
    IUnsafeMethod WithXmlBody(string content);

    /// <inheritdoc cref="WithXmlBody(string)" />
    IUnsafeMethod WithXmlBody(object content);

    /// <summary>
    /// Set <see cref="HttpRequestMessage.Content"/>
    /// and <see cref="HeaderNames.ContentEncoding"/>
    /// to <see cref="MediaTypeNames.Application.Xml"/>.
    /// </summary>
    IUnsafeMethod WithXmlBody(Stream content);

    /// <inheritdoc cref="WithBody(string, Encoding, string)" />
    IUnsafeMethod WithBody(string content, string mediaType);

    /// <inheritdoc cref="WithBody(HttpContent)" />
    IUnsafeMethod WithBody(string content, Encoding encoding, string mediaType);

    /// <summary>
    /// Set <see cref="HttpRequestMessage.Content"/>.
    /// </summary>
    IUnsafeMethod WithBody(HttpContent content);
}
