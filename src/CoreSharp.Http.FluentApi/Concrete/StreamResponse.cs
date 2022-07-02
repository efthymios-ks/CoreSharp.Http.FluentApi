using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Contracts;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Concrete;

/// <inheritdoc cref="IStreamResponse" />
internal class StreamResponse : Response, IStreamResponse
{
    //Constructors
    public StreamResponse(IMethod method) : base(method)
    {
    }

    //Methods
    async Task<Stream> IStreamResponse.SendAsync(CancellationToken cancellationtoken)
    {
        using var response = await SendAsync(cancellationtoken);
        return await (response?.Content.ReadAsStreamAsync(cancellationtoken)).OrDefault();
    }
}
