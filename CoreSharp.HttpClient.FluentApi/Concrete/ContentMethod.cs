using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.HttpClient.FluentApi.Utilities;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    /// <inheritdoc cref="IContentMethod"/>
    internal class ContentMethod : Method, IContentMethod
    {
        //Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const int DefaultBufferSize = 4096;

        //Constructors 
        public ContentMethod(IRoute route, HttpMethod httpMethod)
            : base(route, httpMethod)
            => HttpMethodX.ValidateContentMethod(httpMethod);

        //Properties 
        private IContentMethod Me
            => this;

        HttpContent IContentMethod.HttpContent { get; set; }

        //Methods 
        public override async Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default)
            => await IMethodX.SendAsync(this, httpContent: Me.HttpContent, cancellationToken: cancellationToken);

        public IContentMethod JsonContent(string json)
            => Content(json, MediaTypeNames.Application.Json);

        public IContentMethod JsonContent(Stream stream)
        {
            _ = stream ?? throw new ArgumentNullException(nameof(stream));

            var content = ToJsonStreamContent(stream);
            Content(content);
            return this;
        }

        public IContentMethod JsonContent(object entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var content = ToJsonStreamContent(entity);
            Content(content);
            return this;
        }

        public IContentMethod XmlContent(string xml)
            => Content(xml, MediaTypeNames.Application.Xml);

        public IContentMethod XmlContent(Stream stream)
        {
            _ = stream ?? throw new ArgumentNullException(nameof(stream));

            var content = ToXmlStreamContent(stream);
            Content(content);
            return this;
        }

        public IContentMethod XmlContent(object entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var xml = entity.ToXml();
            XmlContent(xml);
            return this;
        }

        public IContentMethod Content(string content, string mediaTypeName)
            => Content(content, Encoding.UTF8, mediaTypeName);

        public IContentMethod Content(string content, Encoding encoding, string mediaTypeName)
            => Content(new StringContent(content, encoding, mediaTypeName));

        public IContentMethod Content(HttpContent httpContent)
        {
            Me.HttpContent = httpContent;
            return this;
        }

        //Private 
        /// <summary>
        /// Convert object to json <see cref="Stream"/>.
        /// </summary>
        private static Stream ToJsonStream(object entity, int bufferSize = DefaultBufferSize)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var serializer = JsonSerializer.Create();
            var stream = new MemoryStream();
            using var streamWriter = new StreamWriter(stream, Encoding.UTF8, bufferSize, true);
            using var jsonWriter = new JsonTextWriter(streamWriter);

            serializer.Serialize(jsonWriter, entity);
            streamWriter.Flush();
            if (stream.CanSeek)
                stream.Position = 0;

            return stream;
        }

        /// <summary>
        /// Treats entity as json and converts
        /// to <see cref="StreamContent" />
        /// with <see cref="MediaTypeNames.Application.Json"/>.
        /// </summary>
        private static StreamContent ToJsonStreamContent(object entity, int bufferSize = DefaultBufferSize)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var stream = ToJsonStream(entity);
            return ToJsonStreamContent(stream, bufferSize);
        }

        /// <summary>
        /// Create <see cref="StreamContent"/>
        /// from given <see cref="Stream"/>
        /// with <see cref="MediaTypeNames.Application.Json"/>.
        /// </summary>
        private static StreamContent ToJsonStreamContent(Stream stream, int bufferSize = DefaultBufferSize)
        {
            _ = stream ?? throw new ArgumentNullException(nameof(stream));

            return ToStreamContent(stream, MediaTypeNames.Application.Json, bufferSize);
        }

        /// <summary>
        /// Create <see cref="StreamContent"/>
        /// from given <see cref="Stream"/>
        /// with <see cref="MediaTypeNames.Application.Xml"/>.
        /// </summary>
        private static StreamContent ToXmlStreamContent(Stream stream, int bufferSize = DefaultBufferSize)
        {
            _ = stream ?? throw new ArgumentNullException(nameof(stream));

            return ToStreamContent(stream, MediaTypeNames.Application.Xml, bufferSize);
        }

        /// <summary>
        /// Create <see cref="StreamContent"/>
        /// from given <see cref="Stream"/>
        /// with specified <see cref="ContentType"/>.
        /// </summary>
        private static StreamContent ToStreamContent(Stream stream, string mediaTypeName, int bufferSize = DefaultBufferSize)
        {
            _ = stream ?? throw new ArgumentNullException(nameof(stream));
            if (string.IsNullOrWhiteSpace(mediaTypeName))
                throw new ArgumentNullException(nameof(mediaTypeName));

            var streamContent = new StreamContent(stream, bufferSize);
            streamContent.Headers.ContentType = new(mediaTypeName);
            return streamContent;
        }
    }
}
