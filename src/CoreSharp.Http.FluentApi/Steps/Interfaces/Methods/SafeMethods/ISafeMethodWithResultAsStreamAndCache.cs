using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsStream"/>
public interface ISafeMethodWithResultAsStreamAndCache :
    ISafeMethodWithResultAsStream,
    ICachedResult<ISafeMethodWithResultAsStreamAndCache>
{
}
