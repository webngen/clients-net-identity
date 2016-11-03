using System;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebNGen.Identity.Client.Tests
{
    [TestClass]
    public abstract class DiscoveryServiceTests : ApiClientTests
    {
        protected const string enterpriseTokenUri = "http://api.webngen.net/identity/enterprise/tokens";
        protected const string tokenUri = "http://api.webngen.net/identity/tokens";
        protected IdentityDiscoveryService discoveryService;

        protected virtual string DiscoveryContent
            =>
                $"{{\"Links\":[{{\"uri\":\"{tokenUri}\",\"rel\":\"tokens\",\"mediaType\":\"application/json\"}},{{\"uri\":\"{enterpriseTokenUri}\",\"rel\":\"enterpriseTokens\",\"mediaType\":\"application/json\"}}]}}"
            ;

        [TestInitialize]
        public override void Arrange()
        {
            base.Arrange();
            
            var tokenResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(DiscoveryContent), };
            tokenResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            fakeResponseHandler.AddFakeResponse(new Uri(apiUri), tokenResponse);

            discoveryService = new IdentityDiscoveryService(() => httpClient, () => settings);
        }
    }

    [TestClass]
    public class When_get_token_uri : DiscoveryServiceTests
    {
        private string result;

        [TestInitialize]
        public override void Arrange()
        {
            base.Arrange();

            Act();
        }

        public override void Act()
        {
            result = base.discoveryService.GetTokensUri().Result;
        }

        [TestMethod]
        public void Should_return_token_uri_from_api_discovery_resource()
        {
            Assert.AreEqual(enterpriseTokenUri, result);
        }
    }

    [TestClass]
    public class When_get_enterprise_token_uri : DiscoveryServiceTests
    {
        private string result;

        [TestInitialize]
        public override void Arrange()
        {
            base.Arrange();

            Act();
        }

        public override void Act()
        {
            result = base.discoveryService.GetEnterpriseTokensUri().Result;
        }

        [TestMethod]
        public void Should_return_token_uri_from_api_discovery_resource()
        {
            Assert.AreEqual(enterpriseTokenUri, result);
        }
    }

}
