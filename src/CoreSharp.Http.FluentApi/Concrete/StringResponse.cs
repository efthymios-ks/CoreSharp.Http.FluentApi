using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Concrete;

/// <inheritdoc cref="IStringResponse" />
internal class StringResponse : Response, IStringResponse
{
    //Constructors
    public StringResponse(IMethod method)
        : base(method)
    {
    }

    //Methods
    async Task<string> IStringResponse.SendAsync(CancellationToken cancellationtoken)
    {
        using var response = await SendAsync(cancellationtoken);
        return await (response?.Content.ReadAsStringAsync(cancellationtoken)).OrDefault();
    }
}
