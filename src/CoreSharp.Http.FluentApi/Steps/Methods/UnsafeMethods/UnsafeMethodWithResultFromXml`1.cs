using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using CoreSharp.Http.FluentApi.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultFromXml{TResult}"/>
public class UnsafeMethodWithResultFromXml<TResult> :
    UnsafeMethodWithResult,
    IUnsafeMethodWithResultFromXml<TResult>
    where TResult : class
{
    // Constructors
    public UnsafeMethodWithResultFromXml(IUnsafeMethod method, Func<Stream, TResult> deserializeStreamFunction)
        : base(method)
    {
        ArgumentNullException.ThrowIfNull(deserializeStreamFunction);

        Me.DeserializeStreamFunction = deserializeStreamFunction;
    }

    public UnsafeMethodWithResultFromXml(IUnsafeMethod method, Func<string, TResult> deserializeStringFunction)
        : base(method)
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        Me.DeserializeStringFunction = deserializeStringFunction;
    }

    protected UnsafeMethodWithResultFromXml(IUnsafeMethodWithResultFromXml<TResult> method)
        : base(method)
    {
        Me.DeserializeStreamFunction = method.DeserializeStreamFunction;
        Me.DeserializeStringFunction = method.DeserializeStringFunction;
    }

    // Properties
    private IUnsafeMethodWithResultFromXml<TResult> Me
        => this;
    Func<Stream, TResult> IUnsafeMethodWithResultFromXml<TResult>.DeserializeStreamFunction { get; set; }
    Func<string, TResult> IUnsafeMethodWithResultFromXml<TResult>.DeserializeStringFunction { get; set; }

    // Methods 
    async Task<TResult> IUnsafeMethodWithResultFromXml<TResult>.SendAsync(CancellationToken cancellationToken)
    {
        using var httpResponseMessage = await base.SendAsync(cancellationToken);
        return await HttpResponseMessageUtils.DeserialeAsync(
            httpResponseMessage,
            Me.DeserializeStreamFunction,
            Me.DeserializeStringFunction,
            cancellationToken);
    }
}