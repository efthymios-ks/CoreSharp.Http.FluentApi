using CoreSharp.Http.FluentApi.Steps.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Utilities;

/// <summary>
/// <see cref="IJsonResponse{TResponse}"/> utilities.
/// </summary>
internal static class IJsonResponseX
{
    // Methods 
    /// <summary>
    /// Call <see cref="IMethod.SendAsync(CancellationToken)"/>
    /// and deserialize to provided type using either
    /// <see cref="IJsonResponse{TResponse}.DeserializeStreamFunction"/> or
    /// <see cref="IJsonResponse{TResponse}.DeserializeStringFunction"/>.
    /// </summary>
    public static async Task<TResponse> SendAsync<TResponse>(IJsonResponse<TResponse> jsonResponse, CancellationToken cancellationToken = default)
        where TResponse : class
    {
        _ = jsonResponse ?? throw new ArgumentNullException(nameof(jsonResponse));

        using var response = await (jsonResponse as IResponse)!.SendAsync(cancellationToken);
        if (response is null)
            return null;

        // Stream deserialization 
        if (jsonResponse.DeserializeStreamFunction is not null)
        {
            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return jsonResponse.DeserializeStreamFunction(stream);
        }

        // String deserialization 
        else if (jsonResponse.DeserializeStringFunction is not null)
        {
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            return jsonResponse.DeserializeStringFunction(json);
        }

        // Error
        throw new InvalidOperationException("No deserialization function has been provided.");
    }
}
