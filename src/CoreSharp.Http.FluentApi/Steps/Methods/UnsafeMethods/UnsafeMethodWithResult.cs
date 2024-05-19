using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Json.JsonNet;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

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

        return WithGenericDeserialize(DeserializeFunction);

        Task<TResponse> DeserializeFunction(Stream stream)
             => stream.FromJsonAsync<TResponse>(jsonSerializerOptions);
    }

    public IUnsafeMethodWithResultAsGeneric<TResponse> WithXmlDeserialize<TResponse>()
        where TResponse : class
    {
        return WithGenericDeserialize(DeserializeFunction);

        static TResponse DeserializeFunction(string xml)
            => xml.FromXml<TResponse>();
    }

    public IUnsafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<string, TResponse> deserializeFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeFunction);

        return WithGenericDeserialize(DeserializeTaskFunction);

        async Task<TResponse> DeserializeTaskFunction(Stream response)
        {
            var responseAsString = await response.ToStringAsync();
            return deserializeFunction(responseAsString);
        }
    }

    public IUnsafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<Stream, TResponse> deserializeFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeFunction);

        return WithGenericDeserialize(DeserializeTaskFunction);

        Task<TResponse> DeserializeTaskFunction(Stream response)
        {
            var deserializedResponse = deserializeFunction(response);
            return Task.FromResult(deserializedResponse);
        }
    }

    public IUnsafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<string, Task<TResponse>> deserializeFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeFunction);

        return WithGenericDeserialize(DeserializeTaskFunction);

        async Task<TResponse> DeserializeTaskFunction(Stream response)
        {
            var responseAsString = await response.ToStringAsync();
            return await deserializeFunction(responseAsString);
        }
    }

    public IUnsafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<Stream, Task<TResponse>> deserializeFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeFunction);

        return new UnsafeMethodWithResultAsGeneric<TResponse>(this, deserializeFunction);
    }
}
