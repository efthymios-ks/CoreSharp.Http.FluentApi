using CoreSharp.HttpClient.FluentApi.Exceptions;
using System;

namespace CoreSharp.HttpClient.FluentApi.Options
{
    public class HttpResponseErrorHandlerOptions
    {
        //Properties 
        public Action<HttpResponseException> HandleError { get; set; }
    }
}
