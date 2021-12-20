﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IXmlQueryResponse<TResponse> : IXmlResponse<TResponse>, ICacheQueryResponse<TResponse>
        where TResponse : class
    {
        //Methods
        /// <inheritdoc cref="ICacheQueryResponse{TResponse}.Cache(TimeSpan)"/>
        public new IXmlQueryResponse<TResponse> Cache(TimeSpan duration);

        /// <inheritdoc cref="IGenericResponse{TResponse}.SendAsync(CancellationToken)"/>
        public new ValueTask<TResponse> SendAsync(CancellationToken cancellationToken = default);
    }
}