using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.HttpClient.FluentApi.Tests.Abstracts;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace CoreSharp.HttpClient.FluentApi.Extensions.Tests
{
    public class HttpClientExtensionsTests : HttpClientTestsBase
    {
        //Methods
        [Test]
        public void Request_ClienItNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => ClientNull.Request();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Request_WhenCalled_ReturnRequest()
        {
            //Act 
            var request = Client.Request();

            //Assert
            request.Should().NotBeNull();
            request.Should().BeAssignableTo<IRequest>();
        }
    }
}
