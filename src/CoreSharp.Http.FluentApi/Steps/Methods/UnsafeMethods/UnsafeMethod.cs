using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.Abstracts;
using CoreSharp.Http.FluentApi.Utilities;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
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
    HttpContent IUnsafeMethod.HttpContent { get; set; }

    // Methods
    public IUnsafeMethod WithJsonBody(string json)
        => WithBody(json, MediaTypeNames.Application.Json);

    public IUnsafeMethod WithJsonBody(object content)
    {
        ArgumentNullException.ThrowIfNull(content);

        var streamContent = ToJsonStreamContent(content);
        return WithBody(streamContent);
    }

    public IUnsafeMethod WithJsonBody(Stream content)
    {
        ArgumentNullException.ThrowIfNull(content);

        var contentAsStream = ToJsonStreamContent(content);
        return WithBody(contentAsStream);
    }

    public IUnsafeMethod WithXmlBody(string xml)
        => WithBody(xml, MediaTypeNames.Application.Xml);

    public IUnsafeMethod WithXmlBody(Stream content)
    {
        ArgumentNullException.ThrowIfNull(content);

        var contentAsStream = ToXmlStreamContent(content);
        return WithBody(contentAsStream);
    }

    public IUnsafeMethod WithXmlBody(object content)
    {
        ArgumentNullException.ThrowIfNull(content);

        var xml = content.ToXml();
        return WithXmlBody(xml);
    }

    public IUnsafeMethod WithBody(string content, string mediaTypeName)
        => WithBody(content, Encoding.UTF8, mediaTypeName);

    public IUnsafeMethod WithBody(string content, Encoding encoding, string mediaTypeName)
        => WithBody(new StringContent(content, encoding, mediaTypeName));

    public IUnsafeMethod WithBody(HttpContent content)
    {
        Me.HttpContent = content;
        return this;
    }

    public override Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default)
        => IMethodUtils.SendAsync(this, httpContent: null, cancellationToken: cancellationToken);

    private static Stream ToJsonStream(object entity, int bufferSize = DefaultBufferSize)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var serializer = JsonSerializer.Create();
        var stream = new MemoryStream();
        using var streamWriter = new StreamWriter(stream, Encoding.UTF8, bufferSize, true);
        using var jsonWriter = new JsonTextWriter(streamWriter);

        serializer.Serialize(jsonWriter, entity);
        streamWriter.Flush();
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        return stream;
    }

    private static StreamContent ToJsonStreamContent(object entity, int bufferSize = DefaultBufferSize)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var stream = ToJsonStream(entity);
        return ToJsonStreamContent(stream, bufferSize);
    }

    private static StreamContent ToJsonStreamContent(Stream stream, int bufferSize = DefaultBufferSize)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return ToStreamContent(stream, MediaTypeNames.Application.Json, bufferSize);
    }

    private static StreamContent ToXmlStreamContent(Stream stream, int bufferSize = DefaultBufferSize)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return ToStreamContent(stream, MediaTypeNames.Application.Xml, bufferSize);
    }

    private static StreamContent ToStreamContent(Stream stream, string mediaTypeName, int bufferSize = DefaultBufferSize)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentException.ThrowIfNullOrEmpty(mediaTypeName);

        var streamContent = new StreamContent(stream, bufferSize);
        streamContent.Headers.ContentType = new(mediaTypeName);
        return streamContent;
    }
}
