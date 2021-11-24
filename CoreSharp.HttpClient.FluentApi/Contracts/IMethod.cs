using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IMethod
    {
        //Properties
        internal IRoute Resource { get; set; }
        internal HttpMethod HttpMethod { get; set; }
    }
}
