namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IQueryMethod : IMethod
    {
        //Properties
        public string QueryParameter { get; internal set; }
    }
}
