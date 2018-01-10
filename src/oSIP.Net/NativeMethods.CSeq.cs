using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_cseq_init(osip_cseq_t** header);

        [DllImport("osipparser2.dll")]
        public static extern void osip_cseq_free(osip_cseq_t* header);

        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_cseq_parse(osip_cseq_t* header, IntPtr hvalue);

        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_cseq_to_str(osip_cseq_t* header, IntPtr* dest);

        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_cseq_clone(osip_cseq_t* header, osip_cseq_t** dest);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_cseq_t
    {
        public static readonly osip_cseq_t* Null = (osip_cseq_t*) IntPtr.Zero;

        public IntPtr method;
        public IntPtr number;
    };
}