namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IGenericQueryResponse<TResponse> : IGenericResponse<TResponse> where TResponse : class
    {
    }
}
