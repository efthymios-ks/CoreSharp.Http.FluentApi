using CoreSharp.Http.FluentApi.Services.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace CoreSharp.Http.FluentApi.Services;

public sealed class CacheStorage(IMemoryCache memoryCache) : ICacheStorage
{
    // Fields
    private const char CacheKeySeparator = ':';
    private readonly IMemoryCache _memoryCache = memoryCache;
    private static ICacheStorage? _instance;

    // Properties
    public static ICacheStorage Instance
        => _instance ??= new CacheStorage(new MemoryCache(new MemoryCacheOptions()));

    // Methods
    public string GetCacheKey<TResponse>(IMethod method)
    {
        ArgumentNullException.ThrowIfNull(method);

        var endpointObject = method.Endpoint;
        var queryParameters = endpointObject!.QueryParameters;
        var requestObject = endpointObject.Request!;
        var headers = requestObject.Headers;
        var endpoint = endpointObject.Endpoint;
        var builder = new StringBuilder();

        // Base route 
        _ = builder.Append(endpoint);

        // Query parameters
        foreach (var (key, value) in queryParameters)
        {
            _ = builder
                .Append(CacheKeySeparator)
                .Append($"{key}={value}");
        }

        // Headers
        foreach (var (key, value) in headers)
        {
            _ = builder
                .Append(CacheKeySeparator)
                .Append($"{key}={value}");
        }

        // Response type 
        _ = builder
            .Append(CacheKeySeparator)
            .Append(typeof(TResponse).FullName);

        return builder.ToString();
    }

    public async Task<TResult> GetOrAddAsync<TResult>(
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
            return cachedResponse!;
        }

        response = await requestFactory();
        return _memoryCache.Set(cacheKey, response, cacheDuration);
    }
}
