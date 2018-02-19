using System;

namespace Epicalsoft.Reach.Api.Client.Net.Models
{
    public class SOSAlert
    {
        public User Sender { get; set; }
        public short CNC { get; set; }
        public GPSLocation Location { get; set; }
    }

    public class GPSLocation
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public DateTime UTC { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public bool Trusted { get; set; }
        public string FullName { get; set; }
        public string Nickname { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string IDN { get; set; }
    }
}