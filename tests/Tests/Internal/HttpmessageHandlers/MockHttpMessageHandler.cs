using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Internal.HttpmessageHandlers;

public sealed class MockHttpMessageHandler : HttpMessageHandler
{
    // Properties
    public HttpRequestMessage CapturedRequest { get; private set; }
    public HttpStatusCode ResponseStatus { get; set; }
    public string ResponseContent { get; set; }

    // Methods
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        CapturedRequest = request;

        cancellationToken.ThrowIfCancellationRequested();

        HttpContent content = null;
        if (!string.IsNullOrWhiteSpace(ResponseContent))
        {
            content = new StringContent(ResponseContent);
        }

        return Task.FromResult(new HttpResponseMessage(ResponseStatus)
        {
            Content = content,
            RequestMessage = request,
        });
    }
}