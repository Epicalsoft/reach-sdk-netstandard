namespace ES.Reach.SDK.Models
{
    public class SuspectVerifyResult
    {
        public bool IsSuspect { get; set; }
        public double Confidence { get; set; }
        public int IncidentId { get; set; }
    }
}