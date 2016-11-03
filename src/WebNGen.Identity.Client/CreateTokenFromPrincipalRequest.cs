using System.Collections.Generic;

namespace WebNGen.Identity.Client
{
    public class CreateTokenFromPrincipalRequest
    {
        public string Name { get; set; }
        public string Realm { get; set; }
        public ICollection<Claim> Claims { get; set; }
    }
}
