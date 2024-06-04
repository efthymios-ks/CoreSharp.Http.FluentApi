using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsGenericAndCache{TResult}"/>
public sealed class SafeMethodWithResultAsGenericAndCache<TResult> :
    SafeMethodWithResultAsGeneric<TResult>,
    ISafeMethodWithResultAsGenericAndCache<TResult>
    where TResult : class
{
    // Constructors
    public SafeMethodWithResultAsGenericAndCache(ISafeMethodWithResultAsGeneric<TResult>? method, TimeSpan duration)
        : base(method, method?.DeserializeFunction)
        => Me.CacheDuration = duration;

    // Properties
    private ISafeMethodWithResultAsGenericAndCache<TResult> Me
        => this;
    TimeSpan ICachedResult<ISafeMethodWithResultAsGenericAndCache<TResult>>.CacheDuration { get; set; }
    Func<Task<bool>> ICachedResult<ISafeMethodWithResultAsGenericAndCache<TResult>>.CacheInvalidationFactory { get; set; } = DefaultCacheInvalidationFactory;

    // Methods
    public ISafeMethodWithResultAsGenericAndCache<TResult> WithCacheInvalidation(Func<bool> cacheInvalidationFactory)
    {
        ArgumentNullException.ThrowIfNull(cacheInvalidationFactory);

        return WithCacheInvalidation(() => Task.FromResult(cacheInvalidationFactory()));
    }

    public ISafeMethodWithResultAsGenericAndCache<TResult> WithCacheInvalidation(Func<Task<bool>> cacheInvalidationFactory)
    {
        ArgumentNullException.ThrowIfNull(cacheInvalidationFactory);

        Me.CacheInvalidationFactory = cacheInvalidationFactory;
        return this;
    }

    public override Task<TResult?> SendAsync(CancellationToken cancellationToken = default)
        => Me.Endpoint!.Request!.CacheStorage!.GetOrAddAsync(
            this,
            Me.CacheDuration,
            () => base.SendAsync(cancellationToken),
            Me.CacheInvalidationFactory);

    private static Task<bool> DefaultCacheInvalidationFactory()
        => Task.FromResult(false);
}
