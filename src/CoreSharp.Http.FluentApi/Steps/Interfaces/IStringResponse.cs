using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces;

public interface IStringResponse : IResponse
{
    /// <inheritdoc cref="IResponse.SendAsync(CancellationToken)"/>
    public new Task<string> SendAsync(CancellationToken cancellationtoken = default);
}
