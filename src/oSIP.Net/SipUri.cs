using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class SipUri
    {
        public SipUri()
        {
            Scheme = "sip";
            Parameters = new List<SipUriParameter>();
            Headers = new List<SipUriParameter>();
        }

        public string Scheme { get; set; }

        public string Host { get; set; }

        public string Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public List<SipUriParameter> Parameters { get; }

        public List<SipUriParameter> Headers { get; }

        internal static SipUri FromNative(osip_uri_t* native)
        {
            var uri = new SipUri
            {
                Scheme = Marshal.PtrToStringAnsi(native->scheme),
                Host = Marshal.PtrToStringAnsi(native->host),
                Port = Marshal.PtrToStringAnsi(native->port),
                Username = Marshal.PtrToStringAnsi(native->username),
                Password = Marshal.PtrToStringAnsi(native->password)
            };

            int size = NativeMethods.osip_list_size(&native->url_params);
            for (int i = 0; i < size; i++)
            {
                osip_uri_param_t* param = (osip_uri_param_t*) NativeMethods.osip_list_get(&native->url_params, i);
                uri.Parameters.Add(SipUriParameter.FromNative(param));
            }

            size = NativeMethods.osip_list_size(&native->url_headers);
            for (int i = 0; i < size; i++)
            {
                osip_uri_param_t* header = (osip_uri_param_t*) NativeMethods.osip_list_get(&native->url_headers, i);
                uri.Headers.Add(SipUriParameter.FromNative(header));
            }

            return uri;
        }

        internal osip_uri_t* ToNative()
        {
            osip_uri_t* native;
            NativeMethods.osip_uri_init(&native).ThrowOnError();

            native->scheme = Marshal.StringToHGlobalAnsi(Scheme);
            native->host = Marshal.StringToHGlobalAnsi(Host);
            native->port = Marshal.StringToHGlobalAnsi(Port);
            native->username = Marshal.StringToHGlobalAnsi(Username);
            native->password = Marshal.StringToHGlobalAnsi(Password);

            for (int i = 0; i < Parameters.Count; i++)
            {
                osip_uri_param_t* param = Parameters[i].ToNative();
                NativeMethods.osip_list_add(&native->url_params, param, i).ThrowOnError();
            }

            for (int i = 0; i < Headers.Count; i++)
            {
                osip_uri_param_t* param = Headers[i].ToNative();
                NativeMethods.osip_list_add(&native->url_headers, param, i).ThrowOnError();
            }

            return native;
        }

        public static SipUri Parse(string str)
        {
            TryParseCore(str, out SipUri uri).ThrowOnError();
            return uri;
        }

        public static bool TryParse(string str, out SipUri uri)
        {
            return TryParseCore(str, out uri).EnsureSuccess();
        }

        private static ErrorCode TryParseCore(string str, out SipUri uri)
        {
            var strPtr = Marshal.StringToHGlobalAnsi(str);
            osip_uri_t* native = null;

            try
            {
                ErrorCode errorCode = NativeMethods.osip_uri_init(&native);
                if (!errorCode.EnsureSuccess())
                {
                    uri = default;
                    return errorCode;
                }

                errorCode = NativeMethods.osip_uri_parse(native, strPtr);
                if (!errorCode.EnsureSuccess())
                {
                    uri = default;
                    return errorCode;
                }

                uri = FromNative(native);
                return errorCode;
            }
            finally
            {
                NativeMethods.osip_uri_free(native);
                Marshal.FreeHGlobal(strPtr);
            }
        }

        public SipUri DeepClone()
        {
            osip_uri_t* native = ToNative();

            try
            {
                return FromNative(native);
            }
            finally
            {
                NativeMethods.osip_uri_free(native);
            }
        }

        public override string ToString()
        {
            osip_uri_t* native = ToNative();
            IntPtr ptr = IntPtr.Zero;

            try
            {
                NativeMethods.osip_uri_to_str(native, &ptr).ThrowOnError();
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                NativeMethods.osip_uri_free(native);
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}