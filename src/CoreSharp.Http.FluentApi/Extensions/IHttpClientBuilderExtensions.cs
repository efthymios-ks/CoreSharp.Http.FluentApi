using CoreSharp.Http.FluentApi.DelegateHandlers;
using CoreSharp.Http.FluentApi.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CoreSharp.Http.FluentApi.Extensions;

/// <summary>
/// <see cref="IHttpClientBuilder"/> extensions.
/// </summary>
public static class IHttpClientBuilderExtensions
{
    public static IHttpClientBuilder AddHttpResponseErrorHandler(
        this IHttpClientBuilder httpClientBuilder,
        Action<HttpResponseErrorHandlerOptions> configure)
    {
        _ = httpClientBuilder ?? throw new ArgumentNullException(nameof(httpClientBuilder));
        _ = configure ?? throw new ArgumentNullException(nameof(configure));

        var services = httpClientBuilder.Services;
        if (!services.Any(service => service.ServiceType == typeof(HttpResponseErrorHandler)))
        {
            services.AddScoped<HttpResponseErrorHandler>();
            services.Configure(configure);
            httpClientBuilder.AddHttpMessageHandler<HttpResponseErrorHandler>();
        }

        return httpClientBuilder;
    }
}
