using CoreSharp.HttpClient.FluentApi.Concrete;
using CoreSharp.HttpClient.FluentApi.Contracts;
using System;
using Http = System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Extensions
{
    /// <summary>
    /// <see cref="HttpClient"/> extensions.
    /// </summary>
    public static class HttpClientExtensions
    {
        //Methods
        /// <summary>
        /// Start a chained configuration for a new http request.
        /// </summary>
        public static IRequest Request(this Http.HttpClient httpClient)
        {
            _ = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            return new Request(httpClient);
        }
    }
}
