using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;

namespace CoreSharp.Http.FluentApi.Steps.Methods.UnsafeMethods;

public class UnsafeMethodWithResultAsStream : UnsafeMethodWithResult, IUnsafeMethodWithResultAsStream
{
    // Constructors
    internal UnsafeMethodWithResultAsStream(IUnsafeMethod? method)
        : base(method)
    {
    }

    // Methods
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
