using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using static System.FormattableString;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    internal class Request : IRequest
    {
        //Constructors 
        public Request(System.Net.Http.HttpClient httpClient)
            => Me.HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        //Properties 
        private IRequest Me => this;
        System.Net.Http.HttpClient IRequest.HttpClient { get; set; }
        HttpCompletionOption IRequest.CompletionOptionInternal { get; set; } = HttpCompletionOption.ResponseHeadersRead;
        IDictionary<string, string> IRequest.HeadersInternal { get; } = new Dictionary<string, string>();
        bool IRequest.ThrowOnError { get; set; } = true;

        //Methods 
        public IRequest Headers(IDictionary<string, string> headers)
        {
            _ = headers ?? throw new ArgumentNullException(nameof(headers));

            foreach (var header in headers)
                Header(header.Key, header.Value);

            return this;
        }

        public IRequest Header(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(nameof(key)))
                throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(nameof(value)))
                throw new ArgumentNullException(nameof(value));

            Me.HeadersInternal.AddOrUpdate(key, value);

            return this;
        }

        public IRequest Accept(string mediaType)
            => Header("Accept", mediaType);

        public IRequest AcceptJson()
            => Accept(MediaTypeNames.Application.Json);

        public IRequest AcceptXml()
            => Accept(MediaTypeNames.Application.Xml);

        public IRequest Authorization(string accessToken)
             => Header("Authorization", $"Bearer {accessToken}");

        public IRequest IgnoreError()
        {
            Me.ThrowOnError = false;
            return this;
        }

        public IRequest CompletionOption(HttpCompletionOption completionOption)
        {
            Me.CompletionOptionInternal = completionOption;
            return this;
        }

        public IRoute Route(string resourceName, int key)
            => Route(Invariant($"{resourceName}/{key})"));

        public IRoute Route(string resourceName, long key)
            => Route(Invariant($"{resourceName}/{key}"));

        public IRoute Route(string resourceName, Guid key)
            => Route($"{resourceName}/{key}");

        public IRoute Route(string resourceName, string key)
            => Route($"{resourceName}/{key}");

        public IRoute Route(string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentNullException(nameof(resourceName));

            //Fix resource name 
            resourceName = UriX.JoinSegments(resourceName).TrimStart('/');

            return new Route(this, resourceName);
        }
    }
}
