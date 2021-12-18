using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.HttpClient.FluentApi.Utilities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Concrete
{
    /// <inheritdoc cref="IJsonQueryResponse{TResponse}"/>
    public class JsonQueryResponse<TResponse> : CacheQueryResponse<TResponse>, IJsonQueryResponse<TResponse> where TResponse : class
    {
        //Constructors
        public JsonQueryResponse(IQueryMethod queryMethod, Func<Stream, TResponse> deserializeStreamFunction)
            : this(queryMethod)
        {
            _ = deserializeStreamFunction ?? throw new ArgumentNullException(nameof(deserializeStreamFunction));

            Me.DeserializeStreamFunction = deserializeStreamFunction;
        }

        public JsonQueryResponse(IQueryMethod queryMethod, Func<string, TResponse> deserializeStringFunction)
            : this(queryMethod)
        {
            _ = deserializeStringFunction ?? throw new ArgumentNullException(nameof(deserializeStringFunction));

            Me.DeserializeStringFunction = deserializeStringFunction;
        }

        public JsonQueryResponse(IQueryMethod queryMethod) : base(queryMethod)
        {
        }

        //Properties 
        private IJsonQueryResponse<TResponse> Me => this;
        Func<Stream, TResponse> IJsonResponse<TResponse>.DeserializeStreamFunction { get; set; }
        Func<string, TResponse> IJsonResponse<TResponse>.DeserializeStringFunction { get; set; }

        //Methods 
        IJsonQueryResponse<TResponse> IJsonQueryResponse<TResponse>.Cache(TimeSpan duration)
        {
            (this as ICacheQueryResponse<TResponse>)!.Cache(duration);
            return this;
        }

        async ValueTask<TResponse> IJsonQueryResponse<TResponse>.SendAsync(CancellationToken cancellationToken)
        {
            //Extract args
            var route = Me.Method.Route.Route;
            var cacheDuration = Me.Duration;
            var memoryCache = Options.MemoryCache;

            //Prepare caching fields 
            var shouldCache = cacheDuration is not null && cacheDuration != TimeSpan.Zero;
            var cacheKey = shouldCache ? $"{route} > {typeof(TResponse).FullName}" : string.Empty;

            //Return cached value, if applicable 
            if (shouldCache && memoryCache.TryGetValue<TResponse>(cacheKey, out var cachedValue))
                return cachedValue;

            //Else request... 
            var response = await (this as IGenericResponse<TResponse>)!.SendAsync(cancellationToken);

            //...and cache response, if needed 
            if (shouldCache)
                memoryCache.Set(cacheKey, response, cacheDuration.Value);

            return response;
        }

        public override async Task<TResponse> SendAsync(CancellationToken cancellationToken = default)
            => await IJsonResponseX.SendAsync(this, cancellationToken);
    }
}
