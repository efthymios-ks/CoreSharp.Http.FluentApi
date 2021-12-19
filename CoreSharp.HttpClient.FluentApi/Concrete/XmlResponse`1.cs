using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.HttpClient.FluentApi.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    /// <inheritdoc cref="IXmlResponse{TResponse}"/>
    internal class XmlResponse<TResponse> : GenericResponse<TResponse>, IXmlResponse<TResponse>
        where TResponse : class
    {
        //Constructors
        public XmlResponse(IMethod method, Func<string, TResponse> deserializeStringFunction)
            : this(method)
            => Me.DeserializeStringFunction = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

        public XmlResponse(IMethod method) : base(method)
        {
        }

        //Properties 
        private IXmlResponse<TResponse> Me => this;
        Func<string, TResponse> IXmlResponse<TResponse>.DeserializeStringFunction { get; set; }

        //Methods 
        public override async Task<TResponse> SendAsync(CancellationToken cancellationToken = default)
            => await IXmlResponseX.SendAsync(this, cancellationToken);
    }
}
