using System;

namespace Epicalsoft.Reach.Api.Client.Net.Models
{
    public class IncidentSeed
    {
        public int Id { get; set; }
        public string Kind { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public DateTime UTC { get; set; }
    }
}