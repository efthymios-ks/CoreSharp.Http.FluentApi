using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Json.JsonNet;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.Json;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResult"/>
public class SafeMethodWithResult : SafeMethod, ISafeMethodWithResult
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

    public ISafeMethodWithResultAsGeneric<TResponse> WithJsonDeserialize<TResponse>()
        where TResponse : class
        => WithJsonDeserialize<TResponse>(JsonSettings.Default);

    public ISafeMethodWithResultAsGeneric<TResponse> WithJsonDeserialize<TResponse>(JsonSerializerSettings jsonSerializerSettings)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(jsonSerializerSettings);

        return WithGenericDeserialize(DeserializeStreamFunction);

        TResponse DeserializeStreamFunction(Stream stream)
            => stream.FromJson<TResponse>(jsonSerializerSettings);
    }

    public ISafeMethodWithResultAsGeneric<TResponse> WithJsonDeserialize<TResponse>(JsonSerializerOptions jsonSerializerOptions)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(jsonSerializerOptions);

        return WithGenericDeserialize(DeserializeStreamFunction);

        // TODO: Fix after adding task-based deserialization.
        TResponse DeserializeStreamFunction(Stream stream)
            => stream
                .FromJsonAsync<TResponse>(jsonSerializerOptions)
                .GetAwaiter()
                .GetResult();
    }

    public ISafeMethodWithResultAsGeneric<TResponse> WithXmlDeserialize<TResponse>()
        where TResponse : class
    {
        return WithGenericDeserialize(DeserializeStringFunction);

        static TResponse DeserializeStringFunction(string xml)
            => xml.FromXml<TResponse>();
    }

    public ISafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<string, TResponse> deserializeStringFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        return new SafeMethodWithResultAsGeneric<TResponse>(this, deserializeStringFunction);
    }

    public ISafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<Stream, TResponse> deserializeStreamFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStreamFunction);

        return new SafeMethodWithResultAsGeneric<TResponse>(this, deserializeStreamFunction);
    }
}
