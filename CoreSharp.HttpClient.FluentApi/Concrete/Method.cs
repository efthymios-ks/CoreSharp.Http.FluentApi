using CoreSharp.HttpClient.FluentApi.Contracts;
using System;
using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    internal class Method : IMethod
    {
        //Constructors 
        public Method(IRoute route, HttpMethod httpMethod)
        {
            _ = route ?? throw new ArgumentNullException(nameof(route));
            _ = httpMethod ?? throw new ArgumentNullException(nameof(httpMethod));

            if (this is IMethod method)
            {
                method.Resource = route;
                method.HttpMethod = httpMethod;
            }
        }
        //Properties  
        IRoute IMethod.Resource { get; set; }
        HttpMethod IMethod.HttpMethod { get; set; }
    }
}
