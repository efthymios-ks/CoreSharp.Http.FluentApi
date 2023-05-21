using CoreSharp.Http.FluentApi.Steps.Interfaces;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps;

/// <inheritdoc cref="IResponse" />
internal abstract class Response : IResponse
{
    // Constructors 
    protected Response(IMethod method)
    {
        ArgumentNullException.ThrowIfNull(method);

        Me.Method = method;
    }

    // Properties
    private IResponse Me
        => this;

    IMethod IResponse.Method { get; set; }

    // Methods 
    public async Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default)
        => await Me.Method.SendAsync(cancellationToken);
}
