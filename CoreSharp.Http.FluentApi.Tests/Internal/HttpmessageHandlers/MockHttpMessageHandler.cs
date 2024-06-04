using System.Net;

namespace Tests.Internal.HttpmessageHandlers;

public sealed class MockHttpMessageHandler : HttpMessageHandler
{
    // Properties
    public HttpRequestMessage? CapturedRequest { get; private set; }
    public bool SetResponseToNull { get; set; }
    public HttpStatusCode ResponseStatus { get; set; }
    public string? ResponseContent { get; set; }

    // Methods
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        CapturedRequest = request;

        cancellationToken.ThrowIfCancellationRequested();

        if (SetResponseToNull)
        {
            return Task.FromResult<HttpResponseMessage>(null!);
        }

        HttpContent? content = null;
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
