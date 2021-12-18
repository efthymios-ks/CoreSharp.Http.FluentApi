namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    /// <inheritdoc cref="IGenericQueryResponse{TResponse}"/>
    public abstract class GenericQueryResponseBase<TResponse> : GenericResponse<TResponse>, IGenericQueryResponse<TResponse> where TResponse : class
    {
        //Constructors
        protected GenericQueryResponseBase(IMethod method) : base(method)
        {
        }
    }
}
