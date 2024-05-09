using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.Abstracts;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;

/// <inheritdoc cref="IUnsafeMethod"/>
public class UnsafeMethod : MethodBase, IUnsafeMethod
{
    // Fields 
    private const int DefaultBufferSize = 64 * 1024;

    // Constructors
    public UnsafeMethod(IMethod method)
        : this(method?.Endpoint, method?.HttpMethod)
    {
    }

    public UnsafeMethod(IEndpoint endpoint, HttpMethod httpMethod)
        : base(endpoint, httpMethod)
    {
    }

    // Properties 
    private IUnsafeMethod Me
        => this;
    private static Encoding DefaultEncoding
        => Encoding.UTF8;
    HttpContent IUnsafeMethod.HttpContent { get; set; }

    // Methods
    public IUnsafeMethod WithJsonBody(string content)
        => WithBody(content, MediaTypeNames.Application.Json);

    public IUnsafeMethod WithJsonBody(object content)
    {
        ArgumentNullException.ThrowIfNull(content);

        var streamContent = ToJsonStreamContent(content);
        return WithBody(streamContent);
    }

    public IUnsafeMethod WithJsonBody(Stream content)
    {
        ArgumentNullException.ThrowIfNull(content);

        var streamContent = ToJsonStreamContent(content);
        return WithBody(streamContent);
    }

    public IUnsafeMethod WithXmlBody(string content)
        => WithBody(content, MediaTypeNames.Application.Xml);

    public IUnsafeMethod WithXmlBody(object content)
    {
        ArgumentNullException.ThrowIfNull(content);

        var xml = content.ToXml();
        return WithXmlBody(xml);
    }

    public IUnsafeMethod WithXmlBody(Stream content)
    {
        ArgumentNullException.ThrowIfNull(content);

        var contentAsStream = ToXmlStreamContent(content);
        return WithBody(contentAsStream);
    }

    public IUnsafeMethod WithBody(string content, string mediaType)
        => WithBody(content, DefaultEncoding, mediaType);

    public IUnsafeMethod WithBody(string content, Encoding encoding, string mediaType)
        => WithBody(new StringContent(content, encoding, mediaType));

    public IUnsafeMethod WithBody(HttpContent content)
    {
        Me.HttpContent = content;
        return this;
    }

    public override Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default)
        => SendAsync(Me.HttpContent, cancellationToken: cancellationToken);

    private static Stream ToJsonStream(object entity)
    {
        var memoryStream = new MemoryStream();
        using var writer = new Utf8JsonWriter(memoryStream);
        JsonSerializer.Serialize(writer, entity, JsonSerializerOptions.Default);
        writer.Flush();
        memoryStream.Position = 0;

        return memoryStream;
    }

    private static StreamContent ToJsonStreamContent(object entity)
    {
        var stream = ToJsonStream(entity);
        return ToStreamContent(stream, MediaTypeNames.Application.Json);
    }

    private static StreamContent ToJsonStreamContent(Stream stream)
        => ToStreamContent(stream, MediaTypeNames.Application.Json);

    private static StreamContent ToXmlStreamContent(Stream stream)
        => ToStreamContent(stream, MediaTypeNames.Application.Xml);

    private static StreamContent ToStreamContent(Stream stream, string mediaTypeName)
    {
        var streamContent = new StreamContent(stream, DefaultBufferSize);
        streamContent.Headers.ContentType = new(mediaTypeName, DefaultEncoding.WebName);
        return streamContent;
    }
}
