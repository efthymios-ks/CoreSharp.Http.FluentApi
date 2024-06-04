using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Json.JsonNet;
using Newtonsoft.Json;
using System.Text.Json;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResult"/>
public class SafeMethodWithResult : SafeMethod, ISafeMethodWithResult
{
    // Constructors
    internal SafeMethodWithResult(ISafeMethod? safeMethod)
        : base(safeMethod)
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

        return WithGenericDeserialize(DeserializeFunction);

        TResponse DeserializeFunction(Stream response)
            => response.FromJson<TResponse>(jsonSerializerSettings);
    }

    public ISafeMethodWithResultAsGeneric<TResponse> WithJsonDeserialize<TResponse>(JsonSerializerOptions jsonSerializerOptions)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(jsonSerializerOptions);

        return WithGenericDeserialize(DeserializeFunction);

        Task<TResponse?> DeserializeFunction(Stream response)
            => response.FromJsonAsync<TResponse?>(jsonSerializerOptions);
    }

    public ISafeMethodWithResultAsGeneric<TResponse> WithXmlDeserialize<TResponse>()
        where TResponse : class
    {
        return WithGenericDeserialize(DeserializeFunction);

        static TResponse DeserializeFunction(string xml)
            => xml.FromXml<TResponse>();
    }

    public ISafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<string, TResponse?> deserializeFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeFunction);

        return WithGenericDeserialize(DeserializeTaskFunction);

        async Task<TResponse?> DeserializeTaskFunction(Stream response)
        {
            var responseAsString = await response.ToStringAsync();
            return deserializeFunction(responseAsString);
        }
    }

    public ISafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<Stream, TResponse?> deserializeFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeFunction);

        return WithGenericDeserialize(DeserializeTaskFunction);

        Task<TResponse?> DeserializeTaskFunction(Stream response)
        {
            var deserializedResponse = deserializeFunction(response);
            return Task.FromResult(deserializedResponse);
        }
    }

    public ISafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<string, Task<TResponse?>> deserializeFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeFunction);

        return WithGenericDeserialize(DeserializeTaskFunction);

        async Task<TResponse?> DeserializeTaskFunction(Stream response)
        {
            var responseAsString = await response.ToStringAsync();
            return await deserializeFunction(responseAsString);
        }
    }

    public ISafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<Stream, Task<TResponse?>> deserializeFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeFunction);

        return new SafeMethodWithResultAsGeneric<TResponse>(this, deserializeFunction);
    }
}
