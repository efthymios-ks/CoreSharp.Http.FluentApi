using CoreSharp.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IRequest
    {
        //Properties
        /// <inheritdoc cref="System.Net.Http.HttpClient" />
        internal System.Net.Http.HttpClient HttpClient { get; set; }

        /// <inheritdoc cref="HttpCompletionOption"/>
        internal HttpCompletionOption CompletionOptionInternal { get; set; }

        /// <inheritdoc cref="HttpRequestMessage.Headers" />
        internal IDictionary<string, string> HeadersInternal { get; }

        /// <summary>
        /// Whether the interface throws an <see cref="HttpResponseException"/>
        /// or not on not successful requests using <see cref="HttpResponseMessage.StatusCode"/>.
        /// </summary>
        internal bool ThrowOnError { get; set; }

        //Methods
        /// <inheritdoc cref="Header(string, string)" />
        IRequest Headers(IDictionary<string, string> headers);

        /// <inheritdoc cref="HeadersInternal" />
        IRequest Header(string key, string value);

        /// <inheritdoc cref="HttpRequestHeader.Accept" />
        IRequest Accept(string mediaType);

        /// <inheritdoc cref="HttpRequestHeader.Accept" />
        IRequest AcceptJson();

        /// <inheritdoc cref="HttpRequestHeader.Accept" />
        IRequest AcceptXml();

        /// <inheritdoc cref="HttpRequestHeader.Authorization" />
        IRequest Authorization(string accessToken);

        /// <inheritdoc cref="ThrowOnError" />
        IRequest IgnoreError();

        /// <inheritdoc cref="CompletionOptionInternal" />
        IRequest CompletionOption(HttpCompletionOption completionOption);

        /// <inheritdoc cref="Route(string)" />
        IRoute Route(string resourceName, int key);

        /// <inheritdoc cref="Route(string)" />
        IRoute Route(string resourceName, long key);

        /// <inheritdoc cref="Route(string)" />
        IRoute Route(string resourceName, Guid key);

        /// <inheritdoc cref="Route(string)" />
        IRoute Route(string resourceName, string key);

        /// <summary>
        /// Add resource route to request.
        /// </summary>
        IRoute Route(string resourceName);
    }
}
