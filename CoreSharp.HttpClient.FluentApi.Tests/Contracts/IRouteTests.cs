using CoreSharp.HttpClient.FluentApi.Contracts;
using CoreSharp.HttpClient.FluentApi.Extensions;
using CoreSharp.HttpClient.FluentApi.Tests.Abstracts;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace CoreSharp.HttpClient.FluentApi.Tests.Contracts
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
    }
}
