using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;
using System;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultFromXmlAndCache{TResult}"/>
public sealed class SafeMethodWithResultFromXmlAndCache<TResult> :
    SafeMethodWithResultFromXml<TResult>,
    ISafeMethodWithResultFromXmlAndCache<TResult>
    where TResult : class
{
    // Constructors
    public SafeMethodWithResultFromXmlAndCache(ISafeMethodWithResultFromXml<TResult> method, TimeSpan duration)
        : base(method)
        => Me.CacheDuration = duration;

    // Properties
    private ISafeMethodWithResultFromXmlAndCache<TResult> Me
        => this;
    TimeSpan ICachedResult<ISafeMethodWithResultFromXmlAndCache<TResult>>.CacheDuration { get; set; }
    Func<Task<bool>> ICachedResult<ISafeMethodWithResultFromXmlAndCache<TResult>>.CacheInvalidationFactory { get; set; }

    // Methods
    public ISafeMethodWithResultFromXmlAndCache<TResult> WithCacheInvalidation(Func<bool> cacheInvalidationFactory)
    {
        ArgumentNullException.ThrowIfNull(cacheInvalidationFactory);

        return WithCacheInvalidation(() => Task.FromResult(cacheInvalidationFactory()));
    }

    public ISafeMethodWithResultFromXmlAndCache<TResult> WithCacheInvalidation(Func<Task<bool>> cacheInvalidationFactory)
    {
        ArgumentNullException.ThrowIfNull(cacheInvalidationFactory);

        Me.CacheInvalidationFactory = cacheInvalidationFactory;
        return this;
    }
}