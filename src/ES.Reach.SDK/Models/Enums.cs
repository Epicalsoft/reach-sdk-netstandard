namespace Epicalsoft.Reach.Api.Client.Net.Models
{
    public enum AcceptLanguage
    {
        English = 1,
        Spanish = 2
    }

    public enum IncidentState
    {
        Archived = 1,
        General = 2,
        Draft = 3
    }

    public enum EvidenceKind
    {
        Image = 1,
        Audio = 2,
        Video = 3,
        Other = 99
    }

    public enum MediaFileKind
    {
        Image = 1,
        Audio = 2,
        Video = 3
    }

    public enum MediaFileTarget
    {
        Evidences = 1
    }

    public enum ClassificationGroup
    {
        MedicalIncidents = 1,
        PublicProtection = 2,
        HumanSecurity = 3,
        PublicAdministration = 4
    }
}