using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;

public class UnsafeMethodWithResultAsString : UnsafeMethodWithResult, IUnsafeMethodWithResultAsString
{
    // Constructors
    public UnsafeMethodWithResultAsString(IUnsafeMethod method)
        : base(method)
    {
    }

    // Methods
    public new virtual async Task<string> SendAsync(CancellationToken cancellationToken = default)
    {
        using var response = await base.SendAsync(cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
