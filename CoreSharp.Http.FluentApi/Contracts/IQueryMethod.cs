using System;
using System.Collections.Generic;
using System.IO;
using JsonNet = Newtonsoft.Json;
using TextJson = System.Text.Json;

namespace CoreSharp.Http.FluentApi.Contracts
{
    public interface IQueryMethod : IMethodWithResponse
    {
        //Properties 
        internal IDictionary<string, object> QueryParameters { get; }

        //Methods
        /// <inheritdoc cref="Query(IDictionary{string, object})" />
        public IQueryMethod Query<TQueryParameter>(TQueryParameter queryParameter)
            where TQueryParameter : class;

        /// <inheritdoc cref="Query(string, object)" />
        public IQueryMethod Query(IDictionary<string, object> parameters);

        /// <summary>
        /// Set query parameter.
        /// </summary>
        public IQueryMethod Query(string key, object value);

        /// <inheritdoc cref="IMethodWithResponse.To{TResponse}" />
        public new IGenericQueryResponse<TResponse> To<TResponse>()
            where TResponse : class;

        /// <inheritdoc cref="Json{TResponse}(JsonNet.JsonSerializerSettings)" />
        public new IJsonQueryResponse<TResponse> Json<TResponse>()
            where TResponse : class;

        /// <inheritdoc cref="Json{TResponse}(Func{Stream, TResponse})" />
        public new IJsonQueryResponse<TResponse> Json<TResponse>(JsonNet.JsonSerializerSettings jsonSerializerSettings)
            where TResponse : class;

        /// <inheritdoc cref="Json{TResponse}(Func{Stream, TResponse})" />
        public new IJsonQueryResponse<TResponse> Json<TResponse>(TextJson.JsonSerializerOptions jsonSerializerOptions)
            where TResponse : class;

        /// <inheritdoc cref="Json{TResponse}(Func{Stream, TResponse})" />
        public new IJsonQueryResponse<TResponse> Json<TResponse>(Func<string, TResponse> deserializeStringFunction)
            where TResponse : class;

        /// <inheritdoc cref="IMethodWithResponse.Json{TResponse}(Func{Stream, TResponse})" />
        public new IJsonQueryResponse<TResponse> Json<TResponse>(Func<Stream, TResponse> deserializeStreamFunction)
            where TResponse : class;

        /// <inheritdoc cref="Xml{TResponse}(Func{Stream, TResponse})" />
        public new IXmlQueryResponse<TResponse> Xml<TResponse>()
            where TResponse : class;

        /// <inheritdoc cref="Xml{TResponse}(Func{string, TResponse})"/>
        public new IXmlQueryResponse<TResponse> Xml<TResponse>(Func<Stream, TResponse> deserializeStreamFunction)
            where TResponse : class;

        /// <inheritdoc cref="IMethodWithResponse.Xml{TResponse}(Func{string, TResponse})" />
        public new IXmlQueryResponse<TResponse> Xml<TResponse>(Func<string, TResponse> deserializeStringFunction)
            where TResponse : class;
    }
}
