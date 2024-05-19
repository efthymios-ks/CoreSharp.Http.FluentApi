using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.NUnit3;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Tests.Internal.HttpmessageHandlers;

namespace Tests.Internal.Attributes;

public sealed class AutoNSubstituteDataAttribute : AutoDataAttribute
{
    public AutoNSubstituteDataAttribute()
        : base(GetFixture)
    {
    }

    private static IFixture GetFixture()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoNSubstituteCustomization()
        {
            ConfigureMembers = true
        });

        fixture.Register<IFixture>(() => fixture);

        fixture.Register(() => new MockHttpMessageHandler()
        {
            ResponseStatus = System.Net.HttpStatusCode.OK,
        });

        fixture.Register(() =>
        {
            var mockHttpMessageHandler = fixture.Create<MockHttpMessageHandler>();
            return new HttpClient(mockHttpMessageHandler);
        });

        fixture.Register(() =>
        {
            var request = Substitute.For<IRequest>();

            var httpClient = fixture.Create<HttpClient>();
            request.HttpClient.Returns(httpClient);
            request.HttpCompletionOption.Returns(HttpCompletionOption.ResponseContentRead);
            request.Headers.Returns(new Dictionary<string, string>());
            request.ThrowOnError.Returns(false);
            request.Timeout.Returns((TimeSpan?)null);

            return request;
        });

        fixture.Register(() =>
        {
            var endpoint = Substitute.For<IEndpoint>();

            var request = fixture.Create<IRequest>();
            endpoint.Request.Returns(request);
            endpoint.Endpoint.Returns("http://www.example.com/");
            endpoint.QueryParameters.Returns(new Dictionary<string, string>());

            return endpoint;
        });

        return fixture;
    }
}
