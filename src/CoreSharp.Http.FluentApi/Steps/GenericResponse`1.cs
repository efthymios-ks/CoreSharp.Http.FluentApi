using CoreSharp.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps;

/// <inheritdoc cref="IGenericResponse{TResponse}"/>
internal class GenericResponse<TResponse> : Response, IGenericResponse<TResponse>
    where TResponse : class
{
    // Constructors
    public GenericResponse(IMethod method)
        : base(method)
    {
    }

    // Methods
    public new virtual async Task<TResponse> SendAsync(CancellationToken cancellationToken = default)
    {
        using var response = await base.SendAsync(cancellationToken);
        if (response is null)
        {
            return null;
        }

        return await response.Content.DeserializeAsync<TResponse>(cancellationToken);
    }
}
