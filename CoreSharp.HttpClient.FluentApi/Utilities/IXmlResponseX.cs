using CoreSharp.HttpClient.FluentApi.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Utilities
{
    /// <summary>
    /// <see cref="IXmlResponse{TResponse}"/> utilities.
    /// </summary>
    internal static class IXmlResponseX
    {
        //Methods 
        /// <summary>
        /// Call <see cref="IMethod.SendAsync(CancellationToken)"/>
        /// and deserialize to provided type using either
        /// <see cref="IXmlResponse{TResponse}.DeserializeStringFunction"/>.
        /// </summary>
        public static async Task<TResponse> SendAsync<TResponse>(IXmlResponse<TResponse> xmlResponse, CancellationToken cancellationToken = default)
            where TResponse : class
        {
            _ = xmlResponse ?? throw new ArgumentNullException(nameof(xmlResponse));

            using var response = await (xmlResponse as IResponse)!.SendAsync(cancellationToken);

            //String deserialization
            if (xmlResponse.DeserializeStringFunction is not null)
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                return xmlResponse.DeserializeStringFunction(json);
            }

            //Error
            throw new InvalidOperationException("No deserialization function has been provided.");
        }
    }
}
