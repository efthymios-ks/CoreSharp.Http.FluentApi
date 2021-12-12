using CoreSharp.HttpClient.FluentApi.Contracts;
using System;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    /// <inheritdoc cref="ICacheQueryResponse{TResponse}"/>
    internal class CacheQueryResponse<TResponse> : GenericQueryResponse<TResponse>, ICacheQueryResponse<TResponse> where TResponse : class
    {
        //Constructors
        public CacheQueryResponse(IQueryMethod queryMethod) : base(queryMethod)
        {
        }

        //Properties 
        private ICacheQueryResponse<TResponse> Me => this;
        TimeSpan? ICacheQueryResponse<TResponse>.Duration { get; set; }

        //Methods 
        public ICacheQueryResponse<TResponse> Cache(TimeSpan duration)
        {
            Me.Duration = duration;
            return this;
        }
    }
}
