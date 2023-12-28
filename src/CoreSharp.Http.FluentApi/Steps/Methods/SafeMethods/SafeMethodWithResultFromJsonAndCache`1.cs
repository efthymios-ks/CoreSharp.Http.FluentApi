using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;
using System;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultFromJsonAndCache{TResult}"/>
public sealed class SafeMethodWithResultFromJsonAndCache<TResult> :
    SafeMethodWithResultFromJson<TResult>,
    ISafeMethodWithResultFromJsonAndCache<TResult>
    where TResult : class
{
    // Constructors
    public SafeMethodWithResultFromJsonAndCache(ISafeMethodWithResultFromJson<TResult> method, TimeSpan duration)
        : base(method)
        => Me.CacheDuration = duration;

    // Properties
    private ISafeMethodWithResultFromJsonAndCache<TResult> Me
        => this;
    TimeSpan ICachedResult<ISafeMethodWithResultFromJsonAndCache<TResult>>.CacheDuration { get; set; }
    Func<Task<bool>> ICachedResult<ISafeMethodWithResultFromJsonAndCache<TResult>>.CacheInvalidationFactory { get; set; }

    // Methods
    public ISafeMethodWithResultFromJsonAndCache<TResult> WithCacheInvalidation(Func<bool> cacheInvalidationFactory)
    {
        ArgumentNullException.ThrowIfNull(cacheInvalidationFactory);

        return WithCacheInvalidation(() => Task.FromResult(cacheInvalidationFactory()));
    }

    public ISafeMethodWithResultFromJsonAndCache<TResult> WithCacheInvalidation(Func<Task<bool>> cacheInvalidationFactory)
    {
        ArgumentNullException.ThrowIfNull(cacheInvalidationFactory);

        Me.CacheInvalidationFactory = cacheInvalidationFactory;
        return this;
    }
}
