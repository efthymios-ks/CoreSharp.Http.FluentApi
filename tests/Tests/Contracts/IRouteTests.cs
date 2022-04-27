using CoreSharp.Http.FluentApi.Contracts;
using CoreSharp.Http.FluentApi.Extensions;
using CoreSharp.Http.FluentApi.Tests.Abstracts;
using FluentAssertions;
using Moq.Contrib.HttpClient;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Tests.Contracts
{
    public class IRouteTests : HttpClientTestsBase
    {
        //Methods
        [Test]
        public void Get_ClientIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => ClientNull.Request()
                                            .Route("tests")
                                            .Get();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Get_WhenCalled_ReturnIQueryMethod()
        {
            //Act
            var result = Client.Request()
                               .Route("tests")
                               .Get();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IQueryMethod>();
        }

        [Test]
        public async Task Get_WhenCalled_PerformsGetRequest()
        {
            //Arrange
            static bool AssertRequest(HttpRequestMessage request)
               => AssertRequestMethod(request, HttpMethod.Get);
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .Route("tests")
                        .Get()
                        .SendAsync();
        }

        [Test]
        public void Post_ClientIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => ClientNull.Request()
                                            .Route("tests")
                                            .Post();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Post_WhenCalled_ReturnIContentMethod()
        {
            //Act
            var result = Client.Request()
                               .Route("tests")
                               .Post();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IContentMethod>();
        }

        [Test]
        public async Task Post_WhenCalled_PerformsPostRequest()
        {
            //Arrange
            static bool AssertRequest(HttpRequestMessage request)
               => AssertRequestMethod(request, HttpMethod.Post);
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .Route("tests")
                        .Post()
                        .SendAsync();
        }

        [Test]
        public void Put_ClientIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => ClientNull.Request()
                                            .Route("tests")
                                            .Put();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Put_WhenCalled_ReturnIContentMethod()
        {
            //Act
            var result = Client.Request()
                               .Route("tests")
                               .Put();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IContentMethod>();
        }

        [Test]
        public async Task Put_WhenCalled_PerformsPutRequest()
        {
            //Arrange
            static bool AssertRequest(HttpRequestMessage request)
               => AssertRequestMethod(request, HttpMethod.Put);
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .Route("tests")
                        .Put()
                        .SendAsync();
        }

        [Test]
        public void Patch_ClientIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => ClientNull.Request()
                                            .Route("tests")
                                            .Patch();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Patch_WhenCalled_ReturnIContentMethod()
        {
            //Act
            var result = Client.Request()
                               .Route("tests")
                               .Patch();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IContentMethod>();
        }

        [Test]
        public async Task Patch_WhenCalled_PerformsPatchRequest()
        {
            //Arrange
            static bool AssertRequest(HttpRequestMessage request)
               => AssertRequestMethod(request, HttpMethod.Patch);
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .Route("tests")
                        .Patch()
                        .SendAsync();
        }

        [Test]
        public void Delete_ClientIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => ClientNull.Request()
                                            .Route("tests")
                                            .Delete();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Delete_WhenCalled_ReturnIMethod()
        {
            //Act
            var result = Client.Request()
                               .Route("tests")
                               .Delete();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IMethod>();
        }

        [Test]
        public async Task Delete_WhenCalled_PerformsDeleteRequest()
        {
            //Arrange
            static bool AssertRequest(HttpRequestMessage request)
               => AssertRequestMethod(request, HttpMethod.Delete);
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .Route("tests")
                        .Delete()
                        .SendAsync();
        }

        [Test]
        public void Head_ClientIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => ClientNull.Request()
                                            .Route("tests")
                                            .Head();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Head_WhenCalled_ReturnIMethod()
        {
            //Act
            var result = Client.Request()
                               .Route("tests")
                               .Head();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IMethod>();
        }

        [Test]
        public async Task Head_WhenCalled_PerformsHeadRequest()
        {
            //Arrange
            static bool AssertRequest(HttpRequestMessage request)
               => AssertRequestMethod(request, HttpMethod.Head);
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .Route("tests")
                        .Head()
                        .SendAsync();
        }

        [Test]
        public void Options_ClientIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => ClientNull.Request()
                                            .Route("tests")
                                            .Options();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Options_WhenCalled_ReturnIMethodWithResponse()
        {
            //Act
            var result = Client.Request()
                               .Route("tests")
                               .Options();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IMethodWithResponse>();
        }

        [Test]
        public async Task Options_WhenCalled_PerformsOptionsRequest()
        {
            //Arrange
            static bool AssertRequest(HttpRequestMessage request)
               => AssertRequestMethod(request, HttpMethod.Options);
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .Route("tests")
                        .Options()
                        .SendAsync();
        }

        [Test]
        public void Trace_ClientIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => ClientNull.Request()
                                            .Route("tests")
                                            .Trace();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Trace_WhenCalled_ReturnIMethodWithResponse()
        {
            //Act
            var result = Client.Request()
                               .Route("tests")
                               .Trace();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IMethod>();
        }

        [Test]
        public async Task Trace_WhenCalled_PerformsTraceRequest()
        {
            //Arrange
            static bool AssertRequest(HttpRequestMessage request)
               => AssertRequestMethod(request, HttpMethod.Trace);
            MockHandler.SetupRequest(request => AssertRequest(request))
                       .ReturnsResponse(HttpStatusCode.OK);

            //Act
            await Client.Request()
                        .Route("tests")
                        .Trace()
                        .SendAsync();
        }

        //Private 
        /// <summary>
        /// Make sure <see cref="HttpRequestMessage.Method"/>
        /// is set correctly.
        /// </summary>
        private static bool AssertRequestMethod(HttpRequestMessage request, HttpMethod httpMethod)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            _ = httpMethod ?? throw new ArgumentNullException(nameof(httpMethod));

            var expectedMethod = httpMethod.Method;
            var actualMethod = request.Method.Method;
            if (actualMethod != expectedMethod)
                throw new Exception($"{nameof(HttpMethod)} missmatch (`{actualMethod}` != `{expectedMethod}`).");

            return true;
        }
    }
}
