using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

namespace CoreSharp.Http.FluentApi.Steps.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultAsStream"/>
public class SafeMethodWithResultAsStream :
    SafeMethodWithResult,
    ISafeMethodWithResultAsStream
{
    // Constructors
    internal SafeMethodWithResultAsStream(ISafeMethod? safeMethod)
        : base(safeMethod)
    {
    }

    // Methods
    public ISafeMethodWithResultAsStreamAndCache WithCache(TimeSpan duration)
        => new SafeMethodWithResultAsStreamAndCache(this, duration);

    public new virtual async Task<Stream> SendAsync(CancellationToken cancellationToken = default)
    {
        var response = await base.SendAsync(cancellationToken);
        if (response is null)
        {
            return new MemoryStream();
        }

        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }
}
