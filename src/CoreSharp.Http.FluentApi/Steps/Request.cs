using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Utilities;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using static System.FormattableString;

namespace CoreSharp.Http.FluentApi.Steps;

/// <inheritdoc cref="IRequest"/>
internal class Request : IRequest
{
    // Constructors 
    public Request(HttpClient httpClient)
        => Me.HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    // Properties 
    private IRequest Me
        => this;

    HttpClient IRequest.HttpClient { get; set; }

    bool IRequest.ThrowOnError { get; set; } = true;

    HttpCompletionOption IRequest.CompletionOptionInternal { get; set; } = HttpCompletionOption.ResponseHeadersRead;

    IDictionary<string, string> IRequest.HeadersInternal { get; } = new Dictionary<string, string>();

    TimeSpan? IRequest.TimeoutInternal { get; set; }

    // Methods 
    public IRequest Headers(IDictionary<string, string> headers)
    {
        _ = headers ?? throw new ArgumentNullException(nameof(headers));

        foreach (var header in headers)
            Header(header.Key, header.Value);

        return this;
    }

    public IRequest Header(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        Me.HeadersInternal.AddOrUpdate(key, value);

        return this;
    }

    public IRequest Accept(string mediaTypeName)
        => Header(HeaderNames.Accept, mediaTypeName);

    public IRequest AcceptJson()
        => Accept(MediaTypeNames.Application.Json);

    public IRequest AcceptXml()
        => Accept(MediaTypeNames.Application.Xml);

    public IRequest Bearer(string accessToken)
         => Header(HeaderNames.Authorization, $"Bearer {accessToken}");

    public IRequest IgnoreError()
    {
        Me.ThrowOnError = false;
        return this;
    }

    public IRequest CompletionOption(HttpCompletionOption completionOption)
    {
        Me.CompletionOptionInternal = completionOption;
        return this;
    }

    public IRequest Timeout(TimeSpan timeout)
    {
        if (timeout.TotalMilliseconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(timeout), $"{nameof(timeout)} ({timeout.ToStringReadable()}) has to be positive and non-zero.");
        else if (timeout == System.Threading.Timeout.InfiniteTimeSpan)
            throw new ArgumentOutOfRangeException(nameof(timeout), $"{nameof(timeout)} cannot be {nameof(System.Threading.Timeout.InfiniteTimeSpan)}.");

        Me.TimeoutInternal = timeout;
        return this;
    }

    public IRoute Route(string resourceName, int key)
        => Route(Invariant($"{resourceName}/{key}"));

    public IRoute Route(string resourceName, long key)
        => Route(Invariant($"{resourceName}/{key}"));

    public IRoute Route(string resourceName, Guid key)
        => Route($"{resourceName}/{key}");

    public IRoute Route(string resourceName, string key)
        => Route($"{resourceName}/{key}");

    public IRoute Route(string resourceName)
    {
        if (string.IsNullOrWhiteSpace(resourceName))
            throw new ArgumentNullException(nameof(resourceName));

        // Fix resource name 
        resourceName = UriX.JoinSegments(resourceName).TrimStart('/');

        return new Route(this, resourceName);
    }

    public void Deconstruct(
        out HttpClient httpClient,
        out IDictionary<string, string> headers,
        out HttpCompletionOption httpCompletionOption,
        out TimeSpan timeout,
        out bool throwOnError)
    {
        httpClient = Me.HttpClient;
        headers = Me.HeadersInternal;
        httpCompletionOption = Me.CompletionOptionInternal;
        timeout = Me.TimeoutInternal ?? TimeSpan.Zero;
        throwOnError = Me.ThrowOnError;
    }
}
