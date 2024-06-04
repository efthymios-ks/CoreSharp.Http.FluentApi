namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;

/// <inheritdoc cref="IUnsafeMethod"/>
public interface IUnsafeMethodWithResultAsBytes : IUnsafeMethod
{
    // Methods
    /// <inheritdoc cref="IMethod.SendAsync(CancellationToken)"/>
    new Task<byte[]> SendAsync(CancellationToken cancellationToken = default);
}
