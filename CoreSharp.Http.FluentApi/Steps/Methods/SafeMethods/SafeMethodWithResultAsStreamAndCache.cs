using CoreSharp.Http.FluentApi.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsStreamAndCache"/>
public sealed class SafeMethodWithResultAsStreamAndCache :
    SafeMethodWithResultAsStream,
    ISafeMethodWithResultAsStreamAndCache
{
    // Constructors
    public SafeMethodWithResultAsStreamAndCache(ISafeMethodWithResultAsStream? method, TimeSpan duration)
        : base(method)
        => Me.CacheDuration = duration;

    // Properties
    private ISafeMethodWithResultAsStreamAndCache Me
        => this;
    TimeSpan ICachedResult<ISafeMethodWithResultAsStreamAndCache>.CacheDuration { get; set; }
    Func<Task<bool>> ICachedResult<ISafeMethodWithResultAsStreamAndCache>.CacheInvalidationFactory { get; set; } = DefaultCacheInvalidationFactory;

    // Methods
    public ISafeMethodWithResultAsStreamAndCache WithCacheInvalidation(Func<bool> cacheInvalidationFactory)
    {
        ArgumentNullException.ThrowIfNull(cacheInvalidationFactory);

        return WithCacheInvalidation(() => Task.FromResult(cacheInvalidationFactory()));
    }

    public ISafeMethodWithResultAsStreamAndCache WithCacheInvalidation(Func<Task<bool>> cacheInvalidationFactory)
    {
        ArgumentNullException.ThrowIfNull(cacheInvalidationFactory);

        Me.CacheInvalidationFactory = cacheInvalidationFactory;
        return this;
    }

    public override async Task<Stream> SendAsync(CancellationToken cancellationToken = default)
    {
        var cacheStorage = Me.Endpoint!.Request!.CacheStorage!;
        var bytes = await cacheStorage.GetOrAddAsync(
            this,
            Me.CacheDuration,
            () => SendAndGetBytesAsync(cancellationToken),
            Me.CacheInvalidationFactory);

        return new MemoryStream(bytes);
    }

    private async Task<byte[]> SendAndGetBytesAsync(CancellationToken cancellationToken)
    {
        var stream = await base.SendAsync(cancellationToken);
        return stream.GetBytes();
    }

    private static Task<bool> DefaultCacheInvalidationFactory()
        => Task.FromResult(false);
}
