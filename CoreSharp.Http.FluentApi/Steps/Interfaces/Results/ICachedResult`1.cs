namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Results;

public interface ICachedResult<TContainer>
{
    // Properties
    internal TimeSpan CacheDuration { get; set; }
    internal Func<Task<bool>> CacheInvalidationFactory { get; set; }

    // Methods 
    /// <inheritdoc cref="WithCacheInvalidation(Func{Task{bool}})"/>
    TContainer WithCacheInvalidation(Func<bool> cacheInvalidationFactory);

    /// <summary>
    /// Force new request.
    /// </summary> 
    TContainer WithCacheInvalidation(Func<Task<bool>> cacheInvalidationFactory);
}