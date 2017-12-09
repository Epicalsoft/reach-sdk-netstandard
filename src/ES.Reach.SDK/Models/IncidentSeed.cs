using System;

namespace ES.Reach.SDK.Models
{
    public class IncidentSeed
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public DateTime UTC { get; set; }
    }
}