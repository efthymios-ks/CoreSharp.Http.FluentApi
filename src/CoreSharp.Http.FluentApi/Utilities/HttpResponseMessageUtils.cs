using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Utilities;

internal static class HttpResponseMessageUtils
{
    public static async Task<TResult> DeserialeAsync<TResult>(
        HttpResponseMessage httpResponseMessage,
        Func<Stream, TResult> deserializeStreamFunction,
        Func<string, TResult> deserializeStringFunction,
        CancellationToken cancellationToken)
        where TResult : class
    {
        ArgumentNullException.ThrowIfNull(httpResponseMessage);

        var result = await TryDeserializeFromStreamAsync(httpResponseMessage, deserializeStreamFunction, cancellationToken);
        if (result.success)
        {
            return result.result;
        }

        result = await TryDeserializeFromStringAsync(httpResponseMessage, deserializeStringFunction, cancellationToken);
        if (result.success)
        {
            return result.result;
        }

        throw new InvalidOperationException("No deserialization function has been provided.");
    }

    private static async Task<(bool success, TResult result)> TryDeserializeFromStreamAsync<TResult>(
        HttpResponseMessage response,
        Func<Stream, TResult> deserializeStreamFunction,
        CancellationToken cancellationToken)
        where TResult : class
    {
        if (deserializeStreamFunction is null)
        {
            return (false, null);
        }

        using var buffer = await response.Content.ReadAsStreamAsync(cancellationToken);
        var result = deserializeStreamFunction(buffer);
        return (true, result);
    }

    private static async Task<(bool success, TResult result)> TryDeserializeFromStringAsync<TResult>(
        HttpResponseMessage response,
        Func<string, TResult> deserializeStringFunction,
        CancellationToken cancellationToken)

        where TResult : class
    {
        if (deserializeStringFunction is null)
        {
            return (false, null);
        }

        var buffer = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = deserializeStringFunction(buffer);
        return (true, result);
    }
}
