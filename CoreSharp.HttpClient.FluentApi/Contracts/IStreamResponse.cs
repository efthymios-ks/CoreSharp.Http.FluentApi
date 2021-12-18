using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IStreamResponse : IResponse
    {
        /// <inheritdoc cref="IResponse.SendAsync(CancellationToken)"/>
        public new Task<Stream> SendAsync(CancellationToken cancellationtoken = default);
    }
}
