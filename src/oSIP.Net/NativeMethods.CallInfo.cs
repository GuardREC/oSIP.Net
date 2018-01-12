using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2")]
        public static extern ErrorCode osip_call_info_init(osip_call_info_t** header);

        [DllImport("osipparser2")]
        public static extern void osip_call_info_free(osip_call_info_t* header);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_call_info_parse(osip_call_info_t* header, IntPtr hvalue);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_call_info_to_str(osip_call_info_t* header, IntPtr* dest);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_call_info_clone(osip_call_info_t* header, osip_call_info_t** dest);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_call_info_t
    {
        public static readonly osip_call_info_t* Null = (osip_call_info_t*) IntPtr.Zero;

        public IntPtr element;
        public osip_list_t gen_params;
    }
}