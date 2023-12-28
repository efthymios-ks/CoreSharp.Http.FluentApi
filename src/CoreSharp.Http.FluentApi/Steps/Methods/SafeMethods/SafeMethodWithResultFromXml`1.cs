using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultFromXml{TResult}"/>
public class SafeMethodWithResultFromXml<TResult> :
    SafeMethodWithResult,
    ISafeMethodWithResultFromXml<TResult>
    where TResult : class
{
    // Constructors
    public SafeMethodWithResultFromXml(ISafeMethod method, Func<Stream, TResult> deserializeStreamFunction)
        : base(method)
    {
        ArgumentNullException.ThrowIfNull(deserializeStreamFunction);

        Me.DeserializeStreamFunction = deserializeStreamFunction;
    }

    public SafeMethodWithResultFromXml(ISafeMethod method, Func<string, TResult> deserializeStringFunction)
        : base(method)
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        Me.DeserializeStringFunction = deserializeStringFunction;
    }

    protected SafeMethodWithResultFromXml(ISafeMethodWithResultFromXml<TResult> method)
        : base(method)
    {
        Me.DeserializeStreamFunction = method.DeserializeStreamFunction;
        Me.DeserializeStringFunction = method.DeserializeStringFunction;
    }

    // Properties
    private ISafeMethodWithResultFromXml<TResult> Me
        => this;
    Func<Stream, TResult> ISafeMethodWithResultFromXml<TResult>.DeserializeStreamFunction { get; set; }
    Func<string, TResult> ISafeMethodWithResultFromXml<TResult>.DeserializeStringFunction { get; set; }

    public ISafeMethodWithResultFromXmlAndCache<TResult> WithCache(TimeSpan duration)
        => new SafeMethodWithResultFromXmlAndCache<TResult>(this, duration);

    // Methods 
    async Task<TResult> ISafeMethodWithResultFromXml<TResult>.SendAsync(CancellationToken cancellationToken)
    {
        using var httpResponseMessage = await base.SendAsync(cancellationToken);
        return await HttpResponseMessageUtils.DeserialeAsync(
            httpResponseMessage,
            Me.DeserializeStreamFunction,
            Me.DeserializeStringFunction,
            cancellationToken);
    }
}
