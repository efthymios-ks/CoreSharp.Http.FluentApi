using CoreSharp.HttpClient.FluentApi.Contracts;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    /// <inheritdoc cref="IResponse" />
    public class Response : IResponse
    {
        //Constructors 
        protected Response(IMethod method)
            => Me.Method = method ?? throw new ArgumentNullException(nameof(method));

        //Properties
        private IResponse Me => this;
        IMethod IResponse.Method { get; set; }

        //Methods 
        public async Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default)
            => await Me.Method.SendAsync(cancellationToken);
    }
}
