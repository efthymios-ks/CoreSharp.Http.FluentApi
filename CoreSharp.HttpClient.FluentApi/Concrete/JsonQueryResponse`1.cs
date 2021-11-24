using CoreSharp.HttpClient.FluentApi.Contracts;
using System;
using System.IO;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    internal class JsonQueryResponse<TResponse> : CacheQueryResponse<TResponse>, IJsonQueryResponse<TResponse> where TResponse : class
    {
        //Constructors
        public JsonQueryResponse(IQueryMethod queryMethod, Func<Stream, TResponse> deserializeFunction)
            : base(queryMethod)
        {
            _ = deserializeFunction ?? throw new ArgumentNullException(nameof(deserializeFunction));

            if (this is IJsonQueryResponse<TResponse> jsonQueryResponse)
                jsonQueryResponse.DeserializeFunction = deserializeFunction;
        }

        //Properties
        Func<Stream, TResponse> IJsonResponse<TResponse>.DeserializeFunction { get; set; }
    }
}
