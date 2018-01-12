using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2")]
        public static extern ErrorCode osip_authentication_info_init(osip_authentication_info_t** header);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_authentication_info_parse(osip_authentication_info_t* header, IntPtr hvalue);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_authentication_info_to_str(osip_authentication_info_t* header, IntPtr* dest);

        [DllImport("osipparser2")]
        public static extern void osip_authentication_info_free(osip_authentication_info_t* header);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_authentication_info_clone(osip_authentication_info_t* header, osip_authentication_info_t** dest);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_authentication_info_t
    {
        public static readonly osip_authentication_info_t* Null = (osip_authentication_info_t*) IntPtr.Zero;

        public IntPtr auth_type;
        public IntPtr nextnonce;
        public IntPtr qop_options;
        public IntPtr rspauth;
        public IntPtr cnonce;
        public IntPtr nonce_count;
        public IntPtr snum;
        public IntPtr srand;
        public IntPtr realm;
        public IntPtr targetname;
        public IntPtr opaque;
    }
}