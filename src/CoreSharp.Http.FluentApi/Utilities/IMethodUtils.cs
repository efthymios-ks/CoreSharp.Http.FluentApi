using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using CoreSharp.Utilities;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Utilities;

internal static class IMethodUtils
{
    public static async Task<HttpResponseMessage> SendAsync(
        IMethod method,
        HttpContent httpContent,
        CancellationToken cancellationToken)
    {
        // Extract values
        var httpMethod = method.HttpMethod;

        var endpointObject = method.Endpoint;
        var endpoint = endpointObject.Endpoint;

        var requestObject = endpointObject.Request;
        var httpClient = requestObject.HttpClient;
        var queryParameters = requestObject.QueryParameters;
        var headers = requestObject.Headers;
        var httpCompletionOption = requestObject.HttpCompletionOption;
        var timeout = requestObject.Timeout ?? Timeout.InfiniteTimeSpan;
        var throwOnError = requestObject.ThrowOnError;

        // Add query parameter
        if (queryParameters.Count > 0)
        {
            endpoint = UriX.Build(endpoint, queryParameters);
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
