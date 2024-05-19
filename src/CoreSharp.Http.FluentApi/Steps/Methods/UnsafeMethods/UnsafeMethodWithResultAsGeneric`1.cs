using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsGeneric{TResult}"/>
public class UnsafeMethodWithResultAsGeneric<TResult> :
    UnsafeMethodWithResult,
    IUnsafeMethodWithResultAsGeneric<TResult>
    where TResult : class
{
    // Constructors
    public UnsafeMethodWithResultAsGeneric(IUnsafeMethod method, Func<Stream, Task<TResult>> deserializeSFunction)
        : base(method)
    {
        ArgumentNullException.ThrowIfNull(deserializeSFunction);

        Me.DeserializeFunction = deserializeSFunction;
    }

    // Properties
    private IUnsafeMethodWithResultAsGeneric<TResult> Me
        => this;
    Func<Stream, Task<TResult>> IUnsafeMethodWithResultAsGeneric<TResult>.DeserializeFunction { get; set; }

    // Methods 
    public new async Task<TResult> SendAsync(CancellationToken cancellationToken = default)
    {
        using var response = await base.SendAsync(cancellationToken);
        using var buffer = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await Me.DeserializeFunction(buffer);
    }
}
