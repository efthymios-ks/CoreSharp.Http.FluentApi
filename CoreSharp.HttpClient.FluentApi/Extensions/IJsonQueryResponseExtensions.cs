using CoreSharp.HttpClient.FluentApi.Contracts;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Extensions
{
    /// <summary>
    /// <see cref="IJsonQueryResponse{TResponse}"/> extensions.
    /// </summary>
    public static class IJsonQueryResponseExtensions
    {
        //Methods
        /// <inheritdoc cref="ICacheQueryResponseExtensions.Cache{TResponse}(ICacheQueryResponse{TResponse}, TimeSpan)"/>
        public static IJsonQueryResponse<TResponse> Cache<TResponse>(this IJsonQueryResponse<TResponse> jsonQueryResponse, TimeSpan duration)
            where TResponse : class
        {
            _ = jsonQueryResponse ?? throw new ArgumentNullException(nameof(jsonQueryResponse));

            if (jsonQueryResponse is ICacheQueryResponse<TResponse> cacheQueryResponse)
                cacheQueryResponse.Cache(duration);

            return jsonQueryResponse;
        }

        /// <inheritdoc cref="IJsonResponseExtensions.SendAsync{TResponse}(IJsonResponse{TResponse}, CancellationToken)"/>
        public static async Task<TResponse> SendAsync<TResponse>(this IJsonQueryResponse<TResponse> jsonQueryResponse, CancellationToken cancellationToken = default)
            where TResponse : class
        {
            _ = jsonQueryResponse ?? throw new ArgumentNullException(nameof(jsonQueryResponse));

            //Extract args
            var route = jsonQueryResponse.Method.Resource.Route;
            var cacheDuration = jsonQueryResponse.Duration;

            //Prepare caching fields 
            var shouldCache = cacheDuration is not null && cacheDuration != TimeSpan.Zero;
            var cacheKey = shouldCache ? $"{route} > {typeof(TResponse).FullName}" : string.Empty;

            //Return cached value, if applicable 
            if (shouldCache && Options.MemoryCache.TryGetValue<TResponse>(cacheKey, out var cachedValue))
                return cachedValue;

            //Else request... 
            var response = await (jsonQueryResponse as IJsonResponse<TResponse>)!.SendAsync(cancellationToken);

            //...and cache response, if needed 
            if (shouldCache)
                Options.MemoryCache.Set(cacheKey, response, cacheDuration.Value);

            return response;
        }
    }
}
