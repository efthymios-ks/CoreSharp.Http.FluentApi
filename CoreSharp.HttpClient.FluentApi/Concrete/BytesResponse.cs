using CoreSharp.HttpClient.FluentApi.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    /// <inheritdoc cref="IBytesResponse" />
    public class BytesResponse : Response, IBytesResponse
    {
        //Constructors
        public BytesResponse(IMethod method) : base(method)
        {
        }

        //Methods
        async Task<byte[]> IBytesResponse.SendAsync(CancellationToken cancellationtoken)
        {
            using var response = await SendAsync(cancellationtoken);
            if (response is null)
                return default;
            return await response.Content.ReadAsByteArrayAsync(cancellationtoken);
        }
    }
}
