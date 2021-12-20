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
        /// <see cref="IXmlResponse{TResponse}.DeserializeStreamFunction"/> or
        /// <see cref="IXmlResponse{TResponse}.DeserializeStringFunction"/>.
        /// </summary>
        public static async Task<TResponse> SendAsync<TResponse>(IXmlResponse<TResponse> xmlResponse, CancellationToken cancellationToken = default)
            where TResponse : class
        {
            _ = xmlResponse ?? throw new ArgumentNullException(nameof(xmlResponse));

            using var response = await (xmlResponse as IResponse)!.SendAsync(cancellationToken);

            //Stream deserialization
            if (xmlResponse.DeserializeStreamFunction is not null)
            {
                await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                return xmlResponse.DeserializeStreamFunction(stream);
            }

            //String deserialization
            else if (xmlResponse.DeserializeStringFunction is not null)
            {
                var xml = await response.Content.ReadAsStringAsync(cancellationToken);
                return xmlResponse.DeserializeStringFunction(xml);
            }

            //Error
            throw new InvalidOperationException("No deserialization function has been provided.");
        }
    }
}
