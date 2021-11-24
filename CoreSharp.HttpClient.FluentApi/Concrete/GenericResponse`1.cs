using CoreSharp.HttpClient.FluentApi.Contracts;
using System;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    internal class GenericResponse<TResponse> : IGenericResponse<TResponse> where TResponse : class
    {
        //Constructors
        public GenericResponse(IMethod method)
        {
            _ = method ?? throw new ArgumentNullException(nameof(method));

            if (this is IGenericResponse<TResponse> jsonResponse)
                jsonResponse.Method = method;
        }

        //Properties 
        IMethod IGenericResponse<TResponse>.Method { get; set; }
    }
}
