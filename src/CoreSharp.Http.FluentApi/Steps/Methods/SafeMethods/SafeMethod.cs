using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.Abstracts;
using System.Net.Http;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethod"/>
public class SafeMethod : MethodBase, ISafeMethod
{
    // Constructors
    public SafeMethod(IMethod method)
        : this(method?.Endpoint, method?.HttpMethod)
    {
    }

    public SafeMethod(IEndpoint endpoint, HttpMethod httpMethod)
        : base(endpoint, httpMethod)
    {
    }
}
