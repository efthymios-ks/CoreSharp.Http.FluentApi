using CoreSharp.Http.FluentApi.Services.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Services;

public sealed class CacheStorage : ICacheStorage
{
    // Fields
    private const char CacheKeySeparator = ':';
    private readonly IMemoryCache _memoryCache;
    private static ICacheStorage _instance;

    // Constructors
    public CacheStorage(IMemoryCache memoryCache)
        => _memoryCache = memoryCache;

    // Properties
    public static ICacheStorage Instance
        => _instance ??= new CacheStorage(new MemoryCache(new MemoryCacheOptions()));

    // Methods
    public string GetCacheKey<TResponse>(IMethod method)
    {
        ArgumentNullException.ThrowIfNull(method);

        var endpointObject = method.Endpoint;
        var queryParameters = endpointObject.QueryParameters;
        var requestObject = endpointObject.Request;
        var headers = requestObject.Headers;
        var endpoint = endpointObject.Endpoint;
        var builder = new StringBuilder();

        // Base route 
        builder.Append(endpoint);

        // Query parameters
        foreach (var (key, value) in queryParameters)
        {
            builder
                .Append(CacheKeySeparator)
                .Append($"{key}={value}");
        }

        // Headers
        foreach (var (key, value) in headers)
        {
            builder
                .Append(CacheKeySeparator)
                .Append($"{key}={value}");
        }

        // Response type 
        builder
            .Append(CacheKeySeparator)
            .Append(typeof(TResponse).FullName);

        return builder.ToString();
    }

    public async Task<TResult> GetOrAddResultAsync<TResult>(
        IMethod method,
        TimeSpan cacheDuration,
        Func<Task<TResult>> requestFactory,
        Func<Task<bool>> cacheInvalidationFactory)
    {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentNullException.ThrowIfNull(cacheInvalidationFactory);
        ArgumentNullException.ThrowIfNull(requestFactory);

        var hasValidDuration = cacheDuration > TimeSpan.Zero;
        if (!hasValidDuration)
        {
            return await requestFactory();
        }

        TResult response;
        var cacheKey = GetCacheKey<TResult>(method);
        var shouldInvalidateCache = await cacheInvalidationFactory();
        if (shouldInvalidateCache)
        {
            response = await requestFactory();
            return _memoryCache.Set(cacheKey, response, cacheDuration);
        }

        if (_memoryCache.TryGetValue<TResult>(cacheKey, out var cachedResponse))
        {
            return cachedResponse;
        }

        response = await requestFactory();
        return _memoryCache.Set(cacheKey, response, cacheDuration);
    }
}