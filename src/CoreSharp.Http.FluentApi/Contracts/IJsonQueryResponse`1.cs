using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Contracts;

public interface IJsonQueryResponse<TResponse> : IJsonResponse<TResponse>, ICacheQueryResponse<TResponse>
    where TResponse : class
{
    // Methods
    /// <inheritdoc cref="ICacheQueryResponse{TResponse}.Cache(TimeSpan)"/>
    public new IJsonQueryResponse<TResponse> Cache(TimeSpan duration);

    /// <inheritdoc cref="IGenericResponse{TResponse}.SendAsync(CancellationToken)"/>
    public new ValueTask<TResponse> SendAsync(CancellationToken cancellationToken = default);
}
