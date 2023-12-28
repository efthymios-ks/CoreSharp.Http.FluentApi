using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.UnsafeMethods;

/// <inheritdoc cref="IUnsafeMethod"/>
public interface IUnsafeMethodWithResultAsString : IUnsafeMethod
{
    // Methods
    /// <inheritdoc cref="IMethod.SendAsync(CancellationToken)"/>
    new Task<string> SendAsync(CancellationToken cancellationToken = default);
}
