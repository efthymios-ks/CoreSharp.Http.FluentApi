namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Results;

public interface ICachableResult<out TContainer>
{
    // Methods
    /// <summary>
    /// Enable in-memory, client-side response caching.
    /// </summary>
    TContainer WithCache(TimeSpan duration);
}
