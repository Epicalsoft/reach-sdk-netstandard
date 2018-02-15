using System;

namespace Epicalsoft.Reach.NET
{
    public class ReachClientException : Exception
    {
        public string ErrorMessage { get; set; }
        public ReachExceptionCodes ErrorCode { get; set; }
    }

    public enum ReachExceptionCodes
    {
        ServerUnknown = 0,
        Forbidden = 1,
        Unauthorized = 2,
        AuthTokenExpired = 3,
        ClientUnknown = 4
    }
}