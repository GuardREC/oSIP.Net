using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal static unsafe partial class NativeMethods
    {
        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_list_init(osip_list_t* li);

        [DllImport("osipparser2.dll")]
        public static extern int osip_list_size(osip_list_t* li);

        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_list_add(osip_list_t* li, void* element, int pos);

        [DllImport("osipparser2.dll")]
        public static extern void* osip_list_get(osip_list_t* li, int pos);

        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_list_remove(osip_list_t* li, int pos);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_list_t
    {
        private readonly int nb_elt;
        private readonly __node_t* node;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct __node_t
    {
        private readonly __node_t* next;
        private readonly IntPtr element;
    };
}