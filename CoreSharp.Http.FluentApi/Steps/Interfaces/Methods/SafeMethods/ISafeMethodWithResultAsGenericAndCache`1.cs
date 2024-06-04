using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsGeneric{TResult}"/>
public interface ISafeMethodWithResultAsGenericAndCache<TResult> :
    ISafeMethodWithResultAsGeneric<TResult>,
    ICachedResult<ISafeMethodWithResultAsGenericAndCache<TResult>>
    where TResult : class
{
}
