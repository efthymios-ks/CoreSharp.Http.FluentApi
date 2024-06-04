using App.Constants;
using Microsoft.Extensions.DependencyInjection;

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

        _ = services.AddHttpClient("Default", http => http.BaseAddress = new Uri(Endpoints.EndpointUrl));

        return services.BuildServiceProvider();
    }
}
