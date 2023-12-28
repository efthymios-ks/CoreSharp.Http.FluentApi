using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;
using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsString"/>
public class SafeMethodWithResultAsString :
    SafeMethodWithResult,
    ISafeMethodWithResultAsString
{
    // Constructors
    public SafeMethodWithResultAsString(ISafeMethod method)
        : base(method)
    {
    }

    // Methods
    ISafeMethodWithResultAsStringAndCache ICachableResult<ISafeMethodWithResultAsStringAndCache>.WithCache(TimeSpan duration)
        => new SafeMethodWithResultAsStringAndCache(this, duration);

    public new virtual async Task<string> SendAsync(CancellationToken cancellationToken = default)
    {
        using var response = await base.SendAsync(cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
