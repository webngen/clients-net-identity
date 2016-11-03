using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebNGen.Identity.Client.Tests
{
    [TestClass]
    public abstract class AccessTokenTests : ApiClientTests
    {
        protected IdentityService identityService;
        protected Mock<IIdentityDiscoveryService> discoveryService = new Mock<IIdentityDiscoveryService>();

        [TestInitialize]
        public override void Arrange()
        {
            base.Arrange();

            identityService = new IdentityService(() => httpClient, discoveryService.Object, () => settings);
        }
    }

    [TestClass]
    public class When_create_access_token_from_principal : AccessTokenTests
    {
        private string tokenContent = "ABCDEF";
        private DateTime tokenExpires = DateTime.UtcNow.AddHours(1);
        private Uri requestUri;
        private HttpRequestMessage requestSent;
        private CreateTokenFromPrincipalRequest requestBody;
        private Token token;


        [TestInitialize]
        public override void Arrange()
        {
            base.Arrange();

            var tokensUri = "http://test.api.webngen.net/identity/tokens";
            requestUri = new Uri(tokensUri);
            discoveryService.Setup(x => x.GetTokensUri()).ReturnsAsync(tokensUri);

            var tokenResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent($"{{ \"AccessToken\":\"{tokenContent}\", \"Expires\":\"{tokenExpires.ToString("o")}\", \"TokenType\":\"\"}}"), };
            tokenResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            fakeResponseHandler.AddFakeResponse(requestUri, tokenResponse);

            Act();
        }

        public override void Act()
        {
            token = identityService.CreateAccessTokenAsync(principal).Result;
            requestSent = fakeResponseHandler.FakeRequests[requestUri];
            //requestBody = requestSent.Content.ReadAsAsync<CreateTokenFromPrincipalRequest>().Result;
        }

        [TestMethod]
        public void Should_send_api_key()
        {
            Assert.AreEqual(apiKey, requestSent.Headers.Single(x => x.Key == "x-apikey").Value.First());
        }

        [TestMethod]
        public void Should_send_tenantid()
        {
            Assert.AreEqual(tenantId, requestSent.Headers.Single(x => x.Key == "x-tenantid").Value.First());
        }

        [TestMethod]
        public void Should_send_request_method_post()
        {
            Assert.AreEqual("POST", requestSent.Method.Method);
        }

        //TODO: come up with way to test this, currently the stream is closed before this test is executed
        //[TestMethod]
        //public void Should_send_request_body_claims()
        //{
        //    Assert.AreEqual(claimUserName, requestBody.Claims.Single(x => x.Name == ClaimTypes.Name).Value);
        //}

        [TestMethod]
        public void Should_return_access_token()
        {
            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void Should_return_access_token_value()
        {
            Assert.AreEqual(tokenContent, token.AccessToken);
        }

        [TestMethod]
        public void Should_return_access_token_expiry()
        {
            Assert.AreEqual(tokenExpires, token.Expires);
        }
    }

}
