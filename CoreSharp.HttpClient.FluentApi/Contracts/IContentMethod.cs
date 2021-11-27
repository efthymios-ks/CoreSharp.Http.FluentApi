using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IContentMethod : IMethod
    {
        //Properties
        internal HttpContent Content { get; set; }
    }
}
