namespace oSIP.Net
{
    internal enum ErrorCode
    {
        Success = 0,
        UndefinedError = -1,
        BadParameter = -2,
        WrongState = -3,
        NoMemory = -4,
        SyntaxError = -5,
        NotFound = -6,
        ApiNotInitialized = -7,
        NoNetwork = -10,
        PortBusy = -11,
        UnknownHost = -12,
        DiskFull = -30,
        NoRights = -31,
        FileNotExist = -32,
        Timeout = -50,
        TooMuchCall = -51,
        WrongFormat = -52,
        NoCommonCodec = -53
    }
}