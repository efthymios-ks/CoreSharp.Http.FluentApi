using System.Net.Http;
using System.Text;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IContentMethod : IMethod
    {
        //Properties
        internal HttpContent ContentInternal { get; set; }

        //Methods
        /// <inheritdoc cref="Content(string, Encoding, string)" />
        public IContentMethod Content(string content, string mediaTypeName);

        /// <inheritdoc cref="Content(HttpContent)" />
        public IContentMethod Content(string content, Encoding encoding, string mediaTypeName);

        /// <inheritdoc cref="Content(string, string)" />
        public IContentMethod JsonContent(string content);

        /// <inheritdoc cref="Content(HttpContent)" />
        public IContentMethod JsonContent(object content);

        /// <inheritdoc cref="Content(string, string)" />
        public IContentMethod XmlContent(string content);

        /// <inheritdoc cref="Content(HttpContent)" />
        public IContentMethod XmlContent(object content);

        /// <summary>
        /// Sets <see cref="HttpRequestMessage.Content"/>.
        /// </summary>
        public IContentMethod Content(HttpContent httpContent);
    }
}
