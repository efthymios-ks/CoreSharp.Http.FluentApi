using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Contracts;
using System;
using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    internal class QueryMethod : Method, IQueryMethod
    {
        //Constructors 
        public QueryMethod(IRoute route, HttpMethod httpMethod)
            : base(route, httpMethod)
        {
            var validMethods = new[] { HttpMethod.Get };
            if (!httpMethod.IsIn(validMethods))
            {
                var validMethodsAsString = validMethods.StringJoin(", ");
                var message = $"{nameof(httpMethod)} must be one of the following: {validMethodsAsString}.";
                throw new ArgumentException(message, nameof(httpMethod));
            }
        }

        //Properties  
        string IQueryMethod.QueryParameter { get; set; }
    }
}
