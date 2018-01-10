using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_call_id_init(osip_call_id_t** header);

        [DllImport("osipparser2.dll")]
        public static extern void osip_call_id_free(osip_call_id_t* header);

        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_call_id_parse(osip_call_id_t* header, IntPtr hvalue);

        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_call_id_to_str(osip_call_id_t* header, IntPtr* dest);

        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_call_id_clone(osip_call_id_t* header, osip_call_id_t** dest);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_call_id_t
    {
        public static readonly osip_call_id_t* Null = (osip_call_id_t*) IntPtr.Zero;

        public IntPtr number;
        public IntPtr host;
    }
}