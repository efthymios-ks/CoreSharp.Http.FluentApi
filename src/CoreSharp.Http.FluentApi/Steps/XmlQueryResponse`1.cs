using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps;

/// <inheritdoc cref="IXmlQueryResponse{TResponse}"/>
internal class XmlQueryResponse<TResponse> : CacheQueryResponse<TResponse>, IXmlQueryResponse<TResponse>
    where TResponse : class
{
    // Constructors
    public XmlQueryResponse(IQueryMethod queryMethod, Func<Stream, TResponse> deserializeStreamFunction)
        : this(queryMethod)
        => Me.DeserializeStreamFunction = deserializeStreamFunction ?? throw new ArgumentNullException(nameof(deserializeStreamFunction));

    public XmlQueryResponse(IQueryMethod queryMethod, Func<string, TResponse> deserializeStringFunction)
        : this(queryMethod)
        => Me.DeserializeStringFunction = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

    public XmlQueryResponse(IQueryMethod queryMethod)
        : base(queryMethod)
    {
    }

    // Properties 
    private IXmlQueryResponse<TResponse> Me
        => this;

    Func<Stream, TResponse> IXmlResponse<TResponse>.DeserializeStreamFunction { get; set; }

    Func<string, TResponse> IXmlResponse<TResponse>.DeserializeStringFunction { get; set; }

    // Methods 
    IXmlQueryResponse<TResponse> IXmlQueryResponse<TResponse>.Cache(TimeSpan duration)
    {
        (this as ICacheQueryResponse<TResponse>)!.Cache(duration);
        return this;
    }

    IXmlQueryResponse<TResponse> IXmlQueryResponse<TResponse>.ForceNew(bool forceNewRequest)
      => Me.ForceNew(() => forceNewRequest);

    IXmlQueryResponse<TResponse> IXmlQueryResponse<TResponse>.ForceNew(Func<bool> forceNewRequestConditionFactory)
      => Me.ForceNew(async () => await Task.FromResult(forceNewRequestConditionFactory()));

    IXmlQueryResponse<TResponse> IXmlQueryResponse<TResponse>.ForceNew(Func<Task<bool>> forceNewRequestConditionFactory)
    {
        (this as ICacheQueryResponse<TResponse>)!.ForceNew(forceNewRequestConditionFactory);
        return this;
    }

    async ValueTask<TResponse> IXmlQueryResponse<TResponse>.SendAsync(CancellationToken cancellationToken)
    {
        var requestTask = SendAsync(cancellationToken);
        return await ICacheQueryResponseX.RequestWithCacheAsync(this, requestTask);
    }

    public override async Task<TResponse> SendAsync(CancellationToken cancellationToken = default)
        => await IXmlResponseX.SendAsync(this, cancellationToken);
}
