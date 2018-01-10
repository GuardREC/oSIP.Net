using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    internal partial class NativeMethods
    {
        [DllImport("osipparser2.dll")]
        public static extern void osip_trace_initialize_func(TraceLevel level, IntPtr func);

        [DllImport("osipparser2.dll")]
        public static extern void osip_trace_enable_level(TraceLevel level);

        [DllImport("osipparser2.dll")]
        public static extern void osip_trace_disable_level(TraceLevel level);

        [DllImport("msvcrt.dll")]
        public static extern int vsprintf(IntPtr buffer, IntPtr format, IntPtr argptr);
    }
}