using CoreSharp.HttpClient.FluentApi.Utilities;
using System.Collections.Generic;
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
    }
}
