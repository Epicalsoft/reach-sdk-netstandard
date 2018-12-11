namespace Epicalsoft.Reach.Api.Client.Net.Models
{
    public class EvidencePost
    {
        public EvidencePost(string source, EvidenceKind kind, bool protect)
        {
            Source = source;
            Kind = kind;
            Protected = protect;
        }

        public string Source { get; private set; }
        public EvidenceKind Kind { get; private set; }
        public bool Protected { get; private set; }
    }
}