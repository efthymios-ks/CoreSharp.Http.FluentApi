using System;
using System.IO;
using JsonNet = Newtonsoft.Json;
using TextJson = System.Text.Json;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethod"/>
public interface ISafeMethodWithResult : ISafeMethod
{
    // Methods
    ISafeMethodWithResultAsBytes ToBytes();
    ISafeMethodWithResultAsStream ToStream();
    ISafeMethodWithResultAsString ToString();

    /// <inheritdoc cref="WithJsonDeserialize{TResponse}(Func{Stream, TResponse})"/>
    ISafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>()
        where TResponse : class;

    /// <inheritdoc cref="WithJsonDeserialize{TResponse}(Func{Stream, TResponse})"/>
    ISafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(JsonNet.JsonSerializerSettings jsonSerializerSettings)
        where TResponse : class;

    /// <inheritdoc cref="WithJsonDeserialize{TResponse}(Func{Stream, TResponse})"/>
    ISafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(TextJson.JsonSerializerOptions jsonSerializerOptions)
        where TResponse : class;

    /// <inheritdoc cref="WithJsonDeserialize{TResponse}(Func{Stream, TResponse})"/>
    ISafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(Func<string, TResponse> deserializeStringFunction)
        where TResponse : class;

    /// <summary>
    /// Treat response as JSON and deserialize to provided type.
    /// </summary>
    ISafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(Func<Stream, TResponse> deserializeStringFunction)
        where TResponse : class;

    /// <inheritdoc cref="WithXmlDeserialize{TResponse}(Func{Stream, TResponse})"/>
    ISafeMethodWithResultFromXml<TResponse> WithXmlDeserialize<TResponse>()
        where TResponse : class;

    /// <inheritdoc cref="WithXmlDeserialize{TResponse}(Func{Stream, TResponse})"/>
    ISafeMethodWithResultFromXml<TResponse> WithXmlDeserialize<TResponse>(Func<string, TResponse> deserializeStringFunction)
        where TResponse : class;

    /// <summary>
    /// Treat response as XML and deserialize to provided type.
    /// </summary>
    ISafeMethodWithResultFromXml<TResponse> WithXmlDeserialize<TResponse>(Func<Stream, TResponse> deserializeStreamFunction)
        where TResponse : class;

    // TODO: Add task deserialization overload.
}