using CoreSharp.Http.FluentApi.Contracts;
using CoreSharp.Http.FluentApi.Utilities;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Concrete;

/// <inheritdoc cref="IMethod"/>
internal class Method : IMethod
{
    //Constructors 
    public Method(IRoute route, HttpMethod httpMethod)
    {
        _ = route ?? throw new ArgumentNullException(nameof(route));
        _ = httpMethod ?? throw new ArgumentNullException(nameof(httpMethod));

        Me.Route = route;
        Me.HttpMethod = httpMethod;
    }

    //Properties 
    private IMethod Me
        => this;

    IRoute IMethod.Route { get; set; }

    HttpMethod IMethod.HttpMethod { get; set; }

    //Methods 
    public virtual async Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default)
        => await IMethodX.SendAsync(this, cancellationToken: cancellationToken);
}
