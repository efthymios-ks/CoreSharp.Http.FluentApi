using System;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IGenericQueryResponse<TResponse> : IGenericResponse<TResponse> where TResponse : class
    {
        //Methods 
        /// <summary>
        /// Enable in-memory, client-side response caching.
        /// </summary>
        public ICacheQueryResponse<TResponse> Cache(TimeSpan duration);
    }
}
