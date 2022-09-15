using Microsoft.Extensions.Caching.Memory;

namespace CoreSharp.Http.FluentApi;

internal static class Settings
{
    // Properties
    public static IMemoryCache MemoryCache { get; }
        = new MemoryCache(new MemoryCacheOptions());
}
