using CoreSharp.HttpClient.FluentApi.Contracts;
using System.Collections.Generic;
using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    internal class QueryMethod : Method, IQueryMethod
    {
        //Constructors 
        public QueryMethod(IRoute route, HttpMethod httpMethod)
            : base(route, httpMethod)
        {
            HttpMethodX.ValidateQueryMethod(httpMethod);
        }

        //Properties  
        IDictionary<string, object> IQueryMethod.QueryParameters { get; } = new Dictionary<string, object>();
    }
}
