using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebNGen.Identity.Client
{
    public class IdentityDiscoveryService : IIdentityDiscoveryService
    {
        private readonly Func<HttpClient> _clientFactory;
        private readonly Func<ApiSettings> _apiSettings;

        public IdentityDiscoveryService(Func<HttpClient> clientFactory, Func<ApiSettings> apiSettings)
        {
            _clientFactory = clientFactory;
            _apiSettings = apiSettings;
        }

        public async Task<string> GetTokensUri()
        {

            using (var client = _clientFactory())
            {
                var settings = _apiSettings();
                var uri = new Uri(settings.ApiUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-apikey", settings.ApiKey);
                client.DefaultRequestHeaders.Add("x-tenantid", settings.TenantId);

                var req = new { };
                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                var formatters = new List<MediaTypeFormatter>() {
                        //new MyCustomFormatter(),
                        new JsonMediaTypeFormatter(),
                        //new XmlMediaTypeFormatter()
                    };
                var result = await response.Content.ReadAsAsync<DiscoveryResource>(formatters);


                //// Return the URI of the created resource.
                //return response.Headers.Location;

                return result.Links.Single(x => x.Rel == "tokens").Uri;
            }

        }
    }

    internal class DiscoveryResource
    {
        public Link[] Links { get; set; }
    }

    internal class Link
    {
        public string Uri { get; set; }
        public string Rel { get; set; }
        public string MediaType { get; set; }
    }
}
