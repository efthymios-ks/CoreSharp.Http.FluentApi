using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;

public class UnsafeMethodWithResultAsStream : UnsafeMethodWithResult, IUnsafeMethodWithResultAsStream
{
    // Constructors
    public UnsafeMethodWithResultAsStream(IUnsafeMethod method)
        : base(method)
    {
    }

    // Methods
    public new virtual async Task<Stream> SendAsync(CancellationToken cancellationToken = default)
    {
        using var response = await base.SendAsync(cancellationToken);
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }
}