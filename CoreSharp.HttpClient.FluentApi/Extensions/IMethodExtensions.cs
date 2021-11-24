using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Concrete;
using CoreSharp.HttpClient.FluentApi.Contracts;
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
            var queryParameter = (method as IQueryMethod)?.Query;
            var httpContent = (method as IContentMethod)?.Content;

            //Add query parameter
            if (httpMethod == HttpMethod.Get && !string.IsNullOrWhiteSpace(queryParameter))
                route += queryParameter;

            //Create request 
            using var request = new HttpRequestMessage(httpMethod, route)
            {
                Content = httpContent
            };
            foreach (var header in headers)
                request.Headers.Add(header.Key, header.Value);

            //Send request 
            var response = await httpClient.SendAsync(request, completionOption, cancellationToken);

            //Check response status 
            if (throwOnError)
                await response.EnsureSuccessAsync();

            return response;
        }

        /// <inheritdoc cref="Json{TResponse}(IMethod, Func{Stream, TResponse})" />
        public static IJsonResponse<TResponse> Json<TResponse>(this IMethod method)
            where TResponse : class
            => method.Json<TResponse>(DefaultJsonSettings.Instance);

        /// <inheritdoc cref="Json{TResponse}(IMethod, Func{Stream, TResponse})" />
        public static IJsonResponse<TResponse> Json<TResponse>(this IMethod method, JsonSerializerSettings jsonSerializerSettings)
            where TResponse : class
        {
            _ = jsonSerializerSettings ?? throw new ArgumentNullException(nameof(jsonSerializerSettings));

            TResponse DeserializeFunction(Stream stream) => stream.ToEntity<TResponse>(jsonSerializerSettings);
            return method.Json(DeserializeFunction);
        }

        /// <summary>
        /// Treat <see cref="HttpResponseMessage.Content"/> as json
        /// and convert to strongly-typed object.
        /// </summary>
        public static IJsonResponse<TResponse> Json<TResponse>(this IMethod method, Func<Stream, TResponse> deserializeFunction)
            where TResponse : class
        {
            _ = method ?? throw new ArgumentNullException(nameof(method));
            _ = deserializeFunction ?? throw new ArgumentNullException(nameof(deserializeFunction));

            return new JsonResponse<TResponse>(method, deserializeFunction);
        }
    }
}
