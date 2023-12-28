using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultFromJson{TResult}"/>
public interface ISafeMethodWithResultFromJsonAndCache<TResult> :
    ISafeMethodWithResultFromJson<TResult>,
    ICachedResult<ISafeMethodWithResultFromJsonAndCache<TResult>>
    where TResult : class
{
}
