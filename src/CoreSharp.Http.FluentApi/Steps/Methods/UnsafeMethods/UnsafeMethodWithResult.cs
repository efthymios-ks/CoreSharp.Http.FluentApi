using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Json.JsonNet;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.Json;

namespace CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;

/// <inheritdoc cref="IUnsafeMethodWithResult"/>
public class UnsafeMethodWithResult : UnsafeMethod, IUnsafeMethodWithResult
{
    // Constructors 
    public UnsafeMethodWithResult(IUnsafeMethod method)
        : base(method)
    {
    }

    // Methods 
    public IUnsafeMethodWithResultAsBytes ToBytes()
        => new UnsafeMethodWithResultAsBytes(this);

    public IUnsafeMethodWithResultAsStream ToStream()
        => new UnsafeMethodWithResultAsStream(this);

    public new IUnsafeMethodWithResultAsString ToString()
        => new UnsafeMethodWithResultAsString(this);

    public IUnsafeMethodWithResultAsGeneric<TResponse> WithJsonDeserialize<TResponse>()
        where TResponse : class
        => WithJsonDeserialize<TResponse>(JsonSettings.Default);

    public IUnsafeMethodWithResultAsGeneric<TResponse> WithJsonDeserialize<TResponse>(JsonSerializerSettings jsonSerializerSettings)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(jsonSerializerSettings);

        return WithGenericDeserialize(DeserializeStreamFunction);

        TResponse DeserializeStreamFunction(Stream stream)
            => stream.FromJson<TResponse>(jsonSerializerSettings);
    }

    public IUnsafeMethodWithResultAsGeneric<TResponse> WithJsonDeserialize<TResponse>(JsonSerializerOptions jsonSerializerOptions)
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

    public IUnsafeMethodWithResultAsGeneric<TResponse> WithXmlDeserialize<TResponse>()
        where TResponse : class
    {
        return WithGenericDeserialize(DeserializeStringFunction);

        static TResponse DeserializeStringFunction(string xml)
            => xml.FromXml<TResponse>();
    }

    public IUnsafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<string, TResponse> deserializeStringFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        return new UnsafeMethodWithResultAsGeneric<TResponse>(this, deserializeStringFunction);
    }

    public IUnsafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<Stream, TResponse> deserializeStreamFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStreamFunction);

        return new UnsafeMethodWithResultAsGeneric<TResponse>(this, deserializeStreamFunction);
    }
}
