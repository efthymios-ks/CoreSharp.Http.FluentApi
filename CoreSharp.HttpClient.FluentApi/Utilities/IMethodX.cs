using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.HttpClient.FluentApi.Extensions;
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
        /// <summary>
        /// Send an HTTP request as an
        /// asynchronous operation.
        /// </summary>
        public static async Task<HttpResponseMessage> SendAsync(
            IMethod method,
            IDictionary<string, object> queryParameters = null,
            HttpContent httpContent = null,
            CancellationToken cancellationToken = default)
        {
            //Extract values 
            var (httpClient, headers, httpCompletionMode, timeout, throwOnError) = method.Route.Request;
            var route = method.Route.Route;
            var httpMethod = method.HttpMethod;

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
            try
            {
                var response = await httpClient.SendAsync(request, httpCompletionMode, timeout, cancellationToken);
                await response.EnsureSuccessAsync();
                return response;
            }
            //Throw if needed
            catch when (throwOnError)
            {
                throw;
            }
            //Or "swallow" 
            catch
            {
            }

            return null;
        }
    }
}
