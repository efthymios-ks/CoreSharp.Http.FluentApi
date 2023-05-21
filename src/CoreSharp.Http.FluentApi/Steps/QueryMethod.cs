using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Utilities;
using CoreSharp.Json.JsonNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps;

/// <inheritdoc cref="IQueryMethod"/>
internal sealed class QueryMethod : MethodWithResponse, IQueryMethod
{
    // Constructors 
    public QueryMethod(IRoute route, HttpMethod httpMethod)
        : base(route, httpMethod)
        => HttpMethodX.ValidateQueryMethod(httpMethod);

    // Properties 
    private IQueryMethod Me
        => this;

    IDictionary<string, object> IQueryMethod.QueryParameters { get; } = new Dictionary<string, object>();

    // Methods 
    public override async Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default)
        => await IMethodX.SendAsync(this, queryParameters: Me.QueryParameters, cancellationToken: cancellationToken);

    IGenericQueryResponse<TResponse> IQueryMethod.To<TResponse>()
        where TResponse : class
        => new GenericQueryResponse<TResponse>(this);

    public IQueryMethod Query<TQueryParameter>(TQueryParameter queryParameter)
        where TQueryParameter : class
    {
        ArgumentNullException.ThrowIfNull(queryParameter);

        var parameters = queryParameter.GetPropertiesDictionary();
        return Query(parameters);
    }

    public IQueryMethod Query(IDictionary<string, object> parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        foreach (var (key, value) in parameters)
        {
            Query(key, value);
        }

        return this;
    }

    public IQueryMethod Query(string key, object value)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);

        if (value is null)
        {
            return this;
        }

        var queryParameters = Me.QueryParameters;
        queryParameters[key] = value;
        return this;
    }

    public new IJsonQueryResponse<TResponse> Json<TResponse>()
        where TResponse : class
        => Json<TResponse>(JsonSettings.Default);

    public new IJsonQueryResponse<TResponse> Json<TResponse>(JsonSerializerSettings jsonSerializerSettings)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(jsonSerializerSettings);

        return Json(DeserializeStreamFunction);

        TResponse DeserializeStreamFunction(Stream stream)
            => stream.FromJson<TResponse>(jsonSerializerSettings);
    }

    public new IJsonQueryResponse<TResponse> Json<TResponse>(JsonSerializerOptions jsonSerializerOptions)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(jsonSerializerOptions);

        return Json(DeserializeStreamFunction);

        TResponse DeserializeStreamFunction(Stream stream)
            => stream.FromJsonAsync<TResponse>(jsonSerializerOptions)
                     .GetAwaiter()
                     .GetResult();
    }

    public new IJsonQueryResponse<TResponse> Json<TResponse>(Func<string, TResponse> deserializeStringFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        return new JsonQueryResponse<TResponse>(this, deserializeStringFunction);
    }

    public new IJsonQueryResponse<TResponse> Json<TResponse>(Func<Stream, TResponse> deserializeStreamFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStreamFunction);

        return new JsonQueryResponse<TResponse>(this, deserializeStreamFunction);
    }

    IXmlQueryResponse<TResponse> IQueryMethod.Xml<TResponse>()
    {
        return Me.Xml(DeserializeStreamFunction);

        static TResponse DeserializeStreamFunction(Stream stream)
            => stream.FromXmlAsync<TResponse>()
                     .GetAwaiter()
                     .GetResult();
    }

    IXmlQueryResponse<TResponse> IQueryMethod.Xml<TResponse>(Func<Stream, TResponse> deserializeStreamFunction)
    {
        ArgumentNullException.ThrowIfNull(deserializeStreamFunction);

        return new XmlQueryResponse<TResponse>(this, deserializeStreamFunction);
    }

    IXmlQueryResponse<TResponse> IQueryMethod.Xml<TResponse>(Func<string, TResponse> deserializeStringFunction)
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        return new XmlQueryResponse<TResponse>(this, deserializeStringFunction);
    }
}
