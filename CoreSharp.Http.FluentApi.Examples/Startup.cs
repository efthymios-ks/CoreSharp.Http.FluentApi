using CoreSharp.Http.FluentApi.Domain.Constants;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CoreSharp.Http.FluentApi.Examples
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

            services.AddHttpClient("Default", http => http.BaseAddress = new Uri(Endpoints.EndpointUrl));

            return services.BuildServiceProvider();
        }
    }
}
