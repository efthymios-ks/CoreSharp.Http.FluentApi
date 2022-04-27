using CoreSharp.Http.FluentApi.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreSharp.Http.FluentApi.Extensions.Tests
{
    [TestFixture]
    public class HttpResponseMessageExtensionsTests
    {
        [Test]
        public async Task EnsureSuccessAsync_ResponseIsNull_ThrowArgumentNullException()
        {
            //Arrange
            HttpResponseMessage response = null;

            //Act
            Func<Task> task = () => response.EnsureSuccessAsync();

            //Assert 
            await task.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.Created)]
        public async Task EnsureSuccessAsync_StatusIsOk_Return(HttpStatusCode httpStatusCode)
        {
            //Arrange
            using var response = new HttpResponseMessage(httpStatusCode);

            //Act
            Func<Task> task = () => response.EnsureSuccessAsync();

            //Assert 
            await task.Should().NotThrowAsync<HttpResponseException>();
        }

        [Test]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.Unauthorized)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.NotImplemented)]
        public async Task EnsureSuccessAsync_StatusHasError_ThrowHttpResponseException(HttpStatusCode httpStatusCode)
        {
            //Arrange
            var method = HttpMethod.Get;
            const string requestUrl = "http://www.tests.com/api/";
            const string content = "123";
            using var request = new HttpRequestMessage(method, requestUrl);
            using var response = new HttpResponseMessage(httpStatusCode)
            {
                RequestMessage = request,
                Content = new StringContent(content)
            };

            //Act
            Func<Task> task = () => response.EnsureSuccessAsync();

            //Assert 
            var assertion = await task.Should().ThrowAsync<HttpResponseException>();
            var exception = assertion.Which;
            exception.RequestUrl.Should().Be(requestUrl);
            exception.RequestMethod.Should().Contain(method.Method);
            exception.ResponseStatusCode.Should().Be(httpStatusCode);
            exception.ResponseContent.Should().Be(content);
        }
    }
}
