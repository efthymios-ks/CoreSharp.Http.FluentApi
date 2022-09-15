using Domain.Constants;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace App;

/// <summary>
/// Pseudo-startup class.
/// </summary>
public static class Startup
{
    // Methods
    public static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddHttpClient("Default", http => http.BaseAddress = new Uri(Endpoints.EndpointUrl));

        return services.BuildServiceProvider();
    }
}
