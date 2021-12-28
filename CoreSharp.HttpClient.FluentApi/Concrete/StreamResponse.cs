using CoreSharp.HttpClient.FluentApi.Contracts;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    /// <inheritdoc cref="IStreamResponse" />
    public class StreamResponse : Response, IStreamResponse
    {
        //Constructors
        public StreamResponse(IMethod method) : base(method)
        {
        }

        //Methods
        async Task<Stream> IStreamResponse.SendAsync(CancellationToken cancellationtoken)
        {
            using var response = await SendAsync(cancellationtoken);
            if (response is null)
                return default;
            return await response.Content.ReadAsStreamAsync(cancellationtoken);
        }
    }
}
