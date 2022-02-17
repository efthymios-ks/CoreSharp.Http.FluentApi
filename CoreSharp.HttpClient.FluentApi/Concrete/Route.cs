using CoreSharp.HttpClient.FluentApi.Contracts;
using System;
using System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    /// <inheritdoc cref="IRoute"/>
    internal class Route : IRoute
    {
        //Constructors  
        public Route(IRequest request, string resourceName)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentNullException(nameof(resourceName));

            var me = Me;
            me.Request = request;
            me.Route = resourceName;
        }

        //Properties 
        private IRoute Me
            => this;

        IRequest IRoute.Request { get; set; }

        string IRoute.Route { get; set; }

        //Methods 
        public IQueryMethod Get()
            => new QueryMethod(this, HttpMethod.Get);

        public IContentMethod Post()
            => new ContentMethod(this, HttpMethod.Post);

        public IContentMethod Put()
            => new ContentMethod(this, HttpMethod.Put);

        public IContentMethod Patch()
            => new ContentMethod(this, HttpMethod.Patch);

        public IMethod Delete()
            => new Method(this, HttpMethod.Delete);
    }
}
