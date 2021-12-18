using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IBytesResponse : IResponse
    {
        /// <inheritdoc cref="IResponse.SendAsync(CancellationToken)"/>
        public new Task<byte[]> SendAsync(CancellationToken cancellationtoken = default);
    }
}
