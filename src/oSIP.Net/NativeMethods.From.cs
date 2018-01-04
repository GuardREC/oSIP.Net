using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2.dll")]
        public static extern int osip_from_init (osip_from_t** header);

        [DllImport("osipparser2.dll")]
        public static extern void osip_from_free (osip_from_t * header);

        [DllImport("osipparser2.dll")]
        public static extern int osip_from_parse (osip_from_t * header, IntPtr hvalue);

        [DllImport("osipparser2.dll")]
        public static extern int osip_from_to_str(osip_from_t* header, IntPtr* dest);

        [DllImport("osipparser2.dll")]
        public static extern int osip_from_clone(osip_from_t* header, osip_from_t** dest);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_from_t
    {
        public static readonly osip_from_t* Null = (osip_from_t*) IntPtr.Zero;

        public IntPtr displayname;
        public osip_uri_t* url;
        public osip_list_t gen_params;
    };
}