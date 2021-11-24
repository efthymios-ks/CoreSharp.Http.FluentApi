using CoreSharp.Models;
using System.Collections.Generic;
using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IRequest
    {
        //Properties
        /// <inheritdoc cref="System.Net.Http.HttpClient" />
        internal System.Net.Http.HttpClient HttpClient { get; set; }

        /// <inheritdoc cref="HttpCompletionOption"/>
        internal HttpCompletionOption CompletionOption { get; set; }

        /// <inheritdoc cref="HttpRequestMessage.Headers" />
        internal IDictionary<string, string> Headers { get; }

        /// <summary>
        /// Whether the interface throws an <see cref="HttpResponseException"/>
        /// or not on not successful requests using <see cref="HttpResponseMessage.StatusCode"/>.
        /// </summary>
        internal bool ThrowOnError { get; set; }
    }
}
