namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IRoute
    {
        //Properties 
        internal IRequest Request { get; set; }
        internal string Route { get; set; }
    }
}
