using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethod"/>
public interface ISafeMethodWithResultAsStream :
    ISafeMethod,
    ICachableResult<ISafeMethodWithResultAsStreamAndCache>
{
    // Methods
    /// <inheritdoc cref="IMethod.SendAsync(CancellationToken)"/>
    new Task<Stream> SendAsync(CancellationToken cancellationToken = default);
}
