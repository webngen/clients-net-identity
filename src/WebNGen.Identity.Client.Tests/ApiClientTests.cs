using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebNGen.Identity.Client.Tests
{
    [TestClass]
    public abstract class ApiClientTests
    {
        protected const string apiUri = "http://api.webngen.net/identity", tenantId = "TEN-2134", apiKey = "AGASDF5023945";
        protected const string claimUserName = "Joe Bloggs";
        protected FakeResponseHandler fakeResponseHandler = new FakeResponseHandler();
        protected HttpClient httpClient;// =new HttpClient(fakeResponseHandler);
        protected ClaimsPrincipal principal;
        protected ApiSettings settings = new ApiSettings { TenantId = tenantId, ApiKey = apiKey, ApiUri = apiUri };

        protected virtual IEnumerable<System.Security.Claims.Claim> Claims
        {
            get { yield return new System.Security.Claims.Claim(ClaimTypes.Name, claimUserName); }
        }

        [TestInitialize]
        public virtual void Arrange()
        {
            principal = new ClaimsPrincipal(new ClaimsIdentity(this.Claims));
            httpClient = new HttpClient(fakeResponseHandler);

        }

        public abstract void Act();
    }
}
