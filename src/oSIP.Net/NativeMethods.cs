using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal partial class NativeMethods
    {
        [DllImport("osipparser2")]
        public static extern ErrorCode parser_init();
     }
 }