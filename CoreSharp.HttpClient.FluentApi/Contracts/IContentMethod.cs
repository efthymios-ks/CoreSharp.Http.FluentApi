using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IContentMethod : IMethod
    {
        //Properties
        internal HttpContent ContentInternal { get; set; }

        //Methods 
        /// <inheritdoc cref="Content(string, string)" />
        public IContentMethod Content(string content);

        /// <inheritdoc cref="Content(string, Encoding, string)" />
        public IContentMethod Content(string content, string mediaContentType);

        /// <inheritdoc cref="Content(HttpContent)" />
        public IContentMethod Content(string content, Encoding encoding, string mediaContentType);

        /// <inheritdoc cref="Content(object, JsonSerializerSettings)" />
        public IContentMethod Content(object content);

        /// <inheritdoc cref="Content(HttpContent)" />
        public IContentMethod Content(object content, JsonSerializerSettings jsonSerializerSettings);

        /// <summary>
        /// Sets <see cref="HttpRequestMessage.Content"/>.
        /// </summary>
        public IContentMethod Content(HttpContent httpContent);
    }
}
