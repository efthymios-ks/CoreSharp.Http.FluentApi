using CoreSharp.HttpClient.FluentApi.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    internal class Request : IRequest
    {
        //Constructors 
        public Request(System.Net.Http.HttpClient httpClient)
        {
            _ = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            if (this is IRequest request)
                request.HttpClient = httpClient;
        }

        //Properties 
        System.Net.Http.HttpClient IRequest.HttpClient { get; set; }
        HttpCompletionOption IRequest.CompletionOption { get; set; } = HttpCompletionOption.ResponseHeadersRead;
        IDictionary<string, string> IRequest.Headers { get; } = new Dictionary<string, string>();
        bool IRequest.ThrowOnError { get; set; } = true;
    }
}
