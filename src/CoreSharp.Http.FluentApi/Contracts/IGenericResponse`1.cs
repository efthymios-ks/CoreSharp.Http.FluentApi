using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Contracts;

public interface IGenericResponse<TResponse> : IResponse
    where TResponse : class
{
    //Methods 
    /// <inheritdoc cref="IResponse.SendAsync(CancellationToken)"/>
    public new Task<TResponse> SendAsync(CancellationToken cancellationToken = default);
}
