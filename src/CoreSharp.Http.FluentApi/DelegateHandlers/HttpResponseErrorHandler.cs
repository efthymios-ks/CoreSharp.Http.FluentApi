using CoreSharp.Http.FluentApi.Exceptions;
using CoreSharp.Http.FluentApi.Options;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.DelegateHandlers;

internal class HttpResponseErrorHandler : DelegatingHandler
{
    // Fields
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly HttpResponseErrorHandlerOptions _options;

    // Constructors
    public HttpResponseErrorHandler(IOptions<HttpResponseErrorHandlerOptions> options)
        => _options = options.Value;

    // Methods
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
            return response;

        // Allow rewind 
        await response.Content.LoadIntoBufferAsync();

        // Create exception 
        var exception = await HttpResponseException.CreateAsync(response);
        response.Dispose();

        // Handle exception 
        _options.HandleError(exception);

        // Return "204 NoContent"
        return new(HttpStatusCode.NoContent)
        {
            RequestMessage = request
        };
    }
}
