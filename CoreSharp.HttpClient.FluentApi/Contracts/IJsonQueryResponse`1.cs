namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IJsonQueryResponse<TResponse> : IJsonResponse<TResponse>, ICacheQueryResponse<TResponse>
        where TResponse : class
    {
    }
}
