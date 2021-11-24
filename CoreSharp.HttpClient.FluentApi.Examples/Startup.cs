using CoreSharp.HttpClient.FluentApi.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CoreSharp.HttpClient.FluentApi.Examples
{
    /// <summary>
    /// Pseudo-startup class.
    /// </summary>
    public static class Startup
    {
        //Methods
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddHttpClient("Default", http => http.BaseAddress = new Uri(Options.EndpointUrl));

            return services.BuildServiceProvider();
        }
    }
}
