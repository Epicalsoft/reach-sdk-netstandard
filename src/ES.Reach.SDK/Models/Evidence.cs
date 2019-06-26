namespace Epicalsoft.Reach.Api.Client.Net.Models
{
    public class Evidence
    {
        public int Id { get; set; }
        public int IncidentId { get; set; }
        public EvidenceKind Kind { get; set; }
        public string SourceUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string OriginalUrl { get; set; }
        public bool Protected { get; set; }
    }
}