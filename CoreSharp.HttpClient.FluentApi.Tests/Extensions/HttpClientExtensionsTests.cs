using CoreSharp.HttpClient.FluentApi.Contracts;
using FluentAssertions;
using NUnit.Framework;
using System;
using Http = System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Extensions.Tests
{
    [TestFixture]
    public class HttpClientExtensionsTests
    {
        [Test]
        public void Request_ClienItNull_ThrowArgumentNullException()
        {
            //Arrange
            Http.HttpClient client = null;

            //Act
            Action action = () => client.Request();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Request_WhenCalled_ReturnRequest()
        {
            //Act
            using var client = new Http.HttpClient();
            var request = client.Request();

            //Assert
            request.Should().NotBeNull();
            request.Should().BeAssignableTo<IRequest>();
        }
    }
}
