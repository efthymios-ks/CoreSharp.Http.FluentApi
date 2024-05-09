using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsGeneric{TResult}"/>
public class SafeMethodWithResultAsGeneric<TResult> :
    SafeMethodWithResult,
    ISafeMethodWithResultAsGeneric<TResult>
    where TResult : class
{
    // Constructors
    public SafeMethodWithResultAsGeneric(ISafeMethod safeMethod, Func<Stream, TResult> deserializeStreamFunction)
        : base(safeMethod)
    {
        ArgumentNullException.ThrowIfNull(deserializeStreamFunction);

        Me.DeserializeStreamFunction = deserializeStreamFunction;
    }

    public SafeMethodWithResultAsGeneric(ISafeMethod safeMethod, Func<string, TResult> deserializeStringFunction)
        : base(safeMethod)
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        Me.DeserializeStringFunction = deserializeStringFunction;
    }

    protected SafeMethodWithResultAsGeneric(ISafeMethodWithResultAsGeneric<TResult> safeMethodWithResultFromJson)
        : base(safeMethodWithResultFromJson)
    {
        Me.DeserializeStreamFunction = safeMethodWithResultFromJson.DeserializeStreamFunction;
        Me.DeserializeStringFunction = safeMethodWithResultFromJson.DeserializeStringFunction;
    }

    // Properties
    private ISafeMethodWithResultAsGeneric<TResult> Me
        => this;
    Func<Stream, TResult> ISafeMethodWithResultAsGeneric<TResult>.DeserializeStreamFunction { get; set; }
    Func<string, TResult> ISafeMethodWithResultAsGeneric<TResult>.DeserializeStringFunction { get; set; }

    // Methods
    public ISafeMethodWithResultAsGenericAndCache<TResult> WithCache(TimeSpan duration)
        => new SafeMethodWithResultAsGenericAndCache<TResult>(this, duration);

    public new virtual async Task<TResult> SendAsync(CancellationToken cancellationToken = default)
    {
        using var httpResponseMessage = await base.SendAsync(cancellationToken);
        return await Me.Endpoint.Request.HttpResponseMessageDeserializer.DeserializeAsync(
            httpResponseMessage,
            Me.DeserializeStreamFunction,
            Me.DeserializeStringFunction,
            cancellationToken);
    }
}
