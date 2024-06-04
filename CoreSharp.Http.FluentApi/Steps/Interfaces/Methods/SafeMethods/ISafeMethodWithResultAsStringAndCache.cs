using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsString"/>
public interface ISafeMethodWithResultAsStringAndCache :
    ISafeMethodWithResultAsString,
    ICachedResult<ISafeMethodWithResultAsStringAndCache>
{
}
