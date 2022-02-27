using Moq;
using Moq.Contrib.HttpClient;
using NUnit.Framework;
using System.Net.Http;
using Http = System.Net.Http;

namespace CoreSharp.HttpClient.FluentApi.Tests.Abstracts
{
    [TestFixture]
    public class HttpClientTestsBase
    {
        //Fields 
        public const string BaseUri = "https://www.tests.com/api/";

        //Properties
        protected Http.HttpClient Client { get; set; }
        protected Mock<HttpMessageHandler> MockHandler { get; set; }

        //Methods
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            MockHandler = new Mock<HttpMessageHandler>();
            Client = MockHandler.CreateClient();
            Client.BaseAddress = new(BaseUri);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Client?.Dispose();
            Client = null;
            MockHandler = null;
        }
    }
}
