using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
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
    }
}
