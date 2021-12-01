using System;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface ICacheQueryResponse<TResponse> : IGenericQueryResponse<TResponse> where TResponse : class
    {
        //Properties
        internal TimeSpan? Duration { get; set; }

        //Methods 
        /// <summary>
        /// Enable in-memory, client-side response caching.
        /// </summary>
        public ICacheQueryResponse<TResponse> Cache(TimeSpan duration);
    }
}
