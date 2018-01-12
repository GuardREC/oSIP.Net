using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2")]
        public static extern ErrorCode osip_via_init(osip_via_t** header);

        [DllImport("osipparser2")]
        public static extern void osip_via_free(osip_via_t* header);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_via_parse(osip_via_t* header, IntPtr hvalue);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_via_to_str(osip_via_t* header, IntPtr* dest);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_via_clone(osip_via_t* header, osip_via_t** dest);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_via_t
    {
        public static readonly osip_via_t* Null = (osip_via_t*) IntPtr.Zero;

        public IntPtr version;
        public IntPtr protocol;
        public IntPtr host;
        public IntPtr port;
        public IntPtr comment;
        public osip_list_t via_params;
    }
}