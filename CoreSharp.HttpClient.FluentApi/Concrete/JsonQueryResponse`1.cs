using CoreSharp.HttpClient.FluentApi.Contracts;
using System;
using System.IO;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    internal class JsonQueryResponse<TResponse> : CacheQueryResponse<TResponse>, IJsonQueryResponse<TResponse> where TResponse : class
    {
        //Constructors
        public JsonQueryResponse(IQueryMethod queryMethod, Func<Stream, TResponse> deserializeStreamFunction)
            : this(queryMethod)
        {
            _ = deserializeStreamFunction ?? throw new ArgumentNullException(nameof(deserializeStreamFunction));

            if (this is IJsonQueryResponse<TResponse> jsonQueryResponse)
                jsonQueryResponse.DeserializeStreamFunction = deserializeStreamFunction;
        }

        public JsonQueryResponse(IQueryMethod queryMethod, Func<string, TResponse> deserializeStringFunction)
            : this(queryMethod)
        {
            _ = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

            if (this is IJsonQueryResponse<TResponse> jsonQueryResponse)
                jsonQueryResponse.DeserializeStringFunction = deserializeStringFunction;
        }

        public JsonQueryResponse(IQueryMethod queryMethod) : base(queryMethod)
        {
        }

        //Properties
        Func<Stream, TResponse> IJsonResponse<TResponse>.DeserializeStreamFunction { get; set; }
        Func<string, TResponse> IJsonResponse<TResponse>.DeserializeStringFunction { get; set; }
    }
}
