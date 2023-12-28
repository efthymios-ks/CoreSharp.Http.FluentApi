using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Exceptions;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed class HttpOperationException : Exception
{
    // Constructors 
    public HttpOperationException(
        string requestUrl,
        string requestMethod,
        HttpStatusCode responseStatusCode,
        string responseContent,
        Exception innerException = null)
        : base(responseContent, innerException)
    {
        RequestUrl = requestUrl;
        RequestMethod = requestMethod;
        ResponseStatusCode = responseStatusCode;
    }

    // Properties 
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay
        => LogEntry;
    public string RequestUrl { get; }
    public string RequestMethod { get; }
    public HttpStatusCode ResponseStatusCode { get; }
    public string ResponseContent
        => Message;
    public string LogEntry
        => $"{RequestMethod} > {RequestUrl} > {(int)ResponseStatusCode} {ResponseStatusCode}";

    // Methods
    public override string ToString()
        => LogEntry;

    /// <summary>
    /// Create new instance of <see cref="HttpOperationException"/>
    /// using a <see cref="HttpResponseMessage"/>.
    /// Does not dispose <see cref="HttpResponseMessage"/>.
    /// </summary>
    public static async Task<HttpOperationException> CreateAsync(HttpResponseMessage response, Exception exception = null)
    {
        ArgumentNullException.ThrowIfNull(response);

        var request = response.RequestMessage;
        var requestUrl = request?.RequestUri?.AbsoluteUri;
        var requestMethod = $"{request?.Method.Method}";
        var responseStatus = response.StatusCode;
        var responseContent = await response.Content.ReadAsStringAsync();
        return new(requestUrl, requestMethod, responseStatus, responseContent, exception);
    }
}
