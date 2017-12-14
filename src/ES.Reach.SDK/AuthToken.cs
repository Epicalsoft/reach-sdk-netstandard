using System;

namespace ES.Reach.SDK
{
    public class AuthToken
    {
        public long Expires_At { get; set; }
        public string Access_Token { get; set; }

        public bool IsExpiring()
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return DateTime.UtcNow > unixEpoch.AddSeconds(Expires_At - 60);
        }
    }
}