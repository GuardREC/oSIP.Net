using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal class NativeMethods
    {
        [DllImport("osipparser2.dll")]
        public static extern int parser_init();
    }
}