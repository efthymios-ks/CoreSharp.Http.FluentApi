namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IGenericResponse<TResponse> where TResponse : class
    {
        //Properties
        internal IMethod Method { get; set; }
    }
}
