using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps;

/// <inheritdoc cref="IJsonQueryResponse{TResponse}"/>
internal class JsonQueryResponse<TResponse> : CacheQueryResponse<TResponse>, IJsonQueryResponse<TResponse>
    where TResponse : class
{
    // Constructors
    public JsonQueryResponse(IQueryMethod queryMethod, Func<Stream, TResponse> deserializeStreamFunction)
        : this(queryMethod)
        => Me.DeserializeStreamFunction = deserializeStreamFunction ?? throw new ArgumentNullException(nameof(deserializeStreamFunction));

    public JsonQueryResponse(IQueryMethod queryMethod, Func<string, TResponse> deserializeStringFunction)
        : this(queryMethod)
        => Me.DeserializeStringFunction = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

    public JsonQueryResponse(IQueryMethod queryMethod)
        : base(queryMethod)
    {
    }

    // Properties 
    private IJsonQueryResponse<TResponse> Me
        => this;

    Func<Stream, TResponse> IJsonResponse<TResponse>.DeserializeStreamFunction { get; set; }

    Func<string, TResponse> IJsonResponse<TResponse>.DeserializeStringFunction { get; set; }

    // Methods 
    IJsonQueryResponse<TResponse> IJsonQueryResponse<TResponse>.Cache(TimeSpan duration)
    {
        (this as ICacheQueryResponse<TResponse>)!.Cache(duration);
        return this;
    }

    async ValueTask<TResponse> IJsonQueryResponse<TResponse>.SendAsync(CancellationToken cancellationToken)
    {
        var requestTask = SendAsync(cancellationToken);
        return await ICacheQueryResponseX.RequestWithCacheAsync(this, requestTask);
    }

    public override async Task<TResponse> SendAsync(CancellationToken cancellationToken = default)
        => await IJsonResponseX.SendAsync(this, cancellationToken);
}
