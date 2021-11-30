using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Concrete;
using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.Models;
using CoreSharp.Models.Newtonsoft.Settings;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Extensions
{
    /// <summary>
    /// <see cref="IMethod"/> extensions.
    /// </summary>
    public static class IMethodExtensions
    {
        //Methods 
        /// <inheritdoc cref="System.Net.Http.HttpClient.SendAsync(HttpRequestMessage, CancellationToken)" />
        public static async Task<HttpResponseMessage> SendAsync(this IMethod method, CancellationToken cancellationToken = default)
        {
            _ = method ?? throw new ArgumentNullException(nameof(method));

            //Extract values 
            var httpClient = method.Resource.Request.HttpClient;
            var headers = method.Resource.Request.Headers;
            var throwOnError = method.Resource.Request.ThrowOnError;
            var route = method.Resource.Route;
            var httpMethod = method.HttpMethod;
            var completionOption = method.Resource.Request.CompletionOption;
            var queryParameters = (method as IQueryMethod)?.QueryParameters;
            var httpContent = (method as IContentMethod)?.Content;

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

        /// <inheritdoc cref="Json{TResponse}(IMethod, JsonSerializerSettings)" />
        public static IJsonResponse<TResponse> Json<TResponse>(this IMethod method)
            where TResponse : class
            => method.Json<TResponse>(DefaultJsonSettings.Instance);

        /// <inheritdoc cref="Json{TResponse}(IMethod, Func{Stream, TResponse})" />
        public static IJsonResponse<TResponse> Json<TResponse>(this IMethod method, JsonSerializerSettings jsonSerializerSettings)
            where TResponse : class
        {
            _ = jsonSerializerSettings ?? throw new ArgumentNullException(nameof(jsonSerializerSettings));

            TResponse DeserializeStreamFunction(Stream stream) => stream.ToEntity<TResponse>(jsonSerializerSettings);
            return method.Json(DeserializeStreamFunction);
        }

        /// <inheritdoc cref="Json{TResponse}(IMethod, Func{Stream, TResponse})" />
        public static IJsonResponse<TResponse> Json<TResponse>(this IMethod method, Func<string, TResponse> deserializeStreamFunction)
            where TResponse : class
        {
            _ = method ?? throw new ArgumentNullException(nameof(method));
            _ = deserializeStreamFunction ?? throw new ArgumentNullException(nameof(deserializeStreamFunction));

            return new JsonResponse<TResponse>(method, deserializeStreamFunction);
        }

        /// <summary>
        /// Treat <see cref="HttpResponseMessage.Content"/> as json
        /// and convert to strongly-typed object.
        /// </summary>
        public static IJsonResponse<TResponse> Json<TResponse>(this IMethod method, Func<Stream, TResponse> deserializeStringFunction)
            where TResponse : class
        {
            _ = method ?? throw new ArgumentNullException(nameof(method));
            _ = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

            return new JsonResponse<TResponse>(method, deserializeStringFunction);
        }
    }
}
