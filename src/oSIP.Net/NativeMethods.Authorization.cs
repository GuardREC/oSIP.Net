using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_authorization_init(osip_authorization_t** header);

        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_authorization_parse(osip_authorization_t* header, IntPtr hvalue);

        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_authorization_to_str(osip_authorization_t* header, IntPtr* dest);

        [DllImport("osipparser2.dll")]
        public static extern void osip_authorization_free(osip_authorization_t* header);

        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_authorization_clone(osip_authorization_t* header, osip_authorization_t** dest);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_authorization_t
    {
        public static readonly osip_authorization_t* Null = (osip_authorization_t*) IntPtr.Zero;

        public IntPtr auth_type;
        public IntPtr username;
        public IntPtr realm;
        public IntPtr nonce;
        public IntPtr uri;
        public IntPtr response;
        public IntPtr digest;
        public IntPtr algorithm;
        public IntPtr cnonce;
        public IntPtr opaque;
        public IntPtr message_qop;
        public IntPtr nonce_count;
        public IntPtr version;
        public IntPtr targetname;
        public IntPtr gssapi_data;
        public IntPtr crand;
        public IntPtr cnum;
    };
}