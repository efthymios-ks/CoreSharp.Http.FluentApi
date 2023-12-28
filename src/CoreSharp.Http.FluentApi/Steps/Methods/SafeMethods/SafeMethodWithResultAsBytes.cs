using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsBytes"/>
public class SafeMethodWithResultAsBytes :
    SafeMethodWithResult,
    ISafeMethodWithResultAsBytes
{
    // Constructors
    public SafeMethodWithResultAsBytes(ISafeMethod method)
        : base(method)
    {
    }

    // Methods
    public ISafeMethodWithResultAsBytesAndCache WithCache(TimeSpan duration)
        => new SafeMethodWithResultAsBytesAndCache(this, duration);

    public new virtual async Task<byte[]> SendAsync(CancellationToken cancellationToken = default)
    {
        using var response = await base.SendAsync(cancellationToken);
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }
}
