using CoreSharp.Http.FluentApi.Contracts;
using CoreSharp.Http.FluentApi.Tests.Abstracts;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace CoreSharp.Http.FluentApi.Extensions.Tests;

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
