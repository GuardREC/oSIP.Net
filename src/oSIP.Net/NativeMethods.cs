using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2.dll")]
        public static extern ErrorCode parser_init();

        [DllImport("osipparser2.dll")]
        public static extern void osip_free(void* ptr);
     }
 }