using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultFromJson{TResult}"/>
public class SafeMethodWithResultFromJson<TResult> :
    SafeMethodWithResult,
    ISafeMethodWithResultFromJson<TResult>
    where TResult : class
{
    // Constructors
    public SafeMethodWithResultFromJson(ISafeMethod method, Func<Stream, TResult> deserializeStreamFunction)
        : base(method)
    {
        ArgumentNullException.ThrowIfNull(deserializeStreamFunction);

        Me.DeserializeStreamFunction = deserializeStreamFunction;
    }

    public SafeMethodWithResultFromJson(ISafeMethod method, Func<string, TResult> deserializeStringFunction)
        : base(method)
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        Me.DeserializeStringFunction = deserializeStringFunction;
    }

    protected SafeMethodWithResultFromJson(ISafeMethodWithResultFromJson<TResult> method)
        : base(method)
    {
        Me.DeserializeStreamFunction = method.DeserializeStreamFunction;
        Me.DeserializeStringFunction = method.DeserializeStringFunction;
    }

    // Properties
    private ISafeMethodWithResultFromJson<TResult> Me
        => this;
    Func<Stream, TResult> ISafeMethodWithResultFromJson<TResult>.DeserializeStreamFunction { get; set; }
    Func<string, TResult> ISafeMethodWithResultFromJson<TResult>.DeserializeStringFunction { get; set; }

    // Methods
    public ISafeMethodWithResultFromJsonAndCache<TResult> WithCache(TimeSpan duration)
        => new SafeMethodWithResultFromJsonAndCache<TResult>(this, duration);

    async Task<TResult> ISafeMethodWithResultFromJson<TResult>.SendAsync(CancellationToken cancellationToken)
    {
        using var httpResponseMessage = await base.SendAsync(cancellationToken);
        return await HttpResponseMessageUtils.DeserialeAsync(
            httpResponseMessage,
            Me.DeserializeStreamFunction,
            Me.DeserializeStringFunction,
            cancellationToken);
    }
}
