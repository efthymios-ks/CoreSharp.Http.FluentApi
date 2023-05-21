using CoreSharp.Http.FluentApi.Steps.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Utilities;

/// <summary>
/// <see cref="ICacheQueryResponse{TResponse}"/> utilities.
/// </summary>
internal static class ICacheQueryResponseX
{
    // Methods 
    /// <summary>
    /// Return cached response.
    /// If not existing or timed-out,
    /// request new response and cache.
    /// </summary>
    public static ValueTask<TResponse> RequestWithCacheAsync<TResponse>(
        ICacheQueryResponse<TResponse> cacheQueryResponse,
        Task<TResponse> requestTask)
        where TResponse : class
    {
        _ = requestTask ?? throw new ArgumentNullException(nameof(requestTask));
        _ = cacheQueryResponse ?? throw new ArgumentNullException(nameof(cacheQueryResponse));

        return RequestWithCacheInternalAsync(cacheQueryResponse, requestTask);
    }

    private static async ValueTask<TResponse> RequestWithCacheInternalAsync<TResponse>(
        ICacheQueryResponse<TResponse> cacheQueryResponse,
        Task<TResponse> requestTask)
            where TResponse : class
    {
        // Prepare caching fields 
        var memoryCache = Settings.MemoryCache;
        var cacheDuration = cacheQueryResponse.Duration;
        var hasValidDuration = cacheDuration > TimeSpan.Zero;
        var cacheKey = hasValidDuration ? GenerateRequestHash(cacheQueryResponse) : null;
        var forceNewRequest = hasValidDuration && await cacheQueryResponse.ForceNewRequestConditionFactory();

        // Return cached value, if applicable 
        if (!forceNewRequest && hasValidDuration
            && memoryCache.TryGetValue<TResponse>(cacheKey, out var cachedValue))
        {
            return cachedValue;
        }

        // Else request... 
        var response = await requestTask;

        // ...and cache response, if needed 
        if (hasValidDuration)
        {
            memoryCache.Set(cacheKey, response, cacheDuration);
        }

        return response;
    }

    private static string GenerateRequestHash<TResponse>(ICacheQueryResponse<TResponse> cacheQueryResponse)
        where TResponse : class
    {
        const string separator = ", ";
        var builder = new StringBuilder();
        var method = cacheQueryResponse.Method;
        var queryParameters = (method as IQueryMethod)?.QueryParameters;

        // Base route 
        builder.Append(method.Route.Route);

        // Query parameters 
        if (queryParameters?.Count is > 0)
        {
            foreach (var queryParameter in queryParameters)
            {
                builder.Append(separator).Append($"{queryParameter.Key}={queryParameter.Value}");
            }
        }

        // Response type 
        builder.Append(separator)
               .Append(typeof(TResponse).FullName);

        return builder.ToString();
    }
}
