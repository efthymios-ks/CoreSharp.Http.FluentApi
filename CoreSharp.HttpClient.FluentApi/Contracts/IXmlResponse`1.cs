using System;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IXmlResponse<TResponse> : IGenericResponse<TResponse>
        where TResponse : class
    {
        //Properties 
        internal Func<string, TResponse> DeserializeStringFunction { get; set; }
    }
}
