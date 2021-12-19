using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.Models;
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
            var completionOption = method.Route.Request.CompletionOptionInternal;

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
            var response = await httpClient.SendAsync(request, completionOption, cancellationToken);

            //Check response status 
            if (throwOnError)
                await response.EnsureSuccessAsync();

            return response;
        }
    }
}
