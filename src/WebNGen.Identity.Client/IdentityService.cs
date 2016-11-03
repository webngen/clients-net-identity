using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebNGen.Identity.Client
{

    public class IdentityService : IIdentityService
    {
        private readonly Func<HttpClient> _clientFactory;
        private readonly IIdentityDiscoveryService _discoveryService;
        private readonly Func<ApiSettings> _apiSettings;

        public IdentityService(Func<HttpClient> clientFactory, IIdentityDiscoveryService discoveryService, Func<ApiSettings> apiSettings)
        {
            _clientFactory = clientFactory;
            _discoveryService = discoveryService;
            _apiSettings = apiSettings;
        }

        public async Task<Token> CreateAccessTokenAsync(ClaimsPrincipal principal)
        {
            using (var client = _clientFactory())
            {
                var uri = new Uri(await _discoveryService.GetTokensUri());
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var settings = _apiSettings();
                client.DefaultRequestHeaders.Add("x-apikey", settings.ApiKey);
                client.DefaultRequestHeaders.Add("x-tenantid", settings.TenantId);

                var req = new CreateTokenFromPrincipalRequest { Claims = principal.Claims.Select(x => new Claim(x.Type, x.Value)).ToList(), Realm = settings.Realm, Name = principal.Identity.Name };
                HttpResponseMessage response = await client.PostAsJsonAsync(uri, req);
                response.EnsureSuccessStatusCode();

                var formatters = new List<MediaTypeFormatter>() {
                        //new MyCustomFormatter(),
                        new JsonMediaTypeFormatter(),
                        //new XmlMediaTypeFormatter()
                    };
                var result = await response.Content.ReadAsAsync<Token>(formatters);


                //// Return the URI of the created resource.
                //return response.Headers.Location;

                return result;
            }
        }
    }
}
