using System.Collections.Generic;

namespace Epicalsoft.Reach.Api.Client.Net.Models
{
    public class VerifyFacesResult
    {
        public VerifyFacesStatusCodes Status { get; set; }
        public List<MatchingFace> Matches { get; set; }
        public int? FacesCount { get; set; }
    }

    public enum VerifyFacesStatusCodes
    {
        Ok = 1,
        ZeroResults = 2,
        OverQueryLimit = 3,
        InvalidRequest = 4,
        UnknownError = 5
    }
}