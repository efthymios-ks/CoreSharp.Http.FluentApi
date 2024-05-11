using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsStringAndCache"/>
public sealed class SafeMethodWithResultAsStringAndCache :
    SafeMethodWithResultAsString,
    ISafeMethodWithResultAsStringAndCache
{
    // Constructors
    public SafeMethodWithResultAsStringAndCache(ISafeMethodWithResultAsString method, TimeSpan duration)
        : base(method)
        => Me.CacheDuration = duration;

    // Properties
    private ISafeMethodWithResultAsStringAndCache Me
        => this;
    TimeSpan ICachedResult<ISafeMethodWithResultAsStringAndCache>.CacheDuration { get; set; }
    Func<Task<bool>> ICachedResult<ISafeMethodWithResultAsStringAndCache>.CacheInvalidationFactory { get; set; } = DefaultCacheInvalidationFactory;

    // Methods
    public ISafeMethodWithResultAsStringAndCache WithCacheInvalidation(Func<bool> cacheInvalidationFactory)
    {
        ArgumentNullException.ThrowIfNull(cacheInvalidationFactory);

        return WithCacheInvalidation(() => Task.FromResult(cacheInvalidationFactory()));
    }

    public ISafeMethodWithResultAsStringAndCache WithCacheInvalidation(Func<Task<bool>> cacheInvalidationFactory)
    {
        ArgumentNullException.ThrowIfNull(cacheInvalidationFactory);

        Me.CacheInvalidationFactory = cacheInvalidationFactory;
        return this;
    }

    public override Task<string> SendAsync(CancellationToken cancellationToken = default)
        => Me.Endpoint.Request.CacheStorage.GetOrAddResultAsync(
            this,
            Me.CacheDuration,
            () => base.SendAsync(cancellationToken),
            Me.CacheInvalidationFactory);

    private static Task<bool> DefaultCacheInvalidationFactory()
        => Task.FromResult(false);
}
