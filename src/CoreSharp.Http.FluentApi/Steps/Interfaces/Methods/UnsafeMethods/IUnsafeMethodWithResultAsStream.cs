using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;

/// <inheritdoc cref="IUnsafeMethod"/>
public interface IUnsafeMethodWithResultAsStream : IUnsafeMethod
{
    // Methods
    /// <inheritdoc cref="IMethod.SendAsync(CancellationToken)"/>
    new Task<Stream> SendAsync(CancellationToken cancellationToken = default);
}
