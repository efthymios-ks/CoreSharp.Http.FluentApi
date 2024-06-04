using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethod"/>
public interface ISafeMethodWithResultAsBytes :
    ISafeMethod,
    ICachableResult<ISafeMethodWithResultAsBytesAndCache>
{
    // Methods
    /// <inheritdoc cref="IMethod.SendAsync(CancellationToken)"/>
    new Task<byte[]> SendAsync(CancellationToken cancellationToken = default);
}
