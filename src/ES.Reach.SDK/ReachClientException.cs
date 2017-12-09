using System;

namespace ES.Reach.SDK
{
    public class ReachClientException : Exception
    {
        public string ErrorMessage { get; set; }
        public ReachExceptionCodes ErrorCode { get; set; }

        internal static ReachClientException Create(ReachApiException apiException)
        {
            if (apiException.AppExceptionCode == "Auth_TokenExpired")
                return new ReachClientException { ErrorCode = ReachExceptionCodes.AuthTokenExpired };
            return new ReachClientException { ErrorCode = ReachExceptionCodes.Unknown };
        }
    }

    public enum ReachExceptionCodes
    {
        Unknown = 0,
        Forbidden = 1,
        Unauthorized = 2,
        AuthTokenExpired = 3
    }
}