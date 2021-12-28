using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Utilities
{
    /// <summary>
    /// <see cref="IMethod"/> utilities.
    /// </summary>
    internal static class IMethodX
    {
        /// <inheritdoc cref="IMethod.SendAsync(CancellationToken)" />
        internal static async Task<HttpResponseMessage> SendAsync(IMethod method,
                                                                  IDictionary<string, object> queryParameters = null,
                                                                  HttpContent httpContent = null,
                                                                  CancellationToken cancellationToken = default)
        {
            //Extract values 
            var httpClient = method.Route.Request.HttpClient;
            var headers = method.Route.Request.HeadersInternal;
            var throwOnError = method.Route.Request.ThrowOnError;
            var route = method.Route.Route;
            var httpMethod = method.HttpMethod;
            var httpCompletionMode = method.Route.Request.CompletionOptionInternal;
            var timeout = method.Route.Request.TimeoutInternal ?? TimeSpan.Zero;

            //Add query parameter
            if (httpMethod == HttpMethod.Get && queryParameters.Count > 0)
            {
                var queryBuilder = new UrlQueryBuilder
                {
                    queryParameters
                };
                var queryParameter = queryBuilder.ToString();
                route += queryParameter;
            }

            //Create request 
            using var request = new HttpRequestMessage(httpMethod, route)
            {
                Content = httpContent
            };
            foreach (var (key, value) in headers)
            {
                if (request.Headers.Contains(key))
                    request.Headers.Remove(key);
                request.Headers.Add(key, value);
            }

            //Send request 
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.SendAsync(request, httpCompletionMode, timeout, cancellationToken);
            }
            catch
            {
#pragma warning disable RCS1236 // Use exception filter.
                if (throwOnError)
#pragma warning restore RCS1236 // Use exception filter.
                    throw;
            }

            //Check response status 
            if (response is not null && throwOnError)
                await response.EnsureSuccessAsync();

            return response;
        }
    }
}
