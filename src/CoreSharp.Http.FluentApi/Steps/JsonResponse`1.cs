using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps;

/// <inheritdoc cref="IJsonResponse{TResponse}"/>
internal sealed class JsonResponse<TResponse> : GenericResponse<TResponse>, IJsonResponse<TResponse>
    where TResponse : class
{
    // Constructors
    public JsonResponse(IMethod method, Func<Stream, TResponse> deserializeStreamFunction)
        : this(method)
    {
        ArgumentNullException.ThrowIfNull(deserializeStreamFunction);

        Me.DeserializeStreamFunction = deserializeStreamFunction;
    }

    public JsonResponse(IMethod method, Func<string, TResponse> deserializeStringFunction)
        : this(method)
    {
        ArgumentNullException.ThrowIfNull(deserializeStringFunction);

        Me.DeserializeStringFunction = deserializeStringFunction;
    }

    public JsonResponse(IMethod method)
        : base(method)
    {
    }

    // Properties 
    private IJsonResponse<TResponse> Me
        => this;

    Func<Stream, TResponse> IJsonResponse<TResponse>.DeserializeStreamFunction { get; set; }

    Func<string, TResponse> IJsonResponse<TResponse>.DeserializeStringFunction { get; set; }

    // Methods 
    public override async Task<TResponse> SendAsync(CancellationToken cancellationToken = default)
        => await IJsonResponseX.SendAsync(this, cancellationToken);
}
