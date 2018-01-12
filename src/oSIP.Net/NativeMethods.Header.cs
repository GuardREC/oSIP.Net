using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2")]
        public static extern ErrorCode osip_header_init(osip_header_t** header);

        [DllImport("osipparser2")]
        public static extern void osip_header_free(osip_header_t* header);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_header_to_str(osip_header_t* header, IntPtr* dest);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_header_clone(osip_header_t* header, osip_header_t** dest);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_header_t
    {
        public static readonly osip_header_t* Null = (osip_header_t*) IntPtr.Zero;

        public IntPtr hname;
        public IntPtr hvalue;
    }
}