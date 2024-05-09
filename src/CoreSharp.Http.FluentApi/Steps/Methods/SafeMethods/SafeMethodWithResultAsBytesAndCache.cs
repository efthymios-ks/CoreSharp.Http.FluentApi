using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsBytesAndCache"/>
public sealed class SafeMethodWithResultAsBytesAndCache :
    SafeMethodWithResultAsBytes,
    ISafeMethodWithResultAsBytesAndCache
{
    // Constructors
    public SafeMethodWithResultAsBytesAndCache(ISafeMethodWithResultAsBytes method, TimeSpan duration)
        : base(method)
        => Me.CacheDuration = duration;

    // Properties
    private ISafeMethodWithResultAsBytesAndCache Me
        => this;
    TimeSpan ICachedResult<ISafeMethodWithResultAsBytesAndCache>.CacheDuration { get; set; }
    Func<Task<bool>> ICachedResult<ISafeMethodWithResultAsBytesAndCache>.CacheInvalidationFactory { get; set; } = DefaultCacheInvalidationFactory;

    // Methods
    public ISafeMethodWithResultAsBytesAndCache WithCacheInvalidation(Func<bool> cacheInvalidationFactory)
    {
        ArgumentNullException.ThrowIfNull(cacheInvalidationFactory);

        return WithCacheInvalidation(() => Task.FromResult(cacheInvalidationFactory()));
    }

    public ISafeMethodWithResultAsBytesAndCache WithCacheInvalidation(Func<Task<bool>> cacheInvalidationFactory)
    {
        ArgumentNullException.ThrowIfNull(cacheInvalidationFactory);

        Me.CacheInvalidationFactory = cacheInvalidationFactory;
        return this;
    }

    public override Task<byte[]> SendAsync(CancellationToken cancellationToken = default)
        => Me.Endpoint.Request.CacheStorage.GetOrAddResultAsync(
            this,
            Me.CacheDuration,
            () => base.SendAsync(cancellationToken),
            Me.CacheInvalidationFactory);

    private static Task<bool> DefaultCacheInvalidationFactory()
        => Task.FromResult(false);
}
