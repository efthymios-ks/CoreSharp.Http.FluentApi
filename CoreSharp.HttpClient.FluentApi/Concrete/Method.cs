using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.HttpClient.FluentApi.Utilities;
using CoreSharp.Models.Newtonsoft.Settings;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JsonNet = Newtonsoft.Json;
using TextJson = System.Text.Json;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    /// <inheritdoc cref="IMethod"/>
    internal class Method : IMethod
    {
        //Constructors 
        public Method(IRoute route, HttpMethod httpMethod)
        {
            _ = route ?? throw new ArgumentNullException(nameof(route));
            _ = httpMethod ?? throw new ArgumentNullException(nameof(httpMethod));

            Me.Route = route;
            Me.HttpMethod = httpMethod;
        }

        //Properties 
        private IMethod Me
            => this;

        IRoute IMethod.Route { get; set; }

        HttpMethod IMethod.HttpMethod { get; set; }

        //Methods 
        public virtual async Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default)
            => await IMethodX.SendAsync(this, cancellationToken: cancellationToken);

        public IGenericResponse<TResponse> To<TResponse>()
            where TResponse : class
            => new GenericResponse<TResponse>(this);

        public IJsonResponse<TResponse> Json<TResponse>()
            where TResponse : class
            => Json<TResponse>(DefaultJsonSettings.Instance);

        public IJsonResponse<TResponse> Json<TResponse>(JsonNet.JsonSerializerSettings jsonSerializerSettings)
            where TResponse : class
        {
            _ = jsonSerializerSettings ?? throw new ArgumentNullException(nameof(jsonSerializerSettings));

            TResponse DeserializeStreamFunction(Stream stream)
                => stream.FromJson<TResponse>(jsonSerializerSettings);
            return Json(DeserializeStreamFunction);
        }

        public IJsonResponse<TResponse> Json<TResponse>(TextJson.JsonSerializerOptions jsonSerializerOptions)
            where TResponse : class
        {
            _ = jsonSerializerOptions ?? throw new ArgumentNullException(nameof(jsonSerializerOptions));

            TResponse DeserializeStreamFunction(Stream stream)
                => stream.FromJsonAsync<TResponse>(jsonSerializerOptions)
                         .GetAwaiter()
                         .GetResult();
            return Json(DeserializeStreamFunction);
        }

        public IJsonResponse<TResponse> Json<TResponse>(Func<string, TResponse> deserializeStringFunction)
             where TResponse : class
        {
            _ = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

            return new JsonResponse<TResponse>(this, deserializeStringFunction);
        }

        public IJsonResponse<TResponse> Json<TResponse>(Func<Stream, TResponse> deserializeStringFunction)
            where TResponse : class
        {
            _ = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

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
            _ = deserializeStreamFunction ?? throw new ArgumentNullException(nameof(deserializeStreamFunction));

            return new XmlResponse<TResponse>(this, deserializeStreamFunction);
        }

        public IXmlResponse<TResponse> Xml<TResponse>(Func<string, TResponse> deserializeStringFunction)
            where TResponse : class
        {
            _ = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

            return new XmlResponse<TResponse>(this, deserializeStringFunction);
        }

        public IStringResponse String()
            => new StringResponse(this);

        public IStreamResponse Stream()
            => new StreamResponse(this);

        public IBytesResponse Bytes()
            => new BytesResponse(this);
    }
}
