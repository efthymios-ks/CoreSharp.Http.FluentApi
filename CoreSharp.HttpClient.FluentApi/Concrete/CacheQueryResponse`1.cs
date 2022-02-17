using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.HttpClient.FluentApi.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    /// <inheritdoc cref="ICacheQueryResponse{TResponse}"/>
    internal class CacheQueryResponse<TResponse> : GenericQueryResponse<TResponse>, ICacheQueryResponse<TResponse>
        where TResponse : class
    {
        //Constructors
        public CacheQueryResponse(IGenericQueryResponse<TResponse> genericQueryResponse)
            : this(genericQueryResponse?.Method as IQueryMethod)
        {
        }

        public CacheQueryResponse(IQueryMethod queryMethod)
            : base(queryMethod)
        {
        }

        //Properties 
        private ICacheQueryResponse<TResponse> Me
            => this;

        TimeSpan? ICacheQueryResponse<TResponse>.Duration { get; set; }

        //Methods 
        ICacheQueryResponse<TResponse> ICacheQueryResponse<TResponse>.Cache(TimeSpan duration)
        {
            Me.Duration = duration;
            return this;
        }

        async ValueTask<TResponse> ICacheQueryResponse<TResponse>.SendAsync(CancellationToken cancellationToken)
        {
            var requestTask = (this as IGenericQueryResponse<TResponse>)!.SendAsync(cancellationToken);
            var route = Me.Method.Route.Route;
            var cacheDuration = Me.Duration;
            return await ICacheQueryX.CachedRequestAsync(requestTask, route, cacheDuration);
        }
    }
}
