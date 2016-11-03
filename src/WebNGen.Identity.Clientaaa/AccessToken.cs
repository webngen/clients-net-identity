using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebNGen.Identity.Client
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }

    public class IdentityService
    {
        private readonly Func<HttpClient> _clientFactory;

        public IdentityService(Func<HttpClient> clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<AccessToken> CreateAccessTokenAsync(GenericClaimsIdentity principal)
        {
            using (var client = _clientFactory())
            {
                client.BaseAddress = new Uri("http://localhost:55268/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsJsonAsync("api/products", product);
                response.EnsureSuccessStatusCode();

                var formatters = new List<MediaTypeFormatter>() {
                        //new MyCustomFormatter(),
                        new JsonMediaTypeFormatter(),
                        //new XmlMediaTypeFormatter()
                    };
                var result = await response.Content.ReadAsAsync<AccessToken>(formatters);


                //// Return the URI of the created resource.
                //return response.Headers.Location;

                return result;
            }
        }
    }

    public class Claim
    {
        private Claim() { }

        public Claim(string name, string value)
            : this()
        {
            this.Name = name;
            this.Value = value;

        }
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class GenericClaimsIdentity
    {
        public string Name { get; set; }
        public string Realm { get; set; }
        public ICollection<Claim> Claims { get; set; }
    }
}
