using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsBytes"/>
public class SafeMethodWithResultAsBytes :
    SafeMethodWithResult,
    ISafeMethodWithResultAsBytes
{
    // Constructors 
    internal SafeMethodWithResultAsBytes(ISafeMethod safeMethod)
        : base(safeMethod)
    {
    }

    // Methods
    public ISafeMethodWithResultAsBytesAndCache WithCache(TimeSpan duration)
        => new SafeMethodWithResultAsBytesAndCache(this, duration);

    public new virtual async Task<byte[]> SendAsync(CancellationToken cancellationToken = default)
    {
        using var response = await base.SendAsync(cancellationToken);
        if (response is null)
        {
            return [];
        }

        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }
}
