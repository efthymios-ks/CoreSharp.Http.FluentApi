using System;
using System.IO;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IJsonResponse<TResponse> : IGenericResponse<TResponse> where TResponse : class
    {
        //Properties 
        internal Func<Stream, TResponse> DeserializeFunction { get; set; }
    }
}
