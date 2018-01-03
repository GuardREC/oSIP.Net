using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2.dll")]
        public static extern int parser_init();

        [DllImport("osipparser2.dll")]
        public static extern void osip_free(void* ptr);
    }

    internal static class ReturnCode
    {
        public const int OSIP_SUCCESS = 0;
        public const int OSIP_UNDEFINED_ERROR = -1;
        public const int OSIP_BADPARAMETER = -2;
        public const int OSIP_WRONG_STATE = -3;
        public const int OSIP_NOMEM = -4;
        public const int OSIP_SYNTAXERROR = -5;
        public const int OSIP_NOTFOUND = -6;
        public const int OSIP_API_NOT_INITIALIZED = -7;
        public const int OSIP_NO_NETWORK = -10;
        public const int OSIP_PORT_BUSY = -11;
        public const int OSIP_UNKNOWN_HOST = -12;
        public const int OSIP_DISK_FULL = -30;
        public const int OSIP_NO_RIGHTS = -31;
        public const int OSIP_FILE_NOT_EXIST = -32;
        public const int OSIP_TIMEOUT = -50;
        public const int OSIP_TOOMUCHCALL = -51;
        public const int OSIP_WRONG_FORMAT = -52;
        public const int OSIP_NOCOMMONCODEC = -53;
    }
}