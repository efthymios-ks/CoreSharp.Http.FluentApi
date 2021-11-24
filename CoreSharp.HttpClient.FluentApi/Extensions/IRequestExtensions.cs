using CoreSharp.HttpClient.FluentApi.Concrete;
using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Extensions
{
    /// <summary>
    /// <see cref="IRequest"/> extensions.
    /// </summary>
    public static class IRequestExtensions
    {
        //Methods
        /// <inheritdoc cref="Header(IRequest, string, string)" />
        public static IRequest Headers(this IRequest request, IDictionary<string, string> headers)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            _ = headers ?? throw new ArgumentNullException(nameof(headers));

            foreach (var header in headers)
                request.Header(header.Key, header.Value);

            return request;
        }

        /// <inheritdoc cref="IRequest.Headers" />
        public static IRequest Header(this IRequest request, string key, string value)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(nameof(key)))
                throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(nameof(value)))
                throw new ArgumentNullException(nameof(value));

            if (request.Headers.ContainsKey(key))
                request.Headers[key] = value;
            else
                request.Headers.Add(key, value);

            return request;
        }

        /// <inheritdoc cref="IRequest.ThrowOnError" />
        public static IRequest ThrowOnError(this IRequest request, bool throwOnError = true)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            request.ThrowOnError = throwOnError;

            return request;
        }

        /// <inheritdoc cref="IRequest.CompletionOption" />
        public static IRequest CompletionOption(this IRequest request, HttpCompletionOption completionOption)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            request.CompletionOption = completionOption;

            return request;
        }

        /// <inheritdoc cref="Route(IRequest, string)" />
        public static IRoute Route<TKey>(this IRequest request, string resourceName, TKey key)
            => request.Route($"{resourceName}/{key}");

        /// <summary>
        /// Add resource route to request.
        /// </summary>
        public static IRoute Route(this IRequest request, string resourceName)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentNullException(nameof(resourceName));

            //Fix resource name 
            resourceName = UriX.JoinSegments(resourceName).TrimStart('/');

            return new Route(request, resourceName);
        }
    }
}
