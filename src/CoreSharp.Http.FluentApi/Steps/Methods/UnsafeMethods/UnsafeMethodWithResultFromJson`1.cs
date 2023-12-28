using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultFromJson{TResult}"/>
public class UnsafeMethodWithResultFromJson<TResult> :
    UnsafeMethodWithResult,
    IUnsafeMethodWithResultFromJson<TResult>
    where TResult : class
{
    // Constructors
    public UnsafeMethodWithResultFromJson(IUnsafeMethod method, Func<Stream, TResult> deserializeStreamFunction)
        : base(method)
    {
        ArgumentNullException.ThrowIfNull(deserializeStreamFunction);

        Me.DeserializeStreamFunction = deserializeStreamFunction;
    }

    public UnsafeMethodWithResultFromJson(IUnsafeMethod method, Func<string, TResult> deserializeStringFunction)
        : base(method)
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        Me.DeserializeStringFunction = deserializeStringFunction;
    }

    protected UnsafeMethodWithResultFromJson(IUnsafeMethodWithResultFromJson<TResult> method)
        : base(method)
    {
        Me.DeserializeStreamFunction = method.DeserializeStreamFunction;
        Me.DeserializeStringFunction = method.DeserializeStringFunction;
    }

    // Properties
    private IUnsafeMethodWithResultFromJson<TResult> Me
        => this;
    Func<Stream, TResult> IUnsafeMethodWithResultFromJson<TResult>.DeserializeStreamFunction { get; set; }
    Func<string, TResult> IUnsafeMethodWithResultFromJson<TResult>.DeserializeStringFunction { get; set; }

    // Methods 
    async Task<TResult> IUnsafeMethodWithResultFromJson<TResult>.SendAsync(CancellationToken cancellationToken)
    {
        using var httpResponseMessage = await base.SendAsync(cancellationToken);
        return await HttpResponseMessageUtils.DeserialeAsync(
            httpResponseMessage,
            Me.DeserializeStreamFunction,
            Me.DeserializeStringFunction,
            cancellationToken);
    }
}
