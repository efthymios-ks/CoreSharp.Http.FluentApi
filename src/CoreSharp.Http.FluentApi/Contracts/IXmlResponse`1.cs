using System;
using System.IO;

namespace CoreSharp.Http.FluentApi.Contracts;

public interface IXmlResponse<TResponse> : IGenericResponse<TResponse>
    where TResponse : class
{
    //Properties 
    internal Func<Stream, TResponse> DeserializeStreamFunction { get; set; }
    internal Func<string, TResponse> DeserializeStringFunction { get; set; }
}
