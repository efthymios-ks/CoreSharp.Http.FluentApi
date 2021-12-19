using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.HttpClient.FluentApi.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    /// <inheritdoc cref="IXmlQueryResponse{TResponse}"/>
    internal class XmlQueryResponse<TResponse> : CacheQueryResponse<TResponse>, IXmlQueryResponse<TResponse>
        where TResponse : class
    {
        //Constructors
        public XmlQueryResponse(IQueryMethod queryMethod, Func<string, TResponse> deserializeStringFunction)
            : this(queryMethod)
            => Me.DeserializeStringFunction = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

        public XmlQueryResponse(IQueryMethod queryMethod) : base(queryMethod)
        {
        }

        //Properties 
        private IXmlQueryResponse<TResponse> Me => this;
        Func<string, TResponse> IXmlResponse<TResponse>.DeserializeStringFunction { get; set; }

        //Methods 
        IXmlQueryResponse<TResponse> IXmlQueryResponse<TResponse>.Cache(TimeSpan duration)
        {
            (this as ICacheQueryResponse<TResponse>)!.Cache(duration);
            return this;
        }

        async ValueTask<TResponse> IXmlQueryResponse<TResponse>.SendAsync(CancellationToken cancellationToken)
        {
            var requestTask = SendAsync(cancellationToken);
            var route = Me.Method.Route.Route;
            var cacheDuration = Me.Duration;
            return await ICacheQueryX.CachedRequestAsync(requestTask, route, cacheDuration);
        }

        public override async Task<TResponse> SendAsync(CancellationToken cancellationToken = default)
            => await IXmlResponseX.SendAsync(this, cancellationToken);
    }
}
