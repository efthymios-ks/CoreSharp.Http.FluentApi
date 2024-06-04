using CoreSharp.Http.FluentApi.Exceptions;

namespace CoreSharp.Http.FluentApi.Extensions;

/// <summary>
/// <see cref="HttpResponseMessage"/> extensions.
/// </summary>
public static class HttpResponseMessageExtensions
{
    public static async Task EnsureSuccessAsync(this HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(response);

        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var exception = await HttpOperationException.CreateAsync(response);
        response.Content.Dispose();
        throw exception;
    }
}
