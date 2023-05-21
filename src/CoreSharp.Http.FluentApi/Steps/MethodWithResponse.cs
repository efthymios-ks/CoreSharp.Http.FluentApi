using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Json.JsonNet;
using System;
using System.IO;
using System.Net.Http;
using JsonNet = Newtonsoft.Json;
using TextJson = System.Text.Json;

namespace CoreSharp.Http.FluentApi.Steps;

/// <inheritdoc cref="IMethod"/>
internal class MethodWithResponse : Method, IMethodWithResponse
{
    // Constructors 
    public MethodWithResponse(IRoute route, HttpMethod httpMethod)
        : base(route, httpMethod)
    {
    }

    // Methods 
    public IGenericResponse<TResponse> To<TResponse>()
        where TResponse : class
        => new GenericResponse<TResponse>(this);

    public IJsonResponse<TResponse> Json<TResponse>()
        where TResponse : class
        => Json<TResponse>(JsonSettings.Default);

    public IJsonResponse<TResponse> Json<TResponse>(JsonNet.JsonSerializerSettings jsonSerializerSettings)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(jsonSerializerSettings);

        TResponse DeserializeStreamFunction(Stream stream)
            => stream.FromJson<TResponse>(jsonSerializerSettings);
        return Json(DeserializeStreamFunction);
    }

    public IJsonResponse<TResponse> Json<TResponse>(TextJson.JsonSerializerOptions jsonSerializerOptions)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(jsonSerializerOptions);

        TResponse DeserializeStreamFunction(Stream stream)
            => stream.FromJsonAsync<TResponse>(jsonSerializerOptions)
                     .GetAwaiter()
                     .GetResult();
        return Json(DeserializeStreamFunction);
    }

    public IJsonResponse<TResponse> Json<TResponse>(Func<string, TResponse> deserializeStringFunction)
         where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        return new JsonResponse<TResponse>(this, deserializeStringFunction);
    }

    public IJsonResponse<TResponse> Json<TResponse>(Func<Stream, TResponse> deserializeStringFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        return new JsonResponse<TResponse>(this, deserializeStringFunction);
    }

    public IXmlResponse<TResponse> Xml<TResponse>()
        where TResponse : class
    {
        static TResponse DeserializeStringFunction(string xml)
            => xml.FromXml<TResponse>();

        return Xml(DeserializeStringFunction);
    }

    public IXmlResponse<TResponse> Xml<TResponse>(Func<Stream, TResponse> deserializeStreamFunction) where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStreamFunction);

        return new XmlResponse<TResponse>(this, deserializeStreamFunction);
    }

    public IXmlResponse<TResponse> Xml<TResponse>(Func<string, TResponse> deserializeStringFunction)
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        return new XmlResponse<TResponse>(this, deserializeStringFunction);
    }

    public IStringResponse String()
        => new StringResponse(this);

    public IStreamResponse Stream()
        => new StreamResponse(this);

    public IBytesResponse Bytes()
        => new BytesResponse(this);
}
