using Microsoft.Extensions.Caching.Memory;

namespace CoreSharp.HttpClient.FluentApi
{
    internal static class Options
    {
        //Properties
        public static IMemoryCache MemoryCache { get; } = new MemoryCache(new MemoryCacheOptions());
    }
}
