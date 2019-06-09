using System;
using System.Collections.Generic;

namespace Epicalsoft.Reach.Api.Client.Net.Models
{
    public class IncidentPost
    {
        public string ClassificationCode { get; private set; }
        public string Description { get; private set; }
        public string PrivateNotes { get; private set; }
        public bool Anonymous { get; private set; }
        public short RoadTypeCode { get; private set; }
        public IncidentState State { get; private set; }
        public string Address { get; private set; }
        public double Lat { get; private set; }
        public double Lng { get; private set; }
        public short CNC { get; private set; }
        public DateTime UTC { get; private set; }
        public List<EvidencePost> Evidences { get; private set; }

        public IncidentPost(Classification classification, string description, string privateNotes, bool anonymous, RoadType roadType, IncidentState state, string address, double latitude, double longitude, Country country, DateTime factsUTC)
        {
            if (classification.Code.Length < 6)
                throw new ArgumentException("Invalid classification");

            ClassificationCode = classification.Code;
            Description = description;
            PrivateNotes = privateNotes;
            Anonymous = anonymous;
            RoadTypeCode = roadType.Code;
            State = state;
            Address = address;
            Lat = latitude;
            Lng = longitude;
            CNC = country.NumericCode;
            UTC = factsUTC;

            Evidences = new List<EvidencePost>();
        }
    }
}