using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;

namespace CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsGeneric{TResult}"/>
public class UnsafeMethodWithResultAsGeneric<TResult> :
    UnsafeMethodWithResult,
    IUnsafeMethodWithResultAsGeneric<TResult>
    where TResult : class
{
    // Constructors
    internal UnsafeMethodWithResultAsGeneric(IUnsafeMethod? method, Func<Stream, Task<TResult?>>? deserializeFunction)
        : base(method)
    {
        ArgumentNullException.ThrowIfNull(deserializeFunction);

        Me.DeserializeFunction = deserializeFunction;
    }

    // Properties
    private IUnsafeMethodWithResultAsGeneric<TResult> Me
        => this;
    Func<Stream, Task<TResult?>>? IUnsafeMethodWithResultAsGeneric<TResult>.DeserializeFunction { get; set; }

    // Methods 
    public new async Task<TResult?> SendAsync(CancellationToken cancellationToken = default)
    {
        using var response = await base.SendAsync(cancellationToken);
        if (response is null)
        {
            return null;
        }

        using var buffer = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await Me.DeserializeFunction!(buffer);
    }
}
