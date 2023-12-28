using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Utilities;

internal static class CachedRequestUtils
{
    // Fields
    private const char CacheKeySeparator = ',';
    private static readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

    // Methods
    public static async Task<TResult> GetOrAddResultAsync<TResult>(
        IMethod method,
        TimeSpan cacheDuration,
        Func<Task<bool>> cacheInvalidationFactory,
        Func<Task<TResult>> requestFactory)
    {
        // Prepare caching fields   
        var hasValidDuration = cacheDuration > TimeSpan.Zero;

        if (!hasValidDuration)
        {
            return await requestFactory();
        }

        var cacheKey = GenerateRequestHash<TResult>(method);
        var forceNewRequest = await cacheInvalidationFactory();

        // Return cached value, if applicable.
        if (!forceNewRequest && _memoryCache.TryGetValue<TResult>(cacheKey, out var cachedValue))
        {
            return cachedValue;
        }

        // Else request... 
        var response = await requestFactory();

        // ...and cache response, if needed.
        _memoryCache.Set(cacheKey, response, cacheDuration);

        return response;
    }

    private static string GenerateRequestHash<TResponse>(IMethod method)
    {
        var endpointObject = method.Endpoint;
        var requestObject = endpointObject.Request;
        var queryParameters = requestObject.QueryParameters;
        var endpoint = endpointObject.Endpoint;
        var builder = new StringBuilder();

        // Base route 
        builder.Append(endpoint);

        // Query parameters 
        if (queryParameters is { Count: > 0 })
        {
            foreach (var queryParameter in queryParameters)
            {
                builder
                    .Append(CacheKeySeparator)
                    .Append($"{queryParameter.Key}={queryParameter.Value}");
            }
        }

        // Response type 
        builder
            .Append(CacheKeySeparator)
            .Append(typeof(TResponse).FullName);

        return builder.ToString();
    }
}