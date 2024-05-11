using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Services.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Utilities;
using CoreSharp.Utilities;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using static System.FormattableString;

namespace CoreSharp.Http.FluentApi.Steps;

/// <inheritdoc cref="IRequest"/>
public sealed class Request : IRequest
{
    // Constructors

    public Request(
        HttpClient httpClient,
        ICacheStorage cacheStorage,
        IHttpResponseMessageDeserializer httpResponseMessageDeserializer)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(cacheStorage);
        ArgumentNullException.ThrowIfNull(httpResponseMessageDeserializer);

        var me = Me;
        me.HttpClient = httpClient;
        me.CacheStorage = cacheStorage;
        me.HttpResponseMessageDeserializer = httpResponseMessageDeserializer;
    }

    // Properties 
    private IRequest Me
        => this;
    ICacheStorage IRequest.CacheStorage { get; set; }
    IHttpResponseMessageDeserializer IRequest.HttpResponseMessageDeserializer { get; set; }
    HttpClient IRequest.HttpClient { get; set; }
    IDictionary<string, string> IRequest.QueryParameters { get; }
        = new Dictionary<string, string>();
    IDictionary<string, string> IRequest.Headers { get; }
        = new Dictionary<string, string>();
    bool IRequest.ThrowOnError { get; set; } = true;
    HttpCompletionOption IRequest.HttpCompletionOption { get; set; }
        = HttpCompletionOption.ResponseHeadersRead;
    TimeSpan? IRequest.Timeout { get; set; }

    // Methods 
    public IRequest WithHeaders(IDictionary<string, string> headers)
    {
        ArgumentNullException.ThrowIfNull(headers);

        foreach (var header in headers)
        {
            Me.WithHeader(header.Key, header.Value);
        }

        return this;
    }

    public IRequest WithHeader(string key, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);

        Me.Headers.AddOrUpdate(key, value);

        return this;
    }

    public IRequest Accept(string mediaTypeName)
        => Me.WithHeader(HeaderNames.Accept, mediaTypeName);

    public IRequest AcceptJson()
        => Me.Accept(MediaTypeNames.Application.Json);

    public IRequest AcceptXml()
        => Me.Accept(MediaTypeNames.Application.Xml);

    public IRequest WithAuthorization(string authorization)
        => Me.WithHeader(HeaderNames.Authorization, authorization);

    public IRequest WithBearerToken(string accessToken)
        => Me.WithAuthorization($"Bearer {accessToken}");

    public IRequest WithQuery(IDictionary<string, object> parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        foreach (var (key, value) in parameters)
        {
            Me.WithQuery(key, value);
        }

        return this;
    }

    public IRequest WithQuery<TQueryParameter>(TQueryParameter queryParameter)
        where TQueryParameter : class
    {
        ArgumentNullException.ThrowIfNull(queryParameter);

        if (queryParameter is not IDictionary<string, object> parameters)
        {
            parameters = queryParameter.GetPropertiesDictionary();
        }

        return Me.WithQuery(parameters);
    }

    public IRequest WithQuery(string key, object value)
    {
        Me.QueryParameters[key] = value?.ToString();
        return this;
    }

    public IRequest IgnoreError()
    {
        Me.ThrowOnError = false;
        return this;
    }

    public IRequest WithCompletionOption(HttpCompletionOption completionOption)
    {
        Me.HttpCompletionOption = completionOption;
        return this;
    }

    public IRequest WithTimeout(TimeSpan timeout)
    {
        if (timeout.TotalMilliseconds <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout),
                $"{nameof(timeout)} ({timeout.ToStringReadable()}) has to be positive and non-zero.");
        }

        Me.Timeout = timeout;
        return this;
    }

    public IEndpoint WithEndpoint(string resourceName, int key)
        => Me.WithEndpoint(Invariant($"{resourceName}/{key}"));

    public IEndpoint WithEndpoint(string resourceName, long key)
        => Me.WithEndpoint(Invariant($"{resourceName}/{key}"));

    public IEndpoint WithEndpoint(string resourceName, Guid key)
        => Me.WithEndpoint($"{resourceName}/{key}");

    public IEndpoint WithEndpoint(string resourceName, string key)
        => Me.WithEndpoint($"{resourceName}/{key}");

    public IEndpoint WithEndpoint(IEnumerable<string> segments)
    {
        ArgumentNullException.ThrowIfNull(segments);

        return Me.WithEndpoint(UriUtils.JoinSegments(segments.ToArray()));
    }

    public IEndpoint WithEndpoint(string resourceName)
    {
        ArgumentException.ThrowIfNullOrEmpty(resourceName);

        // Fix resource name 
        resourceName = UriUtils.JoinSegments(resourceName).TrimStart('/');
        return new Endpoint(this, resourceName);
    }
}
