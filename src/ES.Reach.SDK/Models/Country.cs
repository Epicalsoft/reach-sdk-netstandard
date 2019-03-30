namespace Epicalsoft.Reach.Api.Client.Net.Models
{
    public class Country
    {
        public string Name { get; set; }
        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }
        public short NumericCode { get; set; }
        public int CallingCode { get; set; }
        public string Culture { get; set; }
    }
}