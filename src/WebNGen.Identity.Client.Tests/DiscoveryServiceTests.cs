using System;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebNGen.Identity.Client.Tests
{
    [TestClass]
    public abstract class DiscoveryServiceTests : ApiClientTests
    {
        protected IdentityDiscoveryService discoveryService;

        [TestInitialize]
        public override void Arrange()
        {
            base.Arrange();

            discoveryService = new IdentityDiscoveryService(() => httpClient, () => settings);
        }
    }

    [TestClass]
    public class When_get_token_uri : DiscoveryServiceTests
    {
        private string tokenUri = "http://api.webngen.net/identity/tokens";
        private string result;

        [TestInitialize]
        public override void Arrange()
        {
            base.Arrange();
            
            var tokenResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent($"{{\"Links\":[{{\"uri\":\"{tokenUri}\",\"rel\":\"tokens\",\"mediaType\":\"application/json\"}}]}}"), };
            tokenResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            fakeResponseHandler.AddFakeResponse(new Uri(apiUri), tokenResponse);

            Act();
        }

        public override void Act()
        {
            result = base.discoveryService.GetTokensUri().Result;
        }

        [TestMethod]
        public void Should_return_token_uri_from_api_discovery_resource()
        {
            Assert.AreEqual(tokenUri, result);
        }
    }
}
