using CoreSharp.Http.FluentApi.Steps.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps;

/// <inheritdoc cref="IBytesResponse" />
internal class BytesResponse : Response, IBytesResponse
{
    // Constructors
    public BytesResponse(IMethod method)
        : base(method)
    {
    }

    // Methods
    async Task<byte[]> IBytesResponse.SendAsync(CancellationToken cancellationtoken)
    {
        using var response = await SendAsync(cancellationtoken);
        if (response is null)
            return null;

        return await response.Content.ReadAsByteArrayAsync(cancellationtoken);
    }
}
