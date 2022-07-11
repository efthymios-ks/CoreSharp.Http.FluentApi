using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace CoreSharp.Http.FluentApi.Utilities;

/// <summary>
/// <see cref="HttpMethod"/> utilities.
/// </summary>
internal static class HttpMethodX
{
    public static void ValidateQueryMethod(HttpMethod httpMethod)
    {
        _ = httpMethod ?? throw new ArgumentNullException(nameof(httpMethod));

        var validMethods = new[] { HttpMethod.Get };
        if (validMethods.Any(m => m == httpMethod))
            return;

        ThrowInvalidHttpMethodException(httpMethod, validMethods);
    }

    public static void ValidateContentMethod(HttpMethod httpMethod)
    {
        _ = httpMethod ?? throw new ArgumentNullException(nameof(httpMethod));

        var validMethods = new[] { HttpMethod.Post, HttpMethod.Put, HttpMethod.Patch };
        if (validMethods.Any(m => m == httpMethod))
            return;

        ThrowInvalidHttpMethodException(httpMethod, validMethods);
    }

    private static void ThrowInvalidHttpMethodException(HttpMethod httpMethod, IEnumerable<HttpMethod> validMethods)
    {
        var validMethodsAsString = string.Join(", ", validMethods.Select(m => m.Method));
        var message = $"{nameof(httpMethod)} ({httpMethod}) must be one of the following: {validMethodsAsString}.";
        throw new ArgumentException(message, nameof(httpMethod));
    }
}
