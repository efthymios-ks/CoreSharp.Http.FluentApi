using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Contracts;

public interface IResponse
{
    // Properties
    internal IMethod Method { get; set; }

    // Methods 
    /// <inheritdoc cref="IMethod.SendAsync(CancellationToken)"/>
    public Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default);
}
