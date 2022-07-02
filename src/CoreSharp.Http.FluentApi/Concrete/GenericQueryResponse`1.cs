using CoreSharp.Http.FluentApi.Concrete;
using System;

namespace CoreSharp.Http.FluentApi.Contracts;

/// <inheritdoc cref="IGenericQueryResponse{TResponse}"/>
internal class GenericQueryResponse<TResponse> : GenericResponse<TResponse>, IGenericQueryResponse<TResponse>
    where TResponse : class
{
    //Constructors
    public GenericQueryResponse(IQueryMethod queryMethod)
        : base(queryMethod)
    {
    }

    //Methods
    public ICacheQueryResponse<TResponse> Cache(TimeSpan duration)
    {
        var cacheQueryResponse = new CacheQueryResponse<TResponse>(this);
        (cacheQueryResponse as ICacheQueryResponse<TResponse>)!.Cache(duration);
        return cacheQueryResponse;
    }
}
