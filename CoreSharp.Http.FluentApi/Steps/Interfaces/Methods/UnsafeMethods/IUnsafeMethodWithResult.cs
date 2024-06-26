﻿using JsonNet = Newtonsoft.Json;
using TextJson = System.Text.Json;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;

/// <inheritdoc cref="IUnsafeMethod"/>
public interface IUnsafeMethodWithResult : IUnsafeMethod
{
    // Methods
    IUnsafeMethodWithResultAsBytes ToBytes();
    IUnsafeMethodWithResultAsStream ToStream();
    IUnsafeMethodWithResultAsString ToString();

    /// <summary>
    /// Treat response as JSON and deserialize to provided type.
    /// </summary>
    IUnsafeMethodWithResultAsGeneric<TResponse> WithJsonDeserialize<TResponse>()
        where TResponse : class;

    /// <inheritdoc cref="WithJsonDeserialize{TResponse}()"/>
    IUnsafeMethodWithResultAsGeneric<TResponse> WithJsonDeserialize<TResponse>(JsonNet.JsonSerializerSettings jsonSerializerSettings)
      where TResponse : class;

    /// <inheritdoc cref="WithJsonDeserialize{TResponse}()"/>
    IUnsafeMethodWithResultAsGeneric<TResponse> WithJsonDeserialize<TResponse>(TextJson.JsonSerializerOptions jsonSerializerOptions)
      where TResponse : class;

    /// <summary>
    /// Treat response as XML and deserialize to provided type.
    /// </summary>
    IUnsafeMethodWithResultAsGeneric<TResponse> WithXmlDeserialize<TResponse>()
      where TResponse : class;

    /// <inheritdoc cref="WithGenericDeserialize{TResponse}(Func{string, Task{TResponse}})"/>
    IUnsafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<string, TResponse?> deserializeFunction)
        where TResponse : class;

    /// <inheritdoc cref="WithGenericDeserialize{TResponse}(Func{Stream, Task{TResponse}})"/>
    IUnsafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<Stream, TResponse?> deserializeFunction)
        where TResponse : class;

    /// <inheritdoc cref="WithGenericDeserialize{TResponse}(Func{Stream, Task{TResponse}})"/>
    IUnsafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<string, Task<TResponse?>> deserializeFunction)
        where TResponse : class;

    /// <summary>
    /// Deserialize to provided type.
    /// </summary>
    IUnsafeMethodWithResultAsGeneric<TResponse> WithGenericDeserialize<TResponse>(Func<Stream, Task<TResponse?>> deserializeFunction)
        where TResponse : class;
}
