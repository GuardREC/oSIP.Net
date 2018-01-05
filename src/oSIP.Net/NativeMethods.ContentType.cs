using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2.dll")]
        public static extern int osip_content_type_init(osip_content_type_t** header);

        [DllImport("osipparser2.dll")]
        public static extern void osip_content_type_free(osip_content_type_t* header);

        [DllImport("osipparser2.dll")]
        public static extern int osip_content_type_parse(osip_content_type_t* header, IntPtr hvalue);

        [DllImport("osipparser2.dll")]
        public static extern int osip_content_type_to_str(osip_content_type_t* header, IntPtr* dest);

        [DllImport("osipparser2.dll")]
        public static extern int osip_content_type_clone(osip_content_type_t* header, osip_content_type_t** dest);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_content_type_t
    {
        public static readonly osip_content_type_t* Null = (osip_content_type_t*) IntPtr.Zero;

        public IntPtr type;
        public IntPtr subtype;
        public osip_list_t gen_params;
    };
}