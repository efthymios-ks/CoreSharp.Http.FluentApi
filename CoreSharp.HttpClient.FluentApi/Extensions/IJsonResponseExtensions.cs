using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Extensions
{
    /// <summary>
    /// <see cref="IJsonQueryResponse{TResponse}"/> extensions.
    /// </summary>
    public static class IJsonResponseExtensions
    {
        //Methods 
        /// <inheritdoc cref="IMethodExtensions.SendAsync(IMethod, CancellationToken)"/>
        public static async Task<TResponse> SendAsync<TResponse>(this IJsonResponse<TResponse> jsonResponse, CancellationToken cancellationToken = default)
            where TResponse : class
        {
            _ = jsonResponse ?? throw new ArgumentNullException(nameof(jsonResponse));

            using var response = await jsonResponse.Method.SendAsync(cancellationToken);
            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return stream.ToEntity<TResponse>();
        }
    }
}
