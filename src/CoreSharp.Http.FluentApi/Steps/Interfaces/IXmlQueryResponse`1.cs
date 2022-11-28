using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces;

public interface IXmlQueryResponse<TResponse> : IXmlResponse<TResponse>, ICacheQueryResponse<TResponse>
    where TResponse : class
{
    // Methods
    /// <inheritdoc cref="ICacheQueryResponse{TResponse}.Cache(TimeSpan)"/>
    public new IXmlQueryResponse<TResponse> Cache(TimeSpan duration);

    /// <inheritdoc cref="ForceNew(Func{bool})"/>
    public new IXmlQueryResponse<TResponse> ForceNew(bool forceNewRequest);

    /// <inheritdoc cref="ForceNew(Func{Task{bool}})"/>
    public new IXmlQueryResponse<TResponse> ForceNew(Func<bool> forceNewRequestConditionFactory);

    /// <inheritdoc cref="ICacheQueryResponse{TResponse}.ForceNew(Func{Task{bool}})"/>
    public new IXmlQueryResponse<TResponse> ForceNew(Func<Task<bool>> forceNewRequestConditionFactory);

    /// <inheritdoc cref="IGenericResponse{TResponse}.SendAsync(CancellationToken)"/>
    public new ValueTask<TResponse> SendAsync(CancellationToken cancellationToken = default);
}
