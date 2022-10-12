using CoreSharp.Http.FluentApi.Extensions;
using CoreSharp.Http.FluentApi.Steps.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using System;
using Tests.Abstracts;

namespace Tests.Extensions;

public class HttpClientExtensionsTests : HttpClientTestsBase
{
    // Methods
    [Test]
    public void Request_ClienItNull_ThrowArgumentNullException()
    {
        // Act
        Action action = () => ClientNull.Request();

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Request_WhenCalled_ReturnRequest()
    {
        // Act 
        var request = Client.Request();

        // Assert
        request.Should().NotBeNull();
        request.Should().BeAssignableTo<IRequest>();
    }
}
