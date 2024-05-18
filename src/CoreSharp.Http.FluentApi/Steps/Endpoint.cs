using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace CoreSharp.Http.FluentApi.Steps;

/// <inheritdoc cref="IEndpoint"/>
public sealed class Endpoint : IEndpoint
{
    // Constructors  
    public Endpoint(IRequest request, string resourceName)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(resourceName);

        var me = Me;
        me.Request = request;
        me.Endpoint = resourceName;
    }

    // Properties 
    private IEndpoint Me
        => this;
    IRequest IEndpoint.Request { get; set; }
    string IEndpoint.Endpoint { get; set; }
    IDictionary<string, string> IEndpoint.QueryParameters { get; }
        = new Dictionary<string, string>();

    // Methods 
    public IEndpoint WithQuery(IDictionary<string, object> parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        foreach (var (key, value) in parameters)
        {
            Me.WithQuery(key, value);
        }

        return this;
    }

    public IEndpoint WithQuery<TQueryParameter>(TQueryParameter queryParameter)
        where TQueryParameter : class
    {
        ArgumentNullException.ThrowIfNull(queryParameter);

        if (queryParameter is not IDictionary<string, object> parameters)
        {
            parameters = queryParameter.GetPropertiesDictionary();
        }

        return Me.WithQuery(parameters);
    }

    public IEndpoint WithQuery(string key, object value)
    {
        Me.QueryParameters[key] = value?.ToString();
        return this;
    }

    public ISafeMethodWithResult Get()
        => new SafeMethodWithResult(new SafeMethod(this, HttpMethod.Get));

    public IUnsafeMethodWithResult Post()
        => new UnsafeMethodWithResult(new UnsafeMethod(this, HttpMethod.Post));

    public IUnsafeMethodWithResult Put()
        => new UnsafeMethodWithResult(new UnsafeMethod(this, HttpMethod.Put));

    public IUnsafeMethodWithResult Patch()
        => new UnsafeMethodWithResult(new UnsafeMethod(this, HttpMethod.Patch));

    public IUnsafeMethodWithResult Delete()
        => new UnsafeMethodWithResult(new UnsafeMethod(this, HttpMethod.Delete));

    public ISafeMethodWithResult Head()
        => new SafeMethodWithResult(new SafeMethod(this, HttpMethod.Head));

    public ISafeMethodWithResult Options()
        => new SafeMethodWithResult(new SafeMethod(this, HttpMethod.Options));

    public ISafeMethodWithResult Trace()
        => new SafeMethodWithResult(new SafeMethod(this, HttpMethod.Trace));
}
