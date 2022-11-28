using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces;

public interface ICacheQueryResponse<TResponse> : IGenericQueryResponse<TResponse>
    where TResponse : class
{
    // Properties
    internal TimeSpan Duration { get; set; }

    internal Func<Task<bool>> ForceNewRequestConditionFactory { get; set; }

    // Methods 
    /// <inheritdoc cref="IGenericQueryResponse{TResponse}.Cache(TimeSpan)"/>
    public new ICacheQueryResponse<TResponse> Cache(TimeSpan duration);

    /// <inheritdoc cref="ForceNew(Func{bool})"/>
    public ICacheQueryResponse<TResponse> ForceNew(bool forceNewRequest);

    /// <inheritdoc cref="ForceNew(Func{Task{bool}})"/>
    public ICacheQueryResponse<TResponse> ForceNew(Func<bool> forceNewRequestConditionFactory);

    /// <summary>
    /// Force new request. Even if cached response is available.
    /// </summary> 
    public ICacheQueryResponse<TResponse> ForceNew(Func<Task<bool>> forceNewRequestConditionFactory);

    /// <inheritdoc cref="IGenericResponse{TResponse}.SendAsync(CancellationToken)"/>
    public new ValueTask<TResponse> SendAsync(CancellationToken cancellationToken = default);
}
