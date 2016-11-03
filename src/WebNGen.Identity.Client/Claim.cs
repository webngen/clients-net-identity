namespace WebNGen.Identity.Client
{
    public class Claim
    {
        private Claim() { }

        public Claim(string name, string value)
            : this()
        {
            this.Type = name;
            this.Value = value;

        }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
