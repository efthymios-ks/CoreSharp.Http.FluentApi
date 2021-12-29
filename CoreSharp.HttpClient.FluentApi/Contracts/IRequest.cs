﻿using CoreSharp.Models.Exceptions;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IRequest
    {
        //Properties
        /// <inheritdoc cref="System.Net.Http.HttpClient" />
        internal System.Net.Http.HttpClient HttpClient { get; set; }

        /// <inheritdoc cref="HttpRequestMessage.Headers" />
        internal IDictionary<string, string> HeadersInternal { get; }

        /// <inheritdoc cref="HttpCompletionOption"/>
        internal HttpCompletionOption CompletionOptionInternal { get; set; }

        internal TimeSpan? TimeoutInternal { get; set; }

        /// <summary>
        /// Whether the interface should throw an <see cref="HttpResponseException"/>
        /// or not on unsuccessful requests using <see cref="HttpResponseMessage.StatusCode"/>.
        /// </summary>
        internal bool ThrowOnError { get; set; }

        //Methods 
        /// <inheritdoc cref="Header(string, string)" />
        IRequest Headers(IDictionary<string, string> headers);

        /// <summary>
        /// Add specified key-value header
        /// to outgoing <see cref="HttpRequestMessage"/>.
        /// </summary>
        IRequest Header(string key, string value);

        /// <summary>
        /// Set the <see cref="HeaderNames.Accept"/> header.
        /// </summary>
        IRequest Accept(string mediaTypeName);

        /// <summary>
        /// Set the <see cref="HeaderNames.Accept"/> header
        /// to <see cref="MediaTypeNames.Application.Json"/>.
        /// </summary>
        IRequest AcceptJson();

        /// <summary>
        /// Set the <see cref="HeaderNames.Accept"/> header
        /// to <see cref="MediaTypeNames.Application.Xml"/>.
        /// </summary>
        IRequest AcceptXml();

        /// <summary>
        /// Set the <see cref="HeaderNames.Authorization"/> header
        /// to bearer given value.
        /// </summary>
        IRequest Bearer(string accessToken);

        /// <inheritdoc cref="ThrowOnError" />
        IRequest IgnoreError();

        /// <inheritdoc cref="CompletionOptionInternal" />
        IRequest CompletionOption(HttpCompletionOption completionOption);

        /// <summary>
        /// <see cref="TimeSpan"/> to wait before the
        /// <see cref="HttpRequestMessage"/> timeout.
        /// If <see cref="System.Net.Http.HttpClient.Timeout"/>
        /// is lower, then it has higher priority.
        /// </summary>
        IRequest Timeout(TimeSpan timeout);

        /// <inheritdoc cref="Route(string)" />
        IRoute Route(string resourceName, int key);

        /// <inheritdoc cref="Route(string)" />
        IRoute Route(string resourceName, long key);

        /// <inheritdoc cref="Route(string)" />
        IRoute Route(string resourceName, Guid key);

        /// <inheritdoc cref="Route(string)" />
        IRoute Route(string resourceName, string key);

        /// <summary>
        /// Set <see cref="HttpRequestMessage.RequestUri"/>.
        /// </summary>
        IRoute Route(string resourceName);

        public void Deconstruct(
            out System.Net.Http.HttpClient httpClient,
            out IDictionary<string, string> headers,
            out HttpCompletionOption httpCompletionOption,
            out TimeSpan timeout,
            out bool throwOnError);
    }
}
