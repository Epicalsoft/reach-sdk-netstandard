using System.Collections.Generic;

namespace Epicalsoft.Reach.Api.Client.Net.Models
{
    public class FacesVerificationResult
    {
        public FacesVerificationStatusCodes Status { get; set; }
        public List<MatchingFace> Matches { get; set; }
        public int? FacesCount { get; set; }
    }

    public enum FacesVerificationStatusCodes
    {
        OK = 1,
        ZERO_RESULTS = 2,
        OVER_QUERY_LIMIT = 3,
        INVALID_REQUEST = 4,
        UNKNOWN_ERROR = 5
    }
}