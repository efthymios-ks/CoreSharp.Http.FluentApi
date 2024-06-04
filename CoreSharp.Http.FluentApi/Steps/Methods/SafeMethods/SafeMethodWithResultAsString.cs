using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsString"/>
public class SafeMethodWithResultAsString :
    SafeMethodWithResult,
    ISafeMethodWithResultAsString
{
    // Constructors
    internal SafeMethodWithResultAsString(ISafeMethod? safeMethod)
        : base(safeMethod)
    {
    }

    // Methods
    public ISafeMethodWithResultAsStringAndCache WithCache(TimeSpan duration)
        => new SafeMethodWithResultAsStringAndCache(this, duration);

    public new virtual async Task<string?> SendAsync(CancellationToken cancellationToken = default)
    {
        using var response = await base.SendAsync(cancellationToken);
        if (response is null)
        {
            return null;
        }

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
