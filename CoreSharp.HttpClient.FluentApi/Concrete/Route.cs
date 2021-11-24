using CoreSharp.HttpClient.FluentApi.Contracts;
using System;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    internal class Route : IRoute
    {
        //Constructors  
        public Route(IRequest request, string resourceName)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentNullException(nameof(resourceName));

            if (this is IRoute route)
            {
                route.Request = request;
                route.Route = resourceName;
            }
        }

        //Properties 
        IRequest IRoute.Request { get; set; }
        string IRoute.Route { get; set; }
    }
}
