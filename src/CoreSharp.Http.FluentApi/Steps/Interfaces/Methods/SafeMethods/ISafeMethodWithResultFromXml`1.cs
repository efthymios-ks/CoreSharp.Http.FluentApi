using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethod"/>
public interface ISafeMethodWithResultFromXml<TResult> :
    ISafeMethod,
    ICachableResult<ISafeMethodWithResultFromXmlAndCache<TResult>>
    where TResult : class
{
    // Properties 
    internal Func<Stream, TResult> DeserializeStreamFunction { get; set; }
    internal Func<string, TResult> DeserializeStringFunction { get; set; }

    // Methods
    /// <inheritdoc cref="IMethod.SendAsync(CancellationToken)"/>
    new Task<TResult> SendAsync(CancellationToken cancellationToken = default);
}
