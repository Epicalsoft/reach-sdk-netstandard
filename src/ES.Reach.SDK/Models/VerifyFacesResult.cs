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
        OK = 1,
        ZERO_RESULTS = 2,
        OVER_QUERY_LIMIT = 3,
        INVALID_REQUEST = 4,
        UNKNOWN_ERROR = 5
    }
}