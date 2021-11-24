using CoreSharp.HttpClient.FluentApi.Concrete;
using CoreSharp.HttpClient.FluentApi.Contracts;
using System;
using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Extensions
{
    /// <summary>
    /// <see cref="IRoute"/> extensions.
    /// </summary>
    public static class IRouteExtensions
    {
        //Methods 
        /// <summary>
        /// Set request to <see cref="HttpMethod.Get"/>.
        /// </summary>
        public static IQueryMethod Get(this IRoute route)
        {
            _ = route ?? throw new ArgumentNullException(nameof(route));

            return new QueryMethod(route, HttpMethod.Get);
        }

        /// <summary>
        /// Set request to <see cref="HttpMethod.Post"/>.
        /// </summary>
        public static IContentMethod Post(this IRoute route)
        {
            _ = route ?? throw new ArgumentNullException(nameof(route));

            return new ContentMethod(route, HttpMethod.Post);
        }

        /// <summary>
        /// Set request to <see cref="HttpMethod.Put"/>.
        /// </summary>
        public static IContentMethod Put(this IRoute route)
        {
            _ = route ?? throw new ArgumentNullException(nameof(route));

            return new ContentMethod(route, HttpMethod.Put);
        }

        /// <summary>
        /// Set request to <see cref="HttpMethod.Patch"/>.
        /// </summary>
        public static IContentMethod Patch(this IRoute route)
        {
            _ = route ?? throw new ArgumentNullException(nameof(route));

            return new ContentMethod(route, HttpMethod.Patch);
        }

        /// <summary>
        /// Set request to <see cref="HttpMethod.Delete"/>.
        /// </summary>
        public static IMethod Delete(this IRoute route)
        {
            _ = route ?? throw new ArgumentNullException(nameof(route));

            return new Method(route, HttpMethod.Delete);
        }
    }
}
