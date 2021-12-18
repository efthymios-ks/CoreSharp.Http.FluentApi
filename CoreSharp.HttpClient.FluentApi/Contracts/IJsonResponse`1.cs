using System;
using System.IO;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IJsonResponse<TResponse> : IGenericResponse<TResponse> where TResponse : class
    {
        //Properties 
        internal Func<Stream, TResponse> DeserializeStreamFunction { get; set; }
        internal Func<string, TResponse> DeserializeStringFunction { get; set; }
    }
}
