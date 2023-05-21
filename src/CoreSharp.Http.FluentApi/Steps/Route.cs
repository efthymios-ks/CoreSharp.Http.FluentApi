using CoreSharp.Http.FluentApi.Steps.Interfaces;
using System;
using System.Net.Http;

namespace CoreSharp.Http.FluentApi.Steps;

/// <inheritdoc cref="IRoute"/>
internal sealed class Route : IRoute
{
    // Constructors  
    public Route(IRequest request, string resourceName)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(resourceName);

        var me = Me;
        me.Request = request;
        me.Route = resourceName;
    }

    // Properties 
    private IRoute Me
        => this;

    IRequest IRoute.Request { get; set; }

    string IRoute.Route { get; set; }

    // Methods 
    public IQueryMethod Get()
        => new QueryMethod(this, HttpMethod.Get);

    public IContentMethod Post()
        => new ContentMethod(this, HttpMethod.Post);

    public IContentMethod Put()
        => new ContentMethod(this, HttpMethod.Put);

    public IContentMethod Patch()
        => new ContentMethod(this, HttpMethod.Patch);

    public IMethod Delete()
        => new Method(this, HttpMethod.Delete);

    public IMethod Head()
        => new Method(this, HttpMethod.Head);

    public IMethodWithResponse Options()
        => new MethodWithResponse(this, HttpMethod.Options);

    public IMethod Trace()
        => new Method(this, HttpMethod.Trace);
}
