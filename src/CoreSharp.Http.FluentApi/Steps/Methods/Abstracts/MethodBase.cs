using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using CoreSharp.Utilities;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.Abstracts;

/// <inheritdoc cref="IMethod"/>
public abstract class MethodBase : IMethod
{
    // Constructors 
    protected MethodBase(IEndpoint endpoint, HttpMethod httpMethod)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        ArgumentNullException.ThrowIfNull(httpMethod);

        Me.Endpoint = endpoint;
        Me.HttpMethod = httpMethod;
    }

    // Properties 
    private IMethod Me
        => this;
    IEndpoint IMethod.Endpoint { get; set; }
    HttpMethod IMethod.HttpMethod { get; set; }

    // Methods
    public virtual Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default)
        => SendAsync(httpContent: null, cancellationToken: cancellationToken);

    protected async Task<HttpResponseMessage> SendAsync(HttpContent httpContent, CancellationToken cancellationToken = default)
    {
        // Extract values
        var httpMethod = Me.HttpMethod;

        var endpointInterface = Me.Endpoint;
        var endpoint = endpointInterface.Endpoint;

        var requestInterface = endpointInterface.Request;
        var httpClient = requestInterface.HttpClient;
        var queryParameters = requestInterface.QueryParameters;
        var headers = requestInterface.Headers;
        var httpCompletionOption = requestInterface.HttpCompletionOption;
        var timeout = requestInterface.Timeout ?? Timeout.InfiniteTimeSpan;
        var throwOnError = requestInterface.ThrowOnError;

        // Add query parameter
        if (queryParameters.Count > 0)
        {
            endpoint = UriUtils.Build(endpoint, queryParameters);
        }

        // Create request 
        using var request = new HttpRequestMessage(httpMethod, endpoint)
        {
            Content = httpContent
        };

        foreach (var (key, value) in headers)
        {
            request.Headers.Remove(key);
            request.Headers.Add(key, value);
        }

        // Send request
        try
        {
            var response = await httpClient.SendAsync(request, httpCompletionOption, timeout, cancellationToken);
            await response.EnsureSuccessAsync();
            return response;
        }
        catch when (throwOnError)
        {
            // Throw if needed
            throw;
        }
        catch
        {
            // Or ignore 
        }

        return null;
    }
}
