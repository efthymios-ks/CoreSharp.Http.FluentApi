using System.Net.Http;

namespace CoreSharp.Http.FluentApi.Contracts;

public interface IRoute
{
    //Properties 
    internal IRequest Request { get; set; }
    /// <inheritdoc cref="HttpRequestMessage.RequestUri" />
    internal string Route { get; set; }

    //Methods 
    /// <summary>
    /// The HTTP GET method requests a representation of the specified resource. <br/>
    /// Requests using GET should only be used to request data (they shouldn't include data).
    /// </summary>
    IQueryMethod Get();

    /// <summary>
    /// The HTTP POST method sends data to the server. <br/>
    /// The type of the body of the request is indicated by the Content-Type header.
    /// </summary>
    IContentMethod Post();

    /// <summary>
    /// The HTTP PUT request method creates a new resource or replaces a
    /// representation of the target resource with the request payload.
    /// </summary>
    IContentMethod Put();

    /// <summary>
    /// The PATCH method applies partial modifications to a resource.
    /// </summary>
    IContentMethod Patch();

    /// <summary>
    /// The DELETE method deletes the specified resource.
    /// </summary>
    IMethod Delete();

    /// <summary>
    /// The HTTP HEAD method requests the headers that would be returned
    /// if the HEAD request's URL was instead requested with the HTTP GET method. <br/>
    /// For example, if a URL might produce a large download,
    /// a HEAD request could read its Content-Length header to
    /// check the filesize without actually downloading the file.
    /// </summary>
    IMethod Head();

    /// <summary>
    /// The HTTP OPTIONS method requests permitted communication options for a given URL or server. <br/>
    /// A client can specify a URL with this method, or an asterisk (*) to refer to the entire server.
    /// </summary>
    IMethodWithResponse Options();

    /// <summary>
    /// The HTTP TRACE method performs a message loop-back
    /// test along the path to the target resource, <br/>
    /// providing a useful debugging mechanism. <br/>
    /// The final recipient of the request should reflect the message received,
    /// excluding some fields described below, <br/>
    /// back to the client as the message body of a 200 (OK)
    /// response with a Content-Type of message/http. <br/>
    /// The final recipient is either the origin server or the first
    /// server to receive a Max-Forwards value of 0 in the request.
    /// </summary>
    IMethod Trace();
}
