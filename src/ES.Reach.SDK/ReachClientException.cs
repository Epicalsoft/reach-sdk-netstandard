using System;

namespace Epicalsoft.Reach.Api.Client.Net
{
    public class ReachClientException : Exception
    {
        public ReachClientException(ReachExceptionCodes errorCode)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorCode.ToString();
        }

        public ReachClientException(ReachExceptionCodes errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public ReachClientException(ReachExceptionCodes errorCode, Exception exception) : base(exception.Message, exception)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorCode.ToString();
        }

        public ReachExceptionCodes ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum ReachExceptionCodes
    {
        ServerUnknown = 0,
        Forbidden = 1,
        Unauthorized = 2,
        AuthTokenExpired = 3,
        ClientUnknown = 4,
        ConnectionError = 5,
        ClientError = 6
    }
}