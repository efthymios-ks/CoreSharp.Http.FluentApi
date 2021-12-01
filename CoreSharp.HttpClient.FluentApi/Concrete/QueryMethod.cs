﻿using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.Models.Newtonsoft.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    internal class QueryMethod : Method, IQueryMethod
    {
        //Constructors 
        public QueryMethod(IRoute route, HttpMethod httpMethod) : base(route, httpMethod)
            => HttpMethodX.ValidateQueryMethod(httpMethod);

        //Properties 
        private IQueryMethod Me => this;
        IDictionary<string, object> IQueryMethod.QueryParameters { get; } = new Dictionary<string, object>();

        //Methods 
        public IQueryMethod Query<TQueryParameter>(TQueryParameter queryParameter) where TQueryParameter : class
        {
            _ = queryParameter ?? throw new ArgumentNullException(nameof(queryParameter));

            var parameters = queryParameter.GetPropertiesDictionary();
            return Query(parameters);
        }

        public IQueryMethod Query(IDictionary<string, object> parameters)
        {
            _ = parameters ?? throw new ArgumentNullException(nameof(parameters));

            foreach (var (key, value) in parameters)
                Query(key, value);

            return this;
        }

        public IQueryMethod Query(string key, object value)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            Me.QueryParameters.AddOrUpdate(key, value);
            return this;
        }

        public new IJsonQueryResponse<TResponse> Json<TResponse>()
            where TResponse : class
            => Json<TResponse>(DefaultJsonSettings.Instance);

        public new IJsonQueryResponse<TResponse> Json<TResponse>(JsonSerializerSettings jsonSerializerSettings)
            where TResponse : class
        {
            _ = jsonSerializerSettings ?? throw new ArgumentNullException(nameof(jsonSerializerSettings));

            TResponse DeserializeStreamFunction(Stream stream) => stream.ToEntity<TResponse>(jsonSerializerSettings);
            return Json(DeserializeStreamFunction);
        }

        public new IJsonQueryResponse<TResponse> Json<TResponse>(Func<string, TResponse> deserializeStringFunction)
            where TResponse : class
        {
            _ = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

            return new JsonQueryResponse<TResponse>(this, deserializeStringFunction);
        }

        public new IJsonQueryResponse<TResponse> Json<TResponse>(Func<Stream, TResponse> deserializeStreamFunction)
            where TResponse : class
        {
            _ = deserializeStreamFunction ?? throw new ArgumentNullException(nameof(deserializeStreamFunction));

            return new JsonQueryResponse<TResponse>(this, deserializeStreamFunction);
        }
    }
}
