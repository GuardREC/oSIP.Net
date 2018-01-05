using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2.dll")]
        public static extern int osip_accept_encoding_init(osip_accept_encoding_t** header);

        [DllImport("osipparser2.dll")]
        public static extern int osip_accept_encoding_parse(osip_accept_encoding_t* header, IntPtr hvalue);

        [DllImport("osipparser2.dll")]
        public static extern int osip_accept_encoding_to_str(osip_accept_encoding_t* header, IntPtr* dest);

        [DllImport("osipparser2.dll")]
        public static extern void osip_accept_encoding_free(osip_accept_encoding_t* header);

        [DllImport("osipparser2.dll")]
        public static extern int osip_accept_encoding_clone(osip_accept_encoding_t* header, osip_accept_encoding_t** dest);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_accept_encoding_t
    {
        public static readonly osip_accept_encoding_t* Null = (osip_accept_encoding_t*) IntPtr.Zero;

        public IntPtr element;
        public osip_list_t gen_params;
    };
}