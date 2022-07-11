using CoreSharp.Http.FluentApi.Contracts;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Utilities;

/// <summary>
/// <see cref="ICacheQueryResponse{TResponse}"/> utilities.
/// </summary>
internal static class ICacheQueryX
{
    //Methods 
    /// <summary>
    /// Return cached response.
    /// If not existing or timed-out,
    /// request new response and cache.
    /// </summary>
    public static ValueTask<TResponse> CachedRequestAsync<TResponse>(Task<TResponse> requestTask, string route, TimeSpan? cacheDuration)
        where TResponse : class
    {
        _ = requestTask ?? throw new ArgumentNullException(nameof(requestTask));
        if (string.IsNullOrWhiteSpace(route))
            throw new ArgumentNullException(nameof(route));

        return CachedRequestInternalAsync(requestTask, route, cacheDuration);
    }

    private static async ValueTask<TResponse> CachedRequestInternalAsync<TResponse>(Task<TResponse> requestTask, string route, TimeSpan? cacheDuration)
            where TResponse : class
    {
        //Prepare caching fields 
        var memoryCache = Settings.MemoryCache;
        var shouldCache = cacheDuration is not null && cacheDuration > TimeSpan.Zero;
        var cacheKey = shouldCache ? $"{route} > {typeof(TResponse).FullName}" : null;

        //Return cached value, if applicable 
        if (shouldCache && memoryCache.TryGetValue<TResponse>(cacheKey, out var cachedValue))
            return cachedValue;

        //Else request... 
        var response = await requestTask;

        //...and cache response, if needed 
        if (shouldCache)
            memoryCache.Set(cacheKey, response, cacheDuration.Value);

        return response;
    }
}
