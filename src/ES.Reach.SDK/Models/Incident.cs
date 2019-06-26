using System;
using System.Collections.Generic;

namespace Epicalsoft.Reach.Api.Client.Net.Models
{
    public class Incident
    {
        public Incident()
        {
            Evidences = new List<Evidence>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime UTC { get; set; }
        public bool HasEvidence { get; set; }
        public int HighlightsCount { get; set; }
        public int CommentsCount { get; set; }
        public int AlertedCount { get; set; }
        public RoadType RoadType { get; set; }
        public Classification Classification { get; set; }
        public string Nickname { get; set; }
        public long FCP { get; set; }
        public List<Evidence> Evidences { get; set; }
    }
}