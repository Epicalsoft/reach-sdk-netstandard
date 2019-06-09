using System;

namespace Epicalsoft.Reach.Api.Client.Net.Models
{
    public class MediaFileData
    {
        public Guid Code { get; set; }
        public MediaFileTarget Target { get; set; }
        public MediaFileKind Kind { get; set; }
        public string Data { get; set; }
        public string Filename { get; set; }
    }
}