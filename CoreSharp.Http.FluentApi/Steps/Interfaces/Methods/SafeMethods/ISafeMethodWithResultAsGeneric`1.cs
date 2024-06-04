using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethod"/>
public interface ISafeMethodWithResultAsGeneric<TResult> :
    ISafeMethod,
    ICachableResult<ISafeMethodWithResultAsGenericAndCache<TResult>>
    where TResult : class
{
    // Properties 
    internal Func<Stream, Task<TResult?>>? DeserializeFunction { get; set; }

    // Methods
    /// <inheritdoc cref="IMethod.SendAsync(CancellationToken)"/>
    new Task<TResult?> SendAsync(CancellationToken cancellationToken = default);
}
