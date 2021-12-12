using CoreSharp.HttpClient.FluentApi.Contracts;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    /// <inheritdoc cref="IGenericQueryResponse{TResponse}"/>
    internal class GenericQueryResponse<TResponse> : GenericResponse<TResponse>, IGenericQueryResponse<TResponse> where TResponse : class
    {
        //Constructors
        public GenericQueryResponse(IQueryMethod queryMethod) : base(queryMethod)
        {
        }
    }
}
