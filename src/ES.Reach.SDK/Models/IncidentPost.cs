using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Epicalsoft.Reach.Api.Client.Net.Models
{
    public class IncidentPost
    {
        public string ClassificationCode { get; private set; }
        public string Description { get; private set; }
        public string PrivateNotes { get; private set; }
        public bool Anonymous { get; private set; }
        public short RoadTypeCode { get; private set; }
        public IncidentState State { get; private set; }
        public string Address { get; private set; }
        public double Lat { get; private set; }
        public double Lng { get; private set; }
        public short CNC { get; private set; }
        public DateTime UTC { get; private set; }
        public List<EvidencePost> Evidences { get; private set; }

        public IncidentPost(Classification classification, string description, string privateNotes, bool anonymous, RoadType roadType, IncidentState state, string address, double latitude, double longitude, Country country, DateTime factsUTC)
        {
            if (classification.Code.Length < 6)
                throw new ArgumentException("Invalid classification");

            ClassificationCode = classification.Code;
            Description = description;
            PrivateNotes = privateNotes;
            Anonymous = anonymous;
            RoadTypeCode = roadType.Code;
            State = state;
            Address = address;
            Lat = latitude;
            Lng = longitude;
            CNC = country.CNC;
            UTC = factsUTC;

            Evidences = new List<EvidencePost>();
        }

        public EvidencePost AddEvidence(byte[] source, EvidenceKind kind, bool protect)
        {
            EvidencePost evidencePost = null;

            if (kind == EvidenceKind.Image)
            {
                evidencePost = new EvidencePost(ProcessEvidence(source), kind, protect);
                Evidences.Add(evidencePost);
            }

            return evidencePost;
        }

        private string ProcessEvidence(byte[] image)
        {
            using (var inputStream = new MemoryStream(image))
            {
                using (var outputStream = new MemoryStream())
                {
                    int width;
                    int height;
                    var originalImage = new Bitmap(inputStream);

                    int to = 2048;
                    if (originalImage.Width <= 2048 || originalImage.Height <= 2048)
                        to = originalImage.Width > originalImage.Height ? originalImage.Width : originalImage.Height;

                    if (originalImage.Width > originalImage.Height)
                    {
                        width = to;
                        height = to * originalImage.Height / originalImage.Width;
                    }
                    else
                    {
                        height = to;
                        width = to * originalImage.Width / originalImage.Height;
                    }

                    var thumbnailImage = new Bitmap(width, height);

                    using (var graphics = Graphics.FromImage(thumbnailImage))
                    {
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.DrawImage(originalImage, 0, 0, width, height);
                    }

                    thumbnailImage.Save(outputStream, ImageFormat.Jpeg);
                    return Convert.ToBase64String(outputStream.ToArray());
                }
            }
        }
    }
}