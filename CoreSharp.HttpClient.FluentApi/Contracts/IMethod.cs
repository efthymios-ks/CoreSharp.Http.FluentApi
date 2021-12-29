using CoreSharp.HttpClient.FluentApi.Utilities;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IMethod
    {
        //Properties
        internal IRoute Route { get; set; }
        /// <inheritdoc cref="HttpRequestMessage.Method" />
        internal HttpMethod HttpMethod { get; set; }

        //Methods 
        /// <inheritdoc cref="IMethodX.SendAsync(IMethod, IDictionary{string, object}, HttpContent, CancellationToken)" />
        Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Check <see cref="HeaderNames.ContentType"/>
        /// in <see cref="HttpResponseMessage.Headers"/>
        /// and deserialize accordingly.
        /// </summary>
        IGenericResponse<TResponse> To<TResponse>()
            where TResponse : class;

        /// <inheritdoc cref="Json{TResponse}(Func{Stream, TResponse})" />
        IJsonResponse<TResponse> Json<TResponse>()
            where TResponse : class;

        /// <inheritdoc cref="Json{TResponse}(JsonSerializerSettings)" />
        IJsonResponse<TResponse> Json<TResponse>(JsonSerializerSettings jsonSerializerSettings)
            where TResponse : class;

        /// <inheritdoc cref="Json{TResponse}(Func{string, TResponse})" />
        IJsonResponse<TResponse> Json<TResponse>(Func<Stream, TResponse> deserializeStringFunction)
            where TResponse : class;

        /// <summary>
        /// Treat <see cref="HttpResponseMessage.Content"/>
        /// as json and deserialize to provided type.
        /// </summary>
        IJsonResponse<TResponse> Json<TResponse>(Func<string, TResponse> deserializeStreamFunction)
            where TResponse : class;

        /// <inheritdoc cref="Xml{TResponse}(Func{Stream, TResponse})" />
        IXmlResponse<TResponse> Xml<TResponse>()
            where TResponse : class;

        /// <inheritdoc cref="Xml{TResponse}(Func{string, TResponse})"/>
        IXmlResponse<TResponse> Xml<TResponse>(Func<Stream, TResponse> deserializeStreamFunction)
            where TResponse : class;

        /// <summary>
        /// Treat <see cref="HttpResponseMessage.Content"/>
        /// as xml and deserialize to provided type.
        /// </summary>
        IXmlResponse<TResponse> Xml<TResponse>(Func<string, TResponse> deserializeStringFunction)
            where TResponse : class;

        /// <inheritdoc cref="HttpContent.ReadAsStringAsync(CancellationToken)"/>
        IStringResponse String();

        /// <inheritdoc cref="HttpContent.ReadAsStreamAsync(CancellationToken)"/>
        IStreamResponse Stream();

        /// <inheritdoc cref="HttpContent.ReadAsByteArrayAsync(CancellationToken)"/>
        IBytesResponse Bytes();
    }
}
