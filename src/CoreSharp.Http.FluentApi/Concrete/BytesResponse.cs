using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Concrete;

/// <inheritdoc cref="IBytesResponse" />
internal class BytesResponse : Response, IBytesResponse
{
    //Constructors
    public BytesResponse(IMethod method)
        : base(method)
    {
    }

    //Methods
    async Task<byte[]> IBytesResponse.SendAsync(CancellationToken cancellationtoken)
    {
        using var response = await SendAsync(cancellationtoken);
        return await (response?.Content.ReadAsByteArrayAsync(cancellationtoken)).OrDefault();
    }
}
