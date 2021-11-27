using System.Collections.Generic;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IQueryMethod : IMethod
    {
        //Properties 
        internal IDictionary<string, object> QueryParameters { get; }
    }
}
