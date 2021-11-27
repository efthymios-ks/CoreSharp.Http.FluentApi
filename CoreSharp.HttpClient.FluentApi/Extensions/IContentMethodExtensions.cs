using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.Models.Newtonsoft.Settings;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace CoreSharp.HttpClient.FluentApi.Extensions
{
    /// <summary>
    /// <see cref="IContentMethod"/> extensions.
    /// </summary>
    public static class IContentMethodExtensions
    {
        //Methods 
        /// <inheritdoc cref="Content(IContentMethod, string, string)" />
        public static IContentMethod Content(this IContentMethod contentMethod, string content)
            => contentMethod.Content(content, MediaTypeNames.Application.Json);

        /// <inheritdoc cref="Content(IContentMethod, string, Encoding, string)" />
        public static IContentMethod Content(this IContentMethod contentMethod, string content, string mediaContentType)
            => contentMethod.Content(content, Encoding.UTF8, mediaContentType);

        /// <inheritdoc cref="Content(IContentMethod, HttpContent)" />
        public static IContentMethod Content(this IContentMethod contentMethod, string content, Encoding encoding, string mediaContentType)
            => contentMethod.Content(new StringContent(content, encoding, mediaContentType));

        /// <inheritdoc cref="Content(IContentMethod, object, JsonSerializerSettings)" />
        public static IContentMethod Content(this IContentMethod contentMethod, object content)
            => contentMethod.Content(content, DefaultJsonSettings.Instance);

        /// <inheritdoc cref="Content(IContentMethod, HttpContent)" />
        public static IContentMethod Content(this IContentMethod contentMethod, object content, JsonSerializerSettings jsonSerializerSettings)
        {
            _ = jsonSerializerSettings ?? throw new ArgumentNullException(nameof(jsonSerializerSettings));

            if (content is HttpContent httpContent)
                contentMethod.Content(httpContent);
            else if (content is string json)
                contentMethod.Content(json);
            else if (content is not null)
                contentMethod.Content(ToStreamContent(content, jsonSerializerSettings));
            return contentMethod;
        }

        /// <summary>
        /// Sets <see cref="HttpRequestMessage.Content"/>.
        /// </summary>
        public static IContentMethod Content(this IContentMethod contentMethod, HttpContent httpContent)
        {
            _ = contentMethod ?? throw new ArgumentNullException(nameof(contentMethod));

            contentMethod.Content = httpContent;

            return contentMethod;
        }

        //Private 
        /// <summary>
        /// Build <see cref="StreamContent" /> from given item.
        /// </summary>
        private static StreamContent ToStreamContent(
            object content,
            JsonSerializerSettings jsonSerializerSettings,
            int bufferSize = 4096)
        {
            _ = content ?? throw new ArgumentNullException(nameof(content));

            var serializer = JsonSerializer.Create(jsonSerializerSettings);
            var stream = new MemoryStream();
            using var streamWriter = new StreamWriter(stream, Encoding.UTF8, bufferSize, true);
            using var jsonWriter = new JsonTextWriter(streamWriter);

            serializer.Serialize(jsonWriter, content);
            streamWriter.Flush();
            stream.Position = 0;

            var streamContent = new StreamContent(stream, bufferSize);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
            return streamContent;
        }
    }
}
