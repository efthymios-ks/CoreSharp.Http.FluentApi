using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Json.JsonNet;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.Json;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResult"/>
public class SafeMethodWithResult :
    SafeMethod,
    ISafeMethodWithResult
{
    // Constructors 
    public SafeMethodWithResult(ISafeMethod method)
        : base(method)
    {
    }

    // Methods 
    public ISafeMethodWithResultAsBytes ToBytes()
        => new SafeMethodWithResultAsBytes(this);

    public ISafeMethodWithResultAsStream ToStream()
        => new SafeMethodWithResultAsStream(this);

    public new ISafeMethodWithResultAsString ToString()
        => new SafeMethodWithResultAsString(this);

    public ISafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>()
        where TResponse : class
        => WithJsonDeserialize<TResponse>(JsonSettings.Default);

    public ISafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(JsonSerializerSettings jsonSerializerSettings)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(jsonSerializerSettings);

        return WithJsonDeserialize(DeserializeStreamFunction);

        TResponse DeserializeStreamFunction(Stream stream)
            => stream.FromJson<TResponse>(jsonSerializerSettings);
    }

    public ISafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(JsonSerializerOptions jsonSerializerOptions)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(jsonSerializerOptions);

        return WithJsonDeserialize(DeserializeStreamFunction);

        // TODO: Fix after adding task-based deserialization.
        TResponse DeserializeStreamFunction(Stream stream)
            => stream
                .FromJsonAsync<TResponse>(jsonSerializerOptions)
                .GetAwaiter()
                .GetResult();
    }

    public ISafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(Func<string, TResponse> deserializeStringFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        return new SafeMethodWithResultFromJson<TResponse>(this, deserializeStringFunction);
    }

    public ISafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(Func<Stream, TResponse> deserializeStringFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        return new SafeMethodWithResultFromJson<TResponse>(this, deserializeStringFunction);
    }

    public ISafeMethodWithResultFromXml<TResponse> WithXmlDeserialize<TResponse>()
        where TResponse : class
    {
        return WithXmlDeserialize(DeserializeStringFunction);

        static TResponse DeserializeStringFunction(string xml)
            => xml.FromXml<TResponse>();
    }

    public ISafeMethodWithResultFromXml<TResponse> WithXmlDeserialize<TResponse>(Func<string, TResponse> deserializeStringFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        return new SafeMethodWithResultFromXml<TResponse>(this, deserializeStringFunction);
    }

    public ISafeMethodWithResultFromXml<TResponse> WithXmlDeserialize<TResponse>(Func<Stream, TResponse> deserializeStreamFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStreamFunction);

        return new SafeMethodWithResultFromXml<TResponse>(this, deserializeStreamFunction);
    }
}
