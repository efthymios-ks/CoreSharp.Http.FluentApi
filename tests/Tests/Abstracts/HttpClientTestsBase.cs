using Moq;
using Moq.Contrib.HttpClient;
using NUnit.Framework;
using System.Net.Http;

namespace CoreSharp.Http.FluentApi.Tests.Abstracts
{
    [TestFixture]
    public class HttpClientTestsBase
    {
        //Fields 
        private const string BaseUri = "https://www.tests.com/api/";

        //Properties
        protected HttpClient Client { get; private set; }
        protected HttpClient ClientNull { get; }
        protected Mock<HttpMessageHandler> MockHandler { get; private set; }

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
