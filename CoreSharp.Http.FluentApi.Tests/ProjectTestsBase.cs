using CoreSharp.Http.FluentApi.Steps.Interfaces;
using System.Net;
using Tests.Common;
using Tests.Common.Mocks;
using Tests.Common.Mocks.Generator;
using Xunit.Abstractions;

namespace CoreSharp.Http.FluentApi.Tests;

public abstract class ProjectTestsBase : TestsBase
{
    protected ProjectTestsBase()
    {
    }

    protected ProjectTestsBase(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    protected override void ConfigureMockDataGenerator(IMockDataGenerator mockDataGenerator)
    {
        mockDataGenerator.Configure(() => new MockHttpMessageHandler()
        {
            HttpResponseMessageFactory = () => new HttpResponseMessage(HttpStatusCode.OK)
        });

        mockDataGenerator.Configure(() =>
        {
            var mockHttpMessageHandler = mockDataGenerator.Create<MockHttpMessageHandler>();
            return new HttpClient(mockHttpMessageHandler);
        });

        mockDataGenerator.Configure(() =>
        {
            var request = Substitute.For<IRequest>();

            var httpClient = mockDataGenerator.Create<HttpClient>();
            request.HttpClient.Returns(httpClient);
            request.HttpCompletionOption.Returns(HttpCompletionOption.ResponseContentRead);
            request.Headers.Returns(new Dictionary<string, string>());
            request.ThrowOnError.Returns(false);
            request.Timeout.Returns((TimeSpan?)null);

            return request;
        });

        mockDataGenerator.Configure(() =>
        {
            var endpoint = Substitute.For<IEndpoint>();

            var request = mockDataGenerator.Create<IRequest>();
            endpoint.Request.Returns(request);
            endpoint.Endpoint.Returns("http://www.example.com/");
            endpoint.QueryParameters.Returns(new Dictionary<string, string>());

            return endpoint;
        });

        mockDataGenerator.Configure(() => Substitute.For<MemoryStream>());
    }
}
