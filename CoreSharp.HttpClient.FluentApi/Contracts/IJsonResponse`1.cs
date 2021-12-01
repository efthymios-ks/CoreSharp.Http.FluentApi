using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IJsonResponse<TResponse> : IGenericResponse<TResponse> where TResponse : class
    {
        //Properties 
        internal Func<Stream, TResponse> DeserializeStreamFunction { get; set; }
        internal Func<string, TResponse> DeserializeStringFunction { get; set; }

        //Methods 
        /// <inheritdoc cref="IMethod.SendAsync(CancellationToken)"/>
        public Task<TResponse> SendAsync(CancellationToken cancellationToken = default);
    }
}
