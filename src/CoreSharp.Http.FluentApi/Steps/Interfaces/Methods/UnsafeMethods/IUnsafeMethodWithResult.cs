using System;
using System.IO;
using JsonNet = Newtonsoft.Json;
using TextJson = System.Text.Json;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;

/// <inheritdoc cref="IUnsafeMethod"/>
public interface IUnsafeMethodWithResult : IUnsafeMethod
{
    // Methods
    IUnsafeMethodWithResultAsBytes ToBytes();
    IUnsafeMethodWithResultAsStream ToStream();
    IUnsafeMethodWithResultAsString ToString();

    /// <inheritdoc cref="WithJsonDeserialize{TResponse}(Func{Stream, TResponse})"/>
    IUnsafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>()
        where TResponse : class;

    /// <inheritdoc cref="WithJsonDeserialize{TResponse}(Func{Stream, TResponse})"/>
    IUnsafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(JsonNet.JsonSerializerSettings jsonSerializerSettings)
      where TResponse : class;

    /// <inheritdoc cref="WithJsonDeserialize{TResponse}(Func{Stream, TResponse})"/>
    IUnsafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(TextJson.JsonSerializerOptions jsonSerializerOptions)
      where TResponse : class;

    /// <inheritdoc cref="WithJsonDeserialize{TResponse}(Func{Stream, TResponse})"/>
    IUnsafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(Func<string, TResponse> deserializeStringFunction)
       where TResponse : class;

    /// <summary>
    /// Treat response as JSON and deserialize to provided type.
    /// </summary>
    IUnsafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(Func<Stream, TResponse> deserializeStringFunction)
      where TResponse : class;

    /// <inheritdoc cref="WithXmlDeserialize{TResponse}(Func{Stream, TResponse})"/>
    IUnsafeMethodWithResultFromXml<TResponse> WithXmlDeserialize<TResponse>()
      where TResponse : class;

    /// <inheritdoc cref="WithXmlDeserialize{TResponse}(Func{Stream, TResponse})"/>
    IUnsafeMethodWithResultFromXml<TResponse> WithXmlDeserialize<TResponse>(Func<string, TResponse> deserializeStringFunction)
      where TResponse : class;

    /// <summary>
    /// Treat response as XML and deserialize to provided type.
    /// </summary>
    IUnsafeMethodWithResultFromXml<TResponse> WithXmlDeserialize<TResponse>(Func<Stream, TResponse> deserializeStreamFunction)
        where TResponse : class;
}