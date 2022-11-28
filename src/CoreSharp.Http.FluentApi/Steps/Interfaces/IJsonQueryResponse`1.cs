using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces;

public interface IJsonQueryResponse<TResponse> : IJsonResponse<TResponse>, ICacheQueryResponse<TResponse>
    where TResponse : class
{
    // Methods
    /// <inheritdoc cref="ICacheQueryResponse{TResponse}.Cache(TimeSpan)"/>
    public new IJsonQueryResponse<TResponse> Cache(TimeSpan duration);

    /// <inheritdoc cref="ForceNew(Func{bool})"/>
    public new IJsonQueryResponse<TResponse> ForceNew(bool forceNewRequest);

    /// <inheritdoc cref="ForceNew(Func{Task{bool}})"/>
    public new IJsonQueryResponse<TResponse> ForceNew(Func<bool> forceNewRequestConditionFactory);

    /// <inheritdoc cref="ICacheQueryResponse{TResponse}.ForceNew(Func{Task{bool}})"/>
    public new IJsonQueryResponse<TResponse> ForceNew(Func<Task<bool>> forceNewRequestConditionFactory);

    /// <inheritdoc cref="IGenericResponse{TResponse}.SendAsync(CancellationToken)"/>
    public new ValueTask<TResponse> SendAsync(CancellationToken cancellationToken = default);
}
