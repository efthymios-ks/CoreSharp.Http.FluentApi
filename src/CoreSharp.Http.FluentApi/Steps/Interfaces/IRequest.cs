using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces;

public interface IRequest
{
    // Properties
    internal HttpClient HttpClient { get; set; }
    internal IDictionary<string, string> Headers { get; }
    internal IDictionary<string, string> QueryParameters { get; }
    internal HttpCompletionOption HttpCompletionOption { get; set; }
    internal TimeSpan? Timeout { get; set; }
    internal bool ThrowOnError { get; set; }

    // Methods 
    /// <inheritdoc cref="WithHeader(string, string)" />
    IRequest WithHeaders(IDictionary<string, string> headers);

    /// <summary>
    /// Add specified key-value header
    /// to outgoing <see cref="HttpRequestMessage"/>.
    /// </summary>
    IRequest WithHeader(string key, string value);

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

    IRequest WithAuthorization(string authorization);

    /// <summary>
    /// Set the <see cref="HeaderNames.Authorization"/> header
    /// to bearer given value.
    /// </summary>
    IRequest WithBearerToken(string accessToken);

    /// <summary>
    /// Add query parameters.
    /// </summary>
    IRequest WithQuery(IDictionary<string, object> parameters);

    /// <summary>
    /// Add properties of object as query parameters.
    /// </summary>
    IRequest WithQuery<TQueryParameter>(TQueryParameter queryParameter)
        where TQueryParameter : class;

    /// <summary>
    /// Add query parameter.
    /// </summary>
    IRequest WithQuery(string key, object value);

    /// <inheritdoc cref="ThrowOnError" />
    IRequest IgnoreError();

    /// <inheritdoc cref="HttpCompletionOption" />
    IRequest WithCompletionOption(HttpCompletionOption completionOption);

    /// <summary>
    /// <see cref="TimeSpan"/> to wait before the
    /// <see cref="HttpRequestMessage"/> timeout.
    /// If <see cref="HttpClient.Timeout"/>
    /// is lower, then it has higher priority.
    /// </summary>
    IRequest WithTimeout(TimeSpan timeout);

    /// <inheritdoc cref="WithEndpoint(string)" />
    IEndpoint WithEndpoint(string resourceName, int key);

    /// <inheritdoc cref="WithEndpoint(string)" />
    IEndpoint WithEndpoint(string resourceName, long key);

    /// <inheritdoc cref="WithEndpoint(string)" />
    IEndpoint WithEndpoint(string resourceName, Guid key);

    /// <inheritdoc cref="WithEndpoint(string)" />
    IEndpoint WithEndpoint(string resourceName, string key);

    /// <inheritdoc cref="WithEndpoint(string)" />
    IEndpoint WithEndpoint(IEnumerable<string> segments);

    /// <summary>
    /// Set <see cref="HttpRequestMessage.RequestUri"/>.
    /// </summary>
    IEndpoint WithEndpoint(string resourceName);
}