using CoreSharp.Http.FluentApi.Steps.Interfaces;
using CoreSharp.Http.FluentApi.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Steps;

/// <inheritdoc cref="IXmlResponse{TResponse}"/>
internal sealed class XmlResponse<TResponse> : GenericResponse<TResponse>, IXmlResponse<TResponse>
    where TResponse : class
{
    // Constructors
    public XmlResponse(IMethod method, Func<Stream, TResponse> deserializeStreamFunction)
        : this(method)
        => Me.DeserializeStreamFunction = deserializeStreamFunction ?? throw new ArgumentNullException(nameof(deserializeStreamFunction));

    public XmlResponse(IMethod method, Func<string, TResponse> deserializeStringFunction)
        : this(method)
        => Me.DeserializeStringFunction = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

    public XmlResponse(IMethod method)
        : base(method)
    {
    }

    // Properties 
    private IXmlResponse<TResponse> Me
        => this;

    Func<Stream, TResponse> IXmlResponse<TResponse>.DeserializeStreamFunction { get; set; }

    Func<string, TResponse> IXmlResponse<TResponse>.DeserializeStringFunction { get; set; }

    // Methods 
    public override async Task<TResponse> SendAsync(CancellationToken cancellationToken = default)
        => await IXmlResponseX.SendAsync(this, cancellationToken);
}
