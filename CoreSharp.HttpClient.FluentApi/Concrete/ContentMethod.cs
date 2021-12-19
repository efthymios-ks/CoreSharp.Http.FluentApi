using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.HttpClient.FluentApi.Utilities;
using Newtonsoft.Json;
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
        //Constructors 
        public ContentMethod(IRoute route, HttpMethod httpMethod) : base(route, httpMethod)
            => HttpMethodX.ValidateContentMethod(httpMethod);

        //Properties 
        private IContentMethod Me => this;
        HttpContent IContentMethod.ContentInternal { get; set; }

        //Methods 
        public override async Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default)
            => await IMethodX.SendAsync(this, httpContent: Me.ContentInternal, cancellationToken: cancellationToken);

        public IContentMethod Content(string content, string mediaTypeName)
            => Content(content, Encoding.UTF8, mediaTypeName);

        public IContentMethod Content(string content, Encoding encoding, string mediaTypeName)
            => Content(new StringContent(content, encoding, mediaTypeName));

        public IContentMethod JsonContent(string content)
            => Content(content, MediaTypeNames.Application.Json);

        public IContentMethod JsonContent(object content)
        {
            if (content is string json)
                JsonContent(json);
            else if (content is not null)
                Content(ToStreamContent(content));
            return this;
        }

        public IContentMethod Content(HttpContent httpContent)
        {
            Me.ContentInternal = httpContent;
            return this;
        }

        //Private 
        /// <summary>
        /// Build <see cref="StreamContent" /> from given item.
        /// </summary>
        private static StreamContent ToStreamContent(
            object content,
            int bufferSize = 4096)
        {
            var serializer = JsonSerializer.Create();
            var stream = new MemoryStream();
            using var streamWriter = new StreamWriter(stream, Encoding.UTF8, bufferSize, true);
            using var jsonWriter = new JsonTextWriter(streamWriter);

            serializer.Serialize(jsonWriter, content);
            streamWriter.Flush();
            stream.Position = 0;

            var streamContent = new StreamContent(stream, bufferSize);
            streamContent.Headers.ContentType = new(MediaTypeNames.Application.Json);
            return streamContent;
        }
    }
}
