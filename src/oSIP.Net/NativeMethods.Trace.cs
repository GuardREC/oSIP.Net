using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    internal partial class NativeMethods
    {
        [DllImport("osipparser2")]
        public static extern void osip_trace_initialize_func(TraceLevel level, IntPtr func);

        [DllImport("osipparser2")]
        public static extern void osip_trace_enable_level(TraceLevel level);

        [DllImport("osipparser2")]
        public static extern void osip_trace_disable_level(TraceLevel level);

        [DllImport("msvcrt.dll", EntryPoint = "vsprintf")]
        public static extern int vsprintf_win(IntPtr buffer, IntPtr format, IntPtr argptr);

        [DllImport("libc.so.6", EntryPoint = "vsprintf")]
        public static extern int vsprintf_linux(IntPtr buffer, IntPtr format, IntPtr argptr);

        [DllImport("libSystem", EntryPoint = "vsprintf")]
        public static extern int vsprintf_mac(IntPtr buffer, IntPtr format, IntPtr argptr);
    }
}