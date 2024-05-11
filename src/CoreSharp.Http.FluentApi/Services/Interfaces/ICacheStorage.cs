using CoreSharp.Http.FluentApi.Steps.Interfaces.Methods;
using System;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Services.Interfaces;

public interface ICacheStorage
{
    // Methods
    string GetCacheKey<TResponse>(IMethod method);

    Task<TResult> GetOrAddResultAsync<TResult>(
        IMethod method,
        TimeSpan cacheDuration,
        Func<Task<TResult>> requestFactory,
        Func<Task<bool>> cacheInvalidationFactory);
}
