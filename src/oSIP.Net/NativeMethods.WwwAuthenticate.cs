using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2")]
        public static extern ErrorCode osip_www_authenticate_init(osip_www_authenticate_t** header);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_www_authenticate_parse(osip_www_authenticate_t* header, IntPtr hvalue);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_www_authenticate_to_str(osip_www_authenticate_t* header, IntPtr* dest);

        [DllImport("osipparser2")]
        public static extern void osip_www_authenticate_free(osip_www_authenticate_t* header);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_www_authenticate_clone(osip_www_authenticate_t* header, osip_www_authenticate_t** dest);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_www_authenticate_t
    {
        public static readonly osip_www_authenticate_t* Null = (osip_www_authenticate_t*) IntPtr.Zero;

        public IntPtr auth_type;
        public IntPtr realm;
        public IntPtr domain;
        public IntPtr nonce;
        public IntPtr opaque;
        public IntPtr stale;
        public IntPtr algorithm;
        public IntPtr qop_options;
        public IntPtr version;
        public IntPtr targetname;
        public IntPtr gssapi_data;
    }
}