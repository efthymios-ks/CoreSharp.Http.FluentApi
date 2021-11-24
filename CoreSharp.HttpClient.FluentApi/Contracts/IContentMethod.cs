using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IContentMethod : IMethod
    {
        //Properties
        public HttpContent Content { get; internal set; }
    }
}
