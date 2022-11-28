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

    TimeSpan? ICacheQueryResponse<TResponse>.Duration { get; set; }

    // Methods 
    ICacheQueryResponse<TResponse> ICacheQueryResponse<TResponse>.Cache(TimeSpan duration)
    {
        Me.Duration = duration;
        return this;
    }

    async ValueTask<TResponse> ICacheQueryResponse<TResponse>.SendAsync(CancellationToken cancellationToken)
    {
        var requestTask = (this as IGenericQueryResponse<TResponse>)!.SendAsync(cancellationToken);
        return await ICacheQueryResponseX.RequestWithCacheAsync(this, requestTask);
    }
}
