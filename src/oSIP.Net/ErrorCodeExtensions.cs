using System.Collections.Generic;

namespace oSIP.Net
{
    public static class ErrorCodeExtensions
    {
        private static readonly Dictionary<ErrorCode, string> ErrorMessages;

        static ErrorCodeExtensions()
        {
            ErrorMessages = new Dictionary<ErrorCode, string>
            {
                {ErrorCode.UndefinedError, "Undefined error"},
                {ErrorCode.BadParameter, "Bad parameter"},
                {ErrorCode.WrongState, "Wrong state"},
                {ErrorCode.NoMemory, "Allocation failure"},
                {ErrorCode.SyntaxError, "Syntax error"},
                {ErrorCode.NotFound, "Not found"},
                {ErrorCode.ApiNotInitialized, "API not initialized"},
                {ErrorCode.NoNetwork, "No network"},
                {ErrorCode.PortBusy, "Busy port"},
                {ErrorCode.UnknownHost, "Unknown host"},
                {ErrorCode.DiskFull, "Disk full"},
                {ErrorCode.NoRights, "No rights"},
                {ErrorCode.FileNotExist, "File not found"},
                {ErrorCode.Timeout, "Time out"},
                {ErrorCode.TooMuchCall, "Too much call"},
                {ErrorCode.WrongFormat, "Wrong format"},
                {ErrorCode.NoCommonCodec, "No common codec"}
            };
        }

        public static void ThrowOnError(this ErrorCode code)
        {
            if ((int) code >= 0)
            {
                return;
            }

            if (!ErrorMessages.TryGetValue(code, out string errorMessage))
            {
                errorMessage = $"An unknown error occurred ({code})";
            }

            throw new SipException(errorMessage);
        }
    }
}