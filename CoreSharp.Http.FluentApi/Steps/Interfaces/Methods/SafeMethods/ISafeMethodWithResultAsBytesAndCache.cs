using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsBytes"/>
public interface ISafeMethodWithResultAsBytesAndCache :
    ISafeMethodWithResultAsBytes,
    ICachedResult<ISafeMethodWithResultAsBytesAndCache>
{
}
