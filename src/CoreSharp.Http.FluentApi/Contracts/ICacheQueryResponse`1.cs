using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Contracts
{
    public interface ICacheQueryResponse<TResponse> : IGenericQueryResponse<TResponse>
        where TResponse : class
    {
        //Properties
        internal TimeSpan? Duration { get; set; }

        //Methods 
        /// <inheritdoc cref="IGenericQueryResponse{TResponse}.Cache(TimeSpan)"/>
        public new ICacheQueryResponse<TResponse> Cache(TimeSpan duration);

        /// <inheritdoc cref="IGenericResponse{TResponse}.SendAsync(CancellationToken)"/>
        public new ValueTask<TResponse> SendAsync(CancellationToken cancellationToken = default);
    }
}
