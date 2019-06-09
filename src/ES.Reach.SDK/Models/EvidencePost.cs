namespace Epicalsoft.Reach.Api.Client.Net.Models
{
    public class EvidencePost
    {
        public EvidencePost(int mediaFileId, bool protect)
        {
            MediaFileId = mediaFileId;
            Protected = protect;
        }

        public int MediaFileId { get; private set; }
        public bool Protected { get; private set; }
    }
}