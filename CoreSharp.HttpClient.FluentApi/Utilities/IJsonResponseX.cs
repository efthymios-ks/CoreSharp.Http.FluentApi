using CoreSharp.HttpClient.FluentApi.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Utilities
{
    /// <summary>
    /// <see cref="IJsonQueryResponse{TResponse}"/> utilities.
    /// </summary>
    internal static class IJsonResponseX
    {
        //Methods 
        /// <inheritdoc cref="IMethod.SendAsync(CancellationToken)"/>
        public static async Task<TResponse> SendAsync<TResponse>(IJsonResponse<TResponse> jsonResponse, CancellationToken cancellationToken = default)
            where TResponse : class
        {
            _ = jsonResponse ?? throw new ArgumentNullException(nameof(jsonResponse));

            using var response = await jsonResponse.Method.SendAsync(cancellationToken);

            //Stream deserialization 
            if (jsonResponse.DeserializeStreamFunction is not null)
            {
                await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                return jsonResponse.DeserializeStreamFunction(stream);
            }

            //String deserialization
            else if (jsonResponse.DeserializeStringFunction is not null)
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                return jsonResponse.DeserializeStringFunction(json);
            }

            //Error
            else
                throw new InvalidOperationException("No deserialization function has been provided.");
        }
    }
}
