using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Concrete;
using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.Models;
using CoreSharp.Models.Newtonsoft.Settings;
using Newtonsoft.Json;
using System;
using System.IO;

namespace CoreSharp.HttpClient.FluentApi.Extensions
{
    /// <summary>
    /// <see cref="IQueryMethod"/> extensions.
    /// </summary>
    public static class IQueryMethodExtensions
    {
        //Methods 
        /// <inheritdoc cref="Query(IQueryMethod, string)" />
        public static IQueryMethod Query<TQueryParameter>(this IQueryMethod queryMethod, TQueryParameter queryParameter) where TQueryParameter : class
        {
            _ = queryMethod ?? throw new ArgumentNullException(nameof(queryParameter));

            var queryBuilder = new UrlQueryBuilder();
            queryBuilder.Parse(queryParameter);
            return queryMethod.Query($"{queryBuilder}");
        }

        /// <summary>
        /// Define query parameter.
        /// </summary>
        public static IQueryMethod Query(this IQueryMethod queryMethod, string queryParameter)
        {
            _ = queryMethod ?? throw new ArgumentNullException(nameof(queryMethod));

            queryMethod.QueryParameter = queryParameter;

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

        /// <inheritdoc cref="IMethodExtensions.Json{TResponse}(IMethod, Func{Stream, TResponse})" />
        public static IJsonQueryResponse<TResponse> Json<TResponse>(this IQueryMethod queryMethod, Func<Stream, TResponse> deserializeFunction)
            where TResponse : class
        {
            _ = queryMethod ?? throw new ArgumentNullException(nameof(queryMethod));
            _ = deserializeFunction ?? throw new ArgumentNullException(nameof(deserializeFunction));

            return new JsonQueryResponse<TResponse>(queryMethod, deserializeFunction);
        }
    }
}
