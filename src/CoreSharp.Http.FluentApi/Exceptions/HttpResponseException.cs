using CoreSharp.Utilities;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Exceptions;

/// <summary>
/// Simple <see cref="HttpResponseMessage"/> <see cref="Exception"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class HttpResponseException : Exception
{
    //Constructors 
    public HttpResponseException(
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

    //Properties 
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay
        => ResponseStatus;
    public string RequestUrl { get; }
    public string RequestMethod { get; }
    public HttpStatusCode ResponseStatusCode { get; }
    public string ResponseContent
        => Message;
    public string ResponseStatus
        => $"{RequestMethod} > {RequestUrl} > {(int)ResponseStatusCode} {ResponseStatusCode}";

    //Methods
    public override string ToString()
    {
        if (JsonX.IsEmpty(ResponseContent))
            return ResponseStatus;
        else
            return ResponseStatus + Environment.NewLine + ResponseContent;
    }

    /// <summary>
    /// Create new instance of <see cref="HttpResponseException"/>
    /// using a <see cref="HttpResponseMessage"/>.
    /// Does not dispose <see cref="HttpResponseMessage"/>.
    /// </summary>
    public static async Task<HttpResponseException> CreateAsync(HttpResponseMessage response, Exception exception = null)
    {
        _ = response ?? throw new ArgumentNullException(nameof(response));

        var request = response.RequestMessage;

        var requestUrl = request?.RequestUri?.AbsoluteUri;
        var requestMethod = $"{nameof(HttpMethod)}.{request?.Method.Method}";
        var responseStatus = response.StatusCode;
        var responseContent = await response.Content?.ReadAsStringAsync();
        return new(requestUrl, requestMethod, responseStatus, responseContent, exception);
    }
}
