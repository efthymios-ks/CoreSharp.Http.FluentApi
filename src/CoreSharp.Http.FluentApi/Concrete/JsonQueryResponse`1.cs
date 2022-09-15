using CoreSharp.Http.FluentApi.Contracts;
using CoreSharp.Http.FluentApi.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Concrete;

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
        var route = Me.Method.Route.Route;
        var cacheDuration = Me.Duration;
        return await ICacheQueryX.CachedRequestAsync(requestTask, route, cacheDuration);
    }

    public override async Task<TResponse> SendAsync(CancellationToken cancellationToken = default)
        => await IJsonResponseX.SendAsync(this, cancellationToken);
}
