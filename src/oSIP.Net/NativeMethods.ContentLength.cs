using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2.dll")]
        public static extern int osip_content_length_init(osip_content_length_t** header);

        [DllImport("osipparser2.dll")]
        public static extern void osip_content_length_free(osip_content_length_t* header);

        [DllImport("osipparser2.dll")]
        public static extern int osip_content_length_parse(osip_content_length_t* header, IntPtr hvalue);

        [DllImport("osipparser2.dll")]
        public static extern int osip_content_length_to_str(osip_content_length_t* header, IntPtr* dest);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_content_length_t
    {
        public static readonly osip_content_length_t* Null = (osip_content_length_t*) IntPtr.Zero;

        public IntPtr value;
    };
}