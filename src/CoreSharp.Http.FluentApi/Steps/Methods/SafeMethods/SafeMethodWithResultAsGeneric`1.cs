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
    public SafeMethodWithResultAsGeneric(ISafeMethod safeMethod, Func<Stream, Task<TResult>> deserializeStreamFunction)
        : base(safeMethod)
    {
        ArgumentNullException.ThrowIfNull(deserializeStreamFunction);

        Me.DeserializeFunction = deserializeStreamFunction;
    }

    // Properties
    private ISafeMethodWithResultAsGeneric<TResult> Me
        => this;
    Func<Stream, Task<TResult>> ISafeMethodWithResultAsGeneric<TResult>.DeserializeFunction { get; set; }

    // Methods
    public ISafeMethodWithResultAsGenericAndCache<TResult> WithCache(TimeSpan duration)
        => new SafeMethodWithResultAsGenericAndCache<TResult>(this, duration);

    public new virtual async Task<TResult> SendAsync(CancellationToken cancellationToken = default)
    {
        using var response = await base.SendAsync(cancellationToken);
        using var buffer = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await Me.DeserializeFunction(buffer);
    }
}
