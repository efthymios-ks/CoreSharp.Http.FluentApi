using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Concrete;
using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.Models.Newtonsoft.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoreSharp.HttpClient.FluentApi.Extensions
{
    /// <summary>
    /// <see cref="IQueryMethod"/> extensions.
    /// </summary>
    public static class IQueryMethodExtensions
    {
        //Methods 
        /// <inheritdoc cref="Query(IQueryMethod, IDictionary{string, object})" />
        public static IQueryMethod Query<TQueryParameter>(this IQueryMethod queryMethod, TQueryParameter queryParameter) where TQueryParameter : class
        {
            _ = queryMethod ?? throw new ArgumentNullException(nameof(queryParameter));
            _ = queryParameter ?? throw new ArgumentNullException(nameof(queryParameter));

            var parameters = queryParameter.GetPropertiesDictionary();
            return queryMethod.Query(parameters);
        }

        /// <inheritdoc cref="Query(IQueryMethod, string, object)" />
        public static IQueryMethod Query(this IQueryMethod queryMethod, IDictionary<string, object> parameters)
        {
            _ = queryMethod ?? throw new ArgumentNullException(nameof(queryMethod));
            _ = parameters ?? throw new ArgumentNullException(nameof(parameters));

            foreach (var (key, value) in parameters)
                queryMethod.Query(key, value);

            return queryMethod;
        }

        /// <summary>
        /// Define query parameter.
        /// </summary>
        public static IQueryMethod Query(this IQueryMethod queryMethod, string key, object value)
        {
            _ = queryMethod ?? throw new ArgumentNullException(nameof(queryMethod));
            _ = value ?? throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            queryMethod.QueryParameters.AddOrUpdate(key, value);
            return queryMethod;
        }

        /// <inheritdoc cref="Json{TResponse}(IQueryMethod, JsonSerializerSettings)" />
        public static IJsonQueryResponse<TResponse> Json<TResponse>(this IQueryMethod queryMethod)
            where TResponse : class
            => queryMethod.Json<TResponse>(DefaultJsonSettings.Instance);

        /// <inheritdoc cref="Json{TResponse}(IQueryMethod, Func{Stream, TResponse})" />
        public static IJsonQueryResponse<TResponse> Json<TResponse>(this IQueryMethod queryMethod, JsonSerializerSettings jsonSerializerSettings)
            where TResponse : class
        {
            _ = jsonSerializerSettings ?? throw new ArgumentNullException(nameof(jsonSerializerSettings));

            TResponse DeserializeFunction(Stream stream) => stream.ToEntity<TResponse>(jsonSerializerSettings);
            return queryMethod.Json(DeserializeFunction);
        }

        /// <inheritdoc cref="Json{TResponse}(IQueryMethod, Func{Stream, TResponse})" />
        public static IJsonQueryResponse<TResponse> Json<TResponse>(this IQueryMethod queryMethod, Func<string, TResponse> deserializeStringFunction)
            where TResponse : class
        {
            _ = queryMethod ?? throw new ArgumentNullException(nameof(queryMethod));
            _ = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

            return new JsonQueryResponse<TResponse>(queryMethod, deserializeStringFunction);
        }

        /// <inheritdoc cref="IMethodExtensions.Json{TResponse}(IMethod, Func{Stream, TResponse})" />
        public static IJsonQueryResponse<TResponse> Json<TResponse>(this IQueryMethod queryMethod, Func<Stream, TResponse> deserializeStreamFunction)
            where TResponse : class
        {
            _ = queryMethod ?? throw new ArgumentNullException(nameof(queryMethod));
            _ = deserializeStreamFunction ?? throw new ArgumentNullException(nameof(deserializeStreamFunction));

            return new JsonQueryResponse<TResponse>(queryMethod, deserializeStreamFunction);
        }
    }
}
