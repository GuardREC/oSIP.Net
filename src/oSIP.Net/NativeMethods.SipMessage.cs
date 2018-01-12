using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace oSIP.Net
{
    internal unsafe partial class NativeMethods
    {
        [DllImport("osipparser2")]
        public static extern ErrorCode osip_message_init(osip_message_t** sip);

        [DllImport("osipparser2")]
        public static extern void osip_message_free(osip_message_t* sip);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_message_parse(osip_message_t* sip, IntPtr buf, ulong length);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_message_to_str(osip_message_t* sip, IntPtr* dest, int* message_length);

        [DllImport("osipparser2")]
        public static extern ErrorCode osip_message_clone(osip_message_t* sip, osip_message_t** dest);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct osip_message_t
    {
        public static readonly osip_message_t* Null = (osip_message_t*) IntPtr.Zero;

        public IntPtr sip_version;
        public osip_uri_t* req_uri;
        public IntPtr sip_method;

        public int status_code;
        public IntPtr reason_phrase;

        public osip_list_t accepts;
        public osip_list_t accept_encodings;
        public osip_list_t accept_languages;
        public osip_list_t alert_infos;
        public osip_list_t allows;
        public osip_list_t authentication_infos;
        public osip_list_t authorizations;
        public osip_call_id_t* call_id;
        public osip_list_t call_infos;
        public osip_list_t contacts;
        public osip_list_t content_encodings;
        public readonly osip_content_length_t* content_length;
        public osip_content_type_t* content_type;
        public osip_cseq_t* cseq;
        public osip_list_t error_infos;
        public osip_from_t* from;
        public osip_content_length_t* mime_version;
        public osip_list_t proxy_authenticates;
        public osip_list_t proxy_authentication_infos;
        public osip_list_t proxy_authorizations;
        public osip_list_t record_routes;
        public osip_list_t routes;
        public osip_from_t* to;
        public osip_list_t vias;
        public osip_list_t www_authenticates;

        public osip_list_t headers;

        public osip_list_t bodies;
    }
}