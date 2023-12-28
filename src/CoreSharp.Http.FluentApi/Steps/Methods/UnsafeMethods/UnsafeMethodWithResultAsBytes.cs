using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;

public class UnsafeMethodWithResultAsBytes : UnsafeMethodWithResult, IUnsafeMethodWithResultAsBytes
{
    // Constructors
    public UnsafeMethodWithResultAsBytes(IUnsafeMethod method)
        : base(method)
    {
    }

    // Methods 
    public new virtual async Task<byte[]> SendAsync(CancellationToken cancellationToken = default)
    {
        using var response = await base.SendAsync(cancellationToken);
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }
}
