using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using CoreSharp.Http.FluentApi.Utilities;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.Abstracts;

/// <inheritdoc cref="IMethod"/>
public abstract class MethodBase : IMethod
{
    // Constructors 
    protected MethodBase(IEndpoint endpoint, HttpMethod httpMethod)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        ArgumentNullException.ThrowIfNull(httpMethod);

        Me.Endpoint = endpoint;
        Me.HttpMethod = httpMethod;
    }

    // Properties 
    private IMethod Me
        => this;
    IEndpoint IMethod.Endpoint { get; set; }
    HttpMethod IMethod.HttpMethod { get; set; }

    // Methods
    public virtual Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default)
        => IMethodUtils.SendAsync(this, httpContent: null, cancellationToken: cancellationToken);
}
