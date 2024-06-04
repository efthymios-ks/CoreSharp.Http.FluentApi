using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethod"/>
public interface ISafeMethodWithResultAsString :
    ISafeMethod,
    ICachableResult<ISafeMethodWithResultAsStringAndCache>
{
    // Methods
    /// <inheritdoc cref="IMethod.SendAsync(CancellationToken)"/>
    new Task<string?> SendAsync(CancellationToken cancellationToken = default);
}
