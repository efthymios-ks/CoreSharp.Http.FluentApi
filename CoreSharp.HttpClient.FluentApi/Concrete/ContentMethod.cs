using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Contracts;
using System;
using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    internal class ContentMethod : Method, IContentMethod
    {
        //Constructors 
        public ContentMethod(IRoute route, HttpMethod httpMethod)
            : base(route, httpMethod)
        {
            var validMethods = new[] { HttpMethod.Post, HttpMethod.Put, HttpMethod.Patch };
            if (!httpMethod.IsIn(validMethods))
            {
                var validMethodsAsString = validMethods.StringJoin(", ");
                var message = $"{nameof(httpMethod)} must be one of the following: {validMethodsAsString}.";
                throw new ArgumentException(message, nameof(httpMethod));
            }
        }

        //Properties  
        HttpContent IContentMethod.Content { get; set; }
    }
}
