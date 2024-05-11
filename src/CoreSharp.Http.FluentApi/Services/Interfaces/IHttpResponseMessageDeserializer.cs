using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Utilities;

public interface IHttpResponseMessageDeserializer
{
    Task<TResult> DeserializeAsync<TResult>(
        HttpResponseMessage httpResponseMessage,
        Func<Stream, TResult> deserializeStreamFunction,
        Func<string, TResult> deserializeStringFunction,
        CancellationToken cancellationToken)
        where TResult : class;
}
