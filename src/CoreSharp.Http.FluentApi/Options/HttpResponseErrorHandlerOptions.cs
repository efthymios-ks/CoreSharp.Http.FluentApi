using CoreSharp.Http.FluentApi.Exceptions;
using System;

namespace CoreSharp.Http.FluentApi.Options;

public class HttpResponseErrorHandlerOptions
{
    // Properties 
    public Action<HttpResponseException> HandleError { get; set; }
}
