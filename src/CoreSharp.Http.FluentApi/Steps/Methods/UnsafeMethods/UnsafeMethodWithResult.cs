using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Json.JsonNet;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.Json;

namespace CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;

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

    IUnsafeMethodWithResultAsString IUnsafeMethodWithResult.ToString()
        => new UnsafeMethodWithResultAsString(this);

    public IUnsafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>()
        where TResponse : class
        => WithJsonDeserialize<TResponse>(JsonSettings.Default);

    public IUnsafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(JsonSerializerSettings jsonSerializerSettings)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(jsonSerializerSettings);

        return WithJsonDeserialize(DeserializeStreamFunction);

        TResponse DeserializeStreamFunction(Stream stream)
            => stream.FromJson<TResponse>(jsonSerializerSettings);
    }

    public IUnsafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(JsonSerializerOptions jsonSerializerOptions)
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

    public IUnsafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(Func<string, TResponse> deserializeStringFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        return new UnsafeMethodWithResultFromJson<TResponse>(this, deserializeStringFunction);
    }

    public IUnsafeMethodWithResultFromJson<TResponse> WithJsonDeserialize<TResponse>(Func<Stream, TResponse> deserializeStringFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        return new UnsafeMethodWithResultFromJson<TResponse>(this, deserializeStringFunction);
    }

    public IUnsafeMethodWithResultFromXml<TResponse> WithXmlDeserialize<TResponse>()
        where TResponse : class
    {
        return WithXmlDeserialize(DeserializeStringFunction);

        static TResponse DeserializeStringFunction(string xml)
            => xml.FromXml<TResponse>();
    }

    public IUnsafeMethodWithResultFromXml<TResponse> WithXmlDeserialize<TResponse>(Func<string, TResponse> deserializeStringFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        return new UnsafeMethodWithResultFromXml<TResponse>(this, deserializeStringFunction);
    }

    public IUnsafeMethodWithResultFromXml<TResponse> WithXmlDeserialize<TResponse>(Func<Stream, TResponse> deserializeStreamFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStreamFunction);

        return new UnsafeMethodWithResultFromXml<TResponse>(this, deserializeStreamFunction);
    }
}
