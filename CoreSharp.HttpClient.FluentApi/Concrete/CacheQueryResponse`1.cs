using CoreSharp.HttpClient.FluentApi.Contracts;
using System;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    internal class CacheQueryResponse<TResponse> : GenericQueryResponse<TResponse>, ICacheQueryResponse<TResponse> where TResponse : class
    {
        //Constructors
        public CacheQueryResponse(IQueryMethod queryMethod) : base(queryMethod)
        {
        }

        //Properties 
        TimeSpan? ICacheQueryResponse<TResponse>.Duration { get; set; }
    }
}
