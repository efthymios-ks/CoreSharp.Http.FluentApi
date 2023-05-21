using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps;

/// <inheritdoc cref="ICacheQueryResponse{TResponse}"/>
internal class CacheQueryResponse<TResponse> : GenericQueryResponse<TResponse>, ICacheQueryResponse<TResponse>
    where TResponse : class
{
    // Constructors
    public CacheQueryResponse(IGenericQueryResponse<TResponse> genericQueryResponse)
        : this(genericQueryResponse?.Method as IQueryMethod)
    {
    }

    public CacheQueryResponse(IQueryMethod queryMethod)
        : base(queryMethod)
    {
    }

    // Properties 
    private ICacheQueryResponse<TResponse> Me
        => this;

    TimeSpan ICacheQueryResponse<TResponse>.Duration { get; set; }

    Func<Task<bool>> ICacheQueryResponse<TResponse>.ForceNewRequestConditionFactory { get; set; }

    // Methods 
    ICacheQueryResponse<TResponse> ICacheQueryResponse<TResponse>.Cache(TimeSpan duration)
    {
        Me.Duration = duration;
        return this;
    }

    ICacheQueryResponse<TResponse> ICacheQueryResponse<TResponse>.ForceNew(bool forceNewRequest)
      => Me.ForceNew(() => forceNewRequest);

    ICacheQueryResponse<TResponse> ICacheQueryResponse<TResponse>.ForceNew(Func<bool> forceNewRequestConditionFactory)
      => Me.ForceNew(async () => await Task.FromResult(forceNewRequestConditionFactory()));

    ICacheQueryResponse<TResponse> ICacheQueryResponse<TResponse>.ForceNew(Func<Task<bool>> forceNewRequestConditionFactory)
    {
        ArgumentNullException.ThrowIfNull(forceNewRequestConditionFactory);

        Me.ForceNewRequestConditionFactory = forceNewRequestConditionFactory;
        return this;
    }

    async ValueTask<TResponse> ICacheQueryResponse<TResponse>.SendAsync(CancellationToken cancellationToken)
    {
        var requestTask = (this as IGenericQueryResponse<TResponse>)!.SendAsync(cancellationToken);
        return await ICacheQueryResponseX.RequestWithCacheAsync(this, requestTask);
    }
}
