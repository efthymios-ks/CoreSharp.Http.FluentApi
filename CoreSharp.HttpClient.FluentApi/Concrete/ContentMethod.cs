using CoreSharp.HttpClient.FluentApi.Contracts;
using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    internal class ContentMethod : Method, IContentMethod
    {
        //Constructors 
        public ContentMethod(IRoute route, HttpMethod httpMethod)
            : base(route, httpMethod)
        {
        }

        //Properties  
        HttpContent IContentMethod.Content { get; set; }
    }
}
