using CoreSharp.HttpClient.FluentApi.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    /// <inheritdoc cref="IStringResponse" />
    public class StringResponse : Response, IStringResponse
    {
        //Constructors
        public StringResponse(IMethod method) : base(method)
        {
        }

        //Methods
        async Task<string> IStringResponse.SendAsync(CancellationToken cancellationtoken)
        {
            using var response = await SendAsync(cancellationtoken);
            return await response.Content.ReadAsStringAsync(cancellationtoken);
        }
    }
}
