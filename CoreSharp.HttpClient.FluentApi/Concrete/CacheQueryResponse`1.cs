using CoreSharp.HttpClient.FluentApi.Contracts;
using System;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    /// <inheritdoc cref="ICacheQueryResponse{TResponse}"/>
    public abstract class CacheQueryResponse<TResponse> : GenericQueryResponseBase<TResponse>, ICacheQueryResponse<TResponse> where TResponse : class
    {
        //Constructors
        protected CacheQueryResponse(IQueryMethod queryMethod) : base(queryMethod)
        {
        }

        //Properties 
        private ICacheQueryResponse<TResponse> Me => this;
        TimeSpan? ICacheQueryResponse<TResponse>.Duration { get; set; }

        //Methods 
        ICacheQueryResponse<TResponse> ICacheQueryResponse<TResponse>.Cache(TimeSpan duration)
        {
            Me.Duration = duration;
            return this;
        }
    }
}
