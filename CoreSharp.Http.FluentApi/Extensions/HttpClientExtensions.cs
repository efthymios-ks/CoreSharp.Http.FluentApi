using CoreSharp.Http.FluentApi.Services;
using CoreSharp.Http.FluentApi.Steps;
using CoreSharp.Http.FluentApi.Steps.Interfaces;

namespace CoreSharp.Http.FluentApi.Extensions;

/// <summary>
/// <see cref="HttpClient"/> extensions.
/// </summary>
public static class HttpClientExtensions
{
    // Methods
    /// <summary>
    /// Start a chained configuration for a new http request.
    /// </summary>
    public static IRequest Request(this HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        return new Request(httpClient, CacheStorage.Instance);
    }
}
