using CoreSharp.Models.Exceptions;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.DelegateHandlers
{
    internal class HttpResponseErrorHandler : DelegatingHandler
    {
        //Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly HttpResponseErrorHandlerOptions _options;

        //Constructors
        public HttpResponseErrorHandler(IOptions<HttpResponseErrorHandlerOptions> options)
            => _options = options.Value;

        //Methods
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                //To allow rewind 
                await response.Content.LoadIntoBufferAsync();
                var exception = await HttpResponseException.CreateAsync(response);
                if (_options.HandleError is not null)
                    _options.HandleError(exception);
                if (_options.RethrowError)
                    throw exception;
            }

            return response;
        }
    }
}
