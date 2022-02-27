using CoreSharp.HttpClient.FluentApi.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Extensions.Tests
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
        public async Task EnsureSuccessAsync_StatusIsOk_Return(HttpStatusCode code)
        {
            //Arrange
            using var response = new HttpResponseMessage(code);

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
        public async Task EnsureSuccessAsync_StatusHasError_ThrowHttpResponseException(HttpStatusCode code)
        {
            //Arrange
            using var response = new HttpResponseMessage(code);

            //Act
            Func<Task> task = () => response.EnsureSuccessAsync();

            //Assert 
            await task.Should().ThrowAsync<HttpResponseException>();
        }
    }
}
