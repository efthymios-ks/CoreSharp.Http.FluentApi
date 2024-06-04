using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;

namespace CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;

public class UnsafeMethodWithResultAsString : UnsafeMethodWithResult, IUnsafeMethodWithResultAsString
{
    // Constructors
    internal UnsafeMethodWithResultAsString(IUnsafeMethod? method)
        : base(method)
    {
    }

    // Methods
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
