using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.Models;
using CoreSharp.Models.Newtonsoft.Settings;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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

            var me = Me;
            me.Route = route;
            me.HttpMethod = httpMethod;
        }

        //Properties 
        private IMethod Me => this;
        IRoute IMethod.Route { get; set; }
        HttpMethod IMethod.HttpMethod { get; set; }

        //Methods 
        public async Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default)
        {
            //Extract values 
            var httpClient = Me.Route.Request.HttpClient;
            var headers = Me.Route.Request.HeadersInternal;
            var throwOnError = Me.Route.Request.ThrowOnError;
            var route = Me.Route.Route;
            var httpMethod = Me.HttpMethod;
            var completionOption = Me.Route.Request.CompletionOptionInternal;
            var queryParameters = (Me as IQueryMethod)?.QueryParameters;
            var httpContent = (Me as IContentMethod)?.ContentInternal;

            //Add query parameter
            if (httpMethod == HttpMethod.Get && queryParameters.Count > 0)
            {
                var queryBuilder = new UrlQueryBuilder
                {
                    queryParameters
                };
                var queryParameter = queryBuilder.ToString();
                route += queryParameter;
            }

            //Create request 
            using var request = new HttpRequestMessage(httpMethod, route)
            {
                Content = httpContent
            };
            foreach (var (key, value) in headers)
            {
                if (request.Headers.Contains(key))
                    request.Headers.Remove(key);
                request.Headers.Add(key, value);
            }

            //Send request 
            var response = await httpClient.SendAsync(request, completionOption, cancellationToken);

            //Check response status 
            if (throwOnError)
                await response.EnsureSuccessAsync();

            return response;
        }

        public IJsonResponse<TResponse> Json<TResponse>()
            where TResponse : class
            => Json<TResponse>(DefaultJsonSettings.Instance);

        public IGenericResponse<TResponse> To<TResponse>() where TResponse : class
            => new GenericResponse<TResponse>(this);

        public IJsonResponse<TResponse> Json<TResponse>(JsonSerializerSettings jsonSerializerSettings)
            where TResponse : class
        {
            _ = jsonSerializerSettings ?? throw new ArgumentNullException(nameof(jsonSerializerSettings));

            TResponse DeserializeStreamFunction(Stream stream) => stream.FromJson<TResponse>(jsonSerializerSettings);
            return Json(DeserializeStreamFunction);
        }

        public IJsonResponse<TResponse> Json<TResponse>(Func<string, TResponse> deserializeStreamFunction)
             where TResponse : class
        {
            _ = deserializeStreamFunction ?? throw new ArgumentNullException(nameof(deserializeStreamFunction));

            return new JsonResponse<TResponse>(this, deserializeStreamFunction);
        }

        public IJsonResponse<TResponse> Json<TResponse>(Func<Stream, TResponse> deserializeStringFunction)
            where TResponse : class
        {
            _ = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

            return new JsonResponse<TResponse>(this, deserializeStringFunction);
        }

        public IStringResponse String()
            => new StringResponse(this);

        public IStreamResponse Stream()
            => new StreamResponse(this);

        public IBytesResponse Bytes()
            => new BytesResponse(this);
    }
}
