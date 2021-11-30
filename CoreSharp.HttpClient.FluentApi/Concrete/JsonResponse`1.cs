using CoreSharp.HttpClient.FluentApi.Contracts;
using System;
using System.IO;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    internal class JsonResponse<TResponse> : GenericResponse<TResponse>, IJsonResponse<TResponse> where TResponse : class
    {
        //Constructors
        public JsonResponse(IMethod method, Func<Stream, TResponse> deserializeStreamFunction)
            : this(method)
        {
            _ = deserializeStreamFunction ?? throw new ArgumentNullException(nameof(deserializeStreamFunction));

            if (this is IJsonResponse<TResponse> jsonResponse)
                jsonResponse.DeserializeStreamFunction = deserializeStreamFunction;
        }

        public JsonResponse(IMethod method, Func<string, TResponse> deserializeStringFunction)
            : this(method)
        {
            _ = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

            if (this is IJsonResponse<TResponse> jsonResponse)
                jsonResponse.DeserializeStringFunction = deserializeStringFunction;
        }

        public JsonResponse(IMethod method) : base(method)
        {
        }

        //Properties 
        Func<Stream, TResponse> IJsonResponse<TResponse>.DeserializeStreamFunction { get; set; }
        Func<string, TResponse> IJsonResponse<TResponse>.DeserializeStringFunction { get; set; }
    }
}
