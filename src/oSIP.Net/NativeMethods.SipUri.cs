using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_uri_param_init(osip_uri_param_t** url_param);

        [DllImport("osipparser2.dll")]
        public static extern void osip_uri_param_free (osip_uri_param_t* url_param);

        //int osip_uri_param_clone (const osip_uri_param_t * url_param, osip_uri_param_t ** dest);

        //void osip_uri_param_freelist (osip_list_t * url_params);

        //int osip_uri_param_get_byname (osip_list_t * url_params, char *name, osip_uri_param_t ** dest);

        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_uri_init(osip_uri_t** url);

        [DllImport("osipparser2.dll")]
        public static extern void osip_uri_free(osip_uri_t* url);

        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_uri_parse(osip_uri_t* url, IntPtr buf);

        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_uri_to_str(osip_uri_t* url, IntPtr* dest);

        [DllImport("osipparser2.dll")]
        public static extern ErrorCode osip_uri_clone(osip_uri_t* url, osip_uri_t** dest);

        //int osip_uri_to_str_canonical (const osip_uri_t * url, char **dest);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_uri_t
    {
        public static readonly osip_uri_t* Null = (osip_uri_t*) IntPtr.Zero;

        public IntPtr scheme;
        public IntPtr username;
        public IntPtr password;
        public IntPtr host;
        public IntPtr port;
        public osip_list_t url_params;
        public osip_list_t url_headers;
        public IntPtr @string;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_uri_param_t
    {
        public static readonly osip_uri_param_t* Null = (osip_uri_param_t*) IntPtr.Zero;

        public IntPtr gname;
        public IntPtr gvalue;
    }
}