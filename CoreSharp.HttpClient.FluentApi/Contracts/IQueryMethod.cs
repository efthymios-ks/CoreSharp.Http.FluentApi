using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IQueryMethod : IMethod
    {
        //Properties 
        internal IDictionary<string, object> QueryParameters { get; }

        //Methods
        /// <inheritdoc cref="Query(IDictionary{string, object})" />
        public IQueryMethod Query<TQueryParameter>(TQueryParameter queryParameter) where TQueryParameter : class;

        /// <inheritdoc cref="Query(string, object)" />
        public IQueryMethod Query(IDictionary<string, object> parameters);

        /// <summary>
        /// Define query parameter.
        /// </summary>
        public IQueryMethod Query(string key, object value);

        /// <inheritdoc cref="Json{TResponse}(JsonSerializerSettings)" />
        public new IJsonQueryResponse<TResponse> Json<TResponse>() where TResponse : class;

        /// <inheritdoc cref="Json{TResponse}(Func{Stream, TResponse})" />
        public new IJsonQueryResponse<TResponse> Json<TResponse>(JsonSerializerSettings jsonSerializerSettings) where TResponse : class;

        /// <inheritdoc cref="Json{TResponse}(Func{Stream, TResponse})" />
        public new IJsonQueryResponse<TResponse> Json<TResponse>(Func<string, TResponse> deserializeStringFunction) where TResponse : class;

        /// <inheritdoc cref="IMethod.Json{TResponse}(Func{Stream, TResponse})" />
        public new IJsonQueryResponse<TResponse> Json<TResponse>(Func<Stream, TResponse> deserializeStreamFunction) where TResponse : class;

        /// <inheritdoc cref="IMethod.To{TResponse}" />
        public new IGenericQueryResponse<TResponse> To<TResponse>() where TResponse : class;
    }
}
