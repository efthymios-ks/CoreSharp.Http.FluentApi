using CoreSharp.Http.FluentApi.Steps.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps;

/// <inheritdoc cref="IStringResponse" />
internal class StringResponse : Response, IStringResponse
{
    // Constructors
    public StringResponse(IMethod method)
        : base(method)
    {
    }

    // Methods
    async Task<string> IStringResponse.SendAsync(CancellationToken cancellationtoken)
    {
        using var response = await SendAsync(cancellationtoken);
        if (response is null)
        {
            return null;
        }

        return await response?.Content.ReadAsStringAsync(cancellationtoken);
    }
}
