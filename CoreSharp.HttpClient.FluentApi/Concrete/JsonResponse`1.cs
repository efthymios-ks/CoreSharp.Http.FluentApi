using CoreSharp.HttpClient.FluentApi.Contracts;
using System;
using System.IO;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    internal class JsonResponse<TResponse> : GenericResponse<TResponse>, IJsonResponse<TResponse> where TResponse : class
    {
        //Constructors
        public JsonResponse(IMethod method, Func<Stream, TResponse> deserializeFunction)
            : base(method)
        {
            _ = deserializeFunction ?? throw new ArgumentNullException(nameof(deserializeFunction));

            if (this is IJsonResponse<TResponse> jsonResponse)
                jsonResponse.DeserializeFunction = deserializeFunction;
        }

        //Properties 
        Func<Stream, TResponse> IJsonResponse<TResponse>.DeserializeFunction { get; set; }
    }
}
