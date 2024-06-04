using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;

namespace CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;

public class UnsafeMethodWithResultAsBytes : UnsafeMethodWithResult, IUnsafeMethodWithResultAsBytes
{
    internal UnsafeMethodWithResultAsBytes(IUnsafeMethod? method)
        : base(method)
    {
    }

    // Methods 
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
