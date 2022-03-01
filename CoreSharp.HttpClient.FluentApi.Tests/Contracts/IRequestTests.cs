﻿using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.HttpClient.FluentApi.Exceptions;
using CoreSharp.HttpClient.FluentApi.Extensions;
using CoreSharp.HttpClient.FluentApi.Tests.Abstracts;
using FluentAssertions;
using Microsoft.Net.Http.Headers;
using Moq.Contrib.HttpClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using static System.FormattableString;

namespace CoreSharp.HttpClient.FluentApi.Tests.Contracts
{
    public class IRequestTests : HttpClientTestsBase
    {
        //Methods
        [Test]
        [TestCase(null, "1")]
        [TestCase("", "1")]
        [TestCase(" ", "1")]
        [TestCase("h", null)]
        [TestCase("h", "")]
        [TestCase("h", " ")]
        public void Header_KeyOrValueIsEmpty_ThrowArgumentNullException(string headerKey, string headerValue)
        {
            //Act
            Action action = () => Client.Request()
                                        .Header(headerKey, headerValue);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase("h", "1")]
        public async Task Header_WhenCalled_AddHeaderToRequest(string headerKey, string headerValue)
        {
            //Arrange 
            bool AssertRequest(HttpRequestMessage request)
                => AssertRequestHeader(request, headerKey, headerValue);
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .Header(headerKey, headerValue)
                        .Route("tests")
                        .Get()
                        .SendAsync();
        }

        [Test]
        [TestCase(MediaTypeNames.Application.Json)]
        public async Task Accept_WhenCalled_AddAcceptHeaderToRequest(string acceptHeaderValue)
        {
            //Arrange 
            bool AssertRequest(HttpRequestMessage request)
                => AssertRequestHeader(request, HeaderNames.Accept, acceptHeaderValue);
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .Accept(acceptHeaderValue)
                        .Route("tests")
                        .Get()
                        .SendAsync();
        }

        [Test]
        public async Task AcceptJson_WhenCalled_AddJsonAcceptHeaderToRequest()
        {
            //Arrange 
            static bool AssertRequest(HttpRequestMessage request)
                => AssertRequestHeader(request, HeaderNames.Accept, MediaTypeNames.Application.Json);
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .AcceptJson()
                        .Route("tests")
                        .Get()
                        .SendAsync();
        }

        [Test]
        public async Task AcceptXml_WhenCalled_AddXmlAcceptHeaderToRequest()
        {
            //Arrange 
            static bool AssertRequest(HttpRequestMessage request)
                => AssertRequestHeader(request, HeaderNames.Accept, MediaTypeNames.Application.Xml);
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .AcceptXml()
                        .Route("tests")
                        .Get()
                        .SendAsync();
        }

        [Test]
        [TestCase("123")]
        public async Task Bearer_WhenCalled_AddAuthorizationHeaderToRequest(string authorizationHeaderValue)
        {
            //Arrange 
            bool AssertRequest(HttpRequestMessage request)
                => AssertRequestHeader(request, HeaderNames.Authorization, $"Bearer {authorizationHeaderValue}");
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .Bearer(authorizationHeaderValue)
                        .Route("tests")
                        .Get()
                        .SendAsync();
        }

        [Test]
        public async Task IgnoreError_HasErrorAndNotCalled_ThrowHttpResponseException()
        {
            //Arrange 
            MockHandler.SetupAnyRequest()
                       .ReturnsResponse(HttpStatusCode.InternalServerError);

            //Act
            Func<Task> task = () => Client.Request()
                                          .Route("tests")
                                          .Get()
                                          .SendAsync();

            //Assert
            await task.Should().ThrowExactlyAsync<HttpResponseException>();
        }

        [Test]
        public async Task IgnoreError_HasErrorAndCalled_DoNotThrowHttpResponseException()
        {
            //Arrange 
            MockHandler.SetupAnyRequest()
                       .ReturnsResponse(HttpStatusCode.InternalServerError);

            //Act
            Func<Task> task = () => Client.Request()
                                          .IgnoreError()
                                          .Route("tests")
                                          .Get()
                                          .SendAsync();

            //Assert
            await task.Should().NotThrowAsync<HttpResponseException>();
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Route_ResourceNameIsEmpty_ThrowArgumentNullException(string resourceName)
        {
            //Act 
            Action action = () => Client.Request()
                                        .Route(resourceName);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Route_WhenCalled_ReturnRoute()
        {
            //Act 
            var route = Client.Request()
                              .Route("tests");

            //Assert
            route.Should().NotBeNull();
            route.Should().BeAssignableTo<IRoute>();
        }

        [Test]
        [TestCase("tests", 1)]
        public async Task Route_ProvidedKeyAsInt_BuildUrl(string resourceName, int key)
        {
            //Arrange  
            bool AssertRequest(HttpRequestMessage request)
                => AssertRequestUrlWithKey(request, resourceName, key);
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .Route(resourceName, key)
                        .Get()
                        .SendAsync();
        }

        [Test]
        [TestCase("tests", 1)]
        public async Task Route_ProvidedKeyAsLong_BuildUrl(string resourceName, long key)
        {
            //Arrange  
            bool AssertRequest(HttpRequestMessage request)
                => AssertRequestUrlWithKey(request, resourceName, key);
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .Route(resourceName, key)
                        .Get()
                        .SendAsync();
        }

        [Test]
        [TestCase("tests")]
        public async Task Route_ProvidedKeyAsGuid_BuildUrl(string resourceName)
        {
            //Arrange 
            var key = Guid.NewGuid();
            bool AssertRequest(HttpRequestMessage request)
                => AssertRequestUrlWithKey(request, resourceName, key);
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .Route(resourceName, key)
                        .Get()
                        .SendAsync();
        }

        //Private 
        /// <summary>
        /// Make sure <see cref="HttpRequestMessage"/>
        /// contains given header.
        /// </summary>
        private static bool AssertRequestHeader(HttpRequestMessage request, string headerKey, string headerValue)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(headerKey))
                throw new ArgumentNullException(nameof(headerKey));
            else if (string.IsNullOrWhiteSpace(headerValue))
                throw new ArgumentNullException(nameof(headerValue));

            var headers = request.Headers;
            if (!headers.Contains(headerKey))
                ThrowHeaderNotFoundException(headerKey);

            var actualValue = headers.GetValues(headerKey).FirstOrDefault();
            if (actualValue != headerValue)
                ThrowHeaderValueMissmatchException(headerKey, headerValue, actualValue);

            return true;
        }

        /// <summary>
        /// Make sure <see cref="HttpRequestMessage.RequestUri"/>
        /// has the correct route when key is provided.
        /// </summary>
        private static bool AssertRequestUrlWithKey(HttpRequestMessage request, string resourceName, object key)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            _ = key ?? throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentNullException(nameof(resourceName));

            var expectedUriEnd = Invariant($"{resourceName}/{key}").Trim('/');
            var actualUri = request.RequestUri.AbsoluteUri.Trim('/');
            if (!actualUri.EndsWith(expectedUriEnd))
                throw new Exception($"`{actualUri}` does not end with `{expectedUriEnd}`.");

            return true;
        }

        private static void ThrowHeaderNotFoundException(string headerName)
            => throw new KeyNotFoundException($"Header key ({headerName}) not found.");

        private static void ThrowHeaderValueMissmatchException(string headerName, string expectedValue, string actualValue)
            => throw new Exception($"Header=`{headerName}` value missmatch ({actualValue} != {expectedValue}).");
    }
}