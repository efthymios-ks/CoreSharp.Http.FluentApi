using CoreSharp.HttpClient.FluentApi.Concrete;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    /// <inheritdoc cref="IGenericResponse{TResponse}"/>
    public abstract class GenericResponse<TResponse> : Response, IGenericResponse<TResponse> where TResponse : class
    {
        //Constructors
        protected GenericResponse(IMethod method) : base(method)
        {
        }

        //Methods
        public new abstract Task<TResponse> SendAsync(CancellationToken cancellationToken = default);
    }
}
