using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsStream"/>
public class SafeMethodWithResultAsStream :
    SafeMethodWithResult,
    ISafeMethodWithResultAsStream
{
    // Constructors
    public SafeMethodWithResultAsStream(ISafeMethod safeMethod)
        : base(safeMethod)
    {
    }

    // Methods
    public ISafeMethodWithResultAsStreamAndCache WithCache(TimeSpan duration)
        => new SafeMethodWithResultAsStreamAndCache(this, duration);

    public new virtual async Task<Stream> SendAsync(CancellationToken cancellationToken = default)
    {
        var response = await base.SendAsync(cancellationToken);
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }
}