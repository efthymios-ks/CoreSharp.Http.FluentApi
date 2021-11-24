using CoreSharp.HttpClient.FluentApi.Contracts;
using System;

namespace CoreSharp.HttpClient.FluentApi.Extensions
{
    /// <summary>
    /// <see cref="ICacheQueryResponse{TResponse}"/> extensions.
    /// </summary>
    public static class ICacheQueryResponseExtensions
    {
        //Methods
        /// <summary>
        /// Enable in-memory, client-side response caching.
        /// </summary>
        public static ICacheQueryResponse<TResponse> Cache<TResponse>(this ICacheQueryResponse<TResponse> cacheQueryResponse, TimeSpan duration)
            where TResponse : class
        {
            _ = cacheQueryResponse ?? throw new ArgumentNullException(nameof(cacheQueryResponse));

            cacheQueryResponse.Duration = duration;

            return cacheQueryResponse;
        }
    }
}
