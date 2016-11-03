using System;

namespace WebNGen.Identity.Client
{
    public class Token
    {
        public string AccessToken { get; set; }
        public DateTime Expires { get; set; }
    }
}
