using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;

public interface IMethod
{
    // Properties
    internal IEndpoint Endpoint { get; set; }
    internal HttpMethod HttpMethod { get; set; }

    // Methods
    /// <summary>
    /// Send an HTTP request as an asynchronous operation.
    /// </summary>
    Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default);
}
