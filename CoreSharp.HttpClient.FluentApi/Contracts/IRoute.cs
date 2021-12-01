using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Contracts
{
    public interface IRoute
    {
        //Properties 
        internal IRequest Request { get; set; }
        internal string Route { get; set; }

        //Methods 
        /// <summary>
        /// Set request to <see cref="HttpMethod.Get"/>.
        /// </summary>
        IQueryMethod Get();

        /// <summary>
        /// Set request to <see cref="HttpMethod.Post"/>.
        /// </summary>
        IContentMethod Post();

        /// <summary>
        /// Set request to <see cref="HttpMethod.Put"/>.
        /// </summary>
        IContentMethod Put();

        /// <summary>
        /// Set request to <see cref="HttpMethod.Patch"/>.
        /// </summary>
        IContentMethod Patch();

        /// <summary>
        /// Set request to <see cref="HttpMethod.Delete"/>.
        /// </summary>
        IMethod Delete();
    }
}
