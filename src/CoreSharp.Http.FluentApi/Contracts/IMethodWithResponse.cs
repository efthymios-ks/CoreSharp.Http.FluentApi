using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;

namespace CoreSharp.Http.FluentApi.Contracts;

public interface IMethodWithResponse : IMethod
{
    //Methods 
    /// <summary>
    /// Check <see cref="HeaderNames.ContentType"/>
    /// in <see cref="HttpResponseMessage.Headers"/>
    /// and deserialize accordingly.
    /// </summary>
    IGenericResponse<TResponse> To<TResponse>()
        where TResponse : class;

    /// <inheritdoc cref="Json{TResponse}(Func{Stream, TResponse})" />
    IJsonResponse<TResponse> Json<TResponse>()
        where TResponse : class;

    /// <inheritdoc cref="Json{TResponse}(JsonSerializerSettings)" />
    IJsonResponse<TResponse> Json<TResponse>(JsonSerializerSettings jsonSerializerSettings)
        where TResponse : class;

    /// <inheritdoc cref="Json{TResponse}(Func{Stream, TResponse})" />
    IJsonResponse<TResponse> Json<TResponse>(JsonSerializerOptions jsonSerializerOptions)
      where TResponse : class;

    /// <inheritdoc cref="Json{TResponse}(Func{string, TResponse})" />
    IJsonResponse<TResponse> Json<TResponse>(Func<Stream, TResponse> deserializeStringFunction)
        where TResponse : class;

    /// <summary>
    /// Treat <see cref="HttpResponseMessage.Content"/>
    /// as json and deserialize to provided type.
    /// </summary>
    IJsonResponse<TResponse> Json<TResponse>(Func<string, TResponse> deserializeStreamFunction)
        where TResponse : class;

    /// <inheritdoc cref="Xml{TResponse}(Func{Stream, TResponse})" />
    IXmlResponse<TResponse> Xml<TResponse>()
        where TResponse : class;

    /// <inheritdoc cref="Xml{TResponse}(Func{string, TResponse})"/>
    IXmlResponse<TResponse> Xml<TResponse>(Func<Stream, TResponse> deserializeStreamFunction)
        where TResponse : class;

    /// <summary>
    /// Treat <see cref="HttpResponseMessage.Content"/>
    /// as xml and deserialize to provided type.
    /// </summary>
    IXmlResponse<TResponse> Xml<TResponse>(Func<string, TResponse> deserializeStringFunction)
        where TResponse : class;

    /// <summary>
    /// Serialize the HTTP content to a string.
    /// </summary>
    IStringResponse String();

    /// <summary>
    /// Serialize the HTTP content and return a
    /// stream that represents the content.
    /// </summary>
    IStreamResponse Stream();

    /// <summary>
    /// Serialize the HTTP content to a byte array.
    /// </summary>
    IBytesResponse Bytes();
}
