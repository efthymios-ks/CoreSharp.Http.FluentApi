using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.HttpClient.FluentApi.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    /// <inheritdoc cref="IJsonResponse{TResponse}"/>
    internal class JsonResponse<TResponse> : GenericResponse<TResponse>, IJsonResponse<TResponse>
        where TResponse : class
    {
        //Constructors
        public JsonResponse(IMethod method, Func<Stream, TResponse> deserializeStreamFunction)
            : this(method)
            => Me.DeserializeStreamFunction = deserializeStreamFunction ?? throw new ArgumentNullException(nameof(deserializeStreamFunction));

        public JsonResponse(IMethod method, Func<string, TResponse> deserializeStringFunction)
            : this(method)
            => Me.DeserializeStringFunction = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

        public JsonResponse(IMethod method) : base(method)
        {
        }

        //Properties 
        private IJsonResponse<TResponse> Me => this;
        Func<Stream, TResponse> IJsonResponse<TResponse>.DeserializeStreamFunction { get; set; }
        Func<string, TResponse> IJsonResponse<TResponse>.DeserializeStringFunction { get; set; }

        //Methods 
        public override async Task<TResponse> SendAsync(CancellationToken cancellationToken = default)
            => await IJsonResponseX.SendAsync(this, cancellationToken);
    }
}
