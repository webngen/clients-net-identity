namespace WebNGen.Identity.Client
{
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
}
