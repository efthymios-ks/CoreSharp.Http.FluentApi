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
    public UnsafeMethodWithResultAsGeneric(IUnsafeMethod method, Func<Stream, TResult> deserializeStreamFunction)
        : base(method)
    {
        ArgumentNullException.ThrowIfNull(deserializeStreamFunction);

        Me.DeserializeStreamFunction = deserializeStreamFunction;
    }

    public UnsafeMethodWithResultAsGeneric(IUnsafeMethod method, Func<string, TResult> deserializeStringFunction)
        : base(method)
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        Me.DeserializeStringFunction = deserializeStringFunction;
    }

    protected UnsafeMethodWithResultAsGeneric(IUnsafeMethodWithResultAsGeneric<TResult> method)
        : base(method)
    {
        Me.DeserializeStreamFunction = method.DeserializeStreamFunction;
        Me.DeserializeStringFunction = method.DeserializeStringFunction;
    }

    // Properties
    private IUnsafeMethodWithResultAsGeneric<TResult> Me
        => this;
    Func<Stream, TResult> IUnsafeMethodWithResultAsGeneric<TResult>.DeserializeStreamFunction { get; set; }
    Func<string, TResult> IUnsafeMethodWithResultAsGeneric<TResult>.DeserializeStringFunction { get; set; }

    // Methods 
    public new async Task<TResult> SendAsync(CancellationToken cancellationToken = default)
    {
        using var httpResponseMessage = await base.SendAsync(cancellationToken);
        return await Me.Endpoint.Request.HttpResponseMessageDeserializer.DeserializeAsync(
            httpResponseMessage,
            Me.DeserializeStreamFunction,
            Me.DeserializeStringFunction,
            cancellationToken);
    }
}
