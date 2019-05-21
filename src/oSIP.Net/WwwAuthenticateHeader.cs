using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class WwwAuthenticateHeader
    {
        public string AuthenticationType { get; set; }

        public string Realm { get; set; }

        public string Domain { get; set; }

        public string Nonce { get; set; }

        public string Opaque { get; set; }

        public string Stale { get; set; }

        public string Algorithm { get; set; }

        public string QopOptions { get; set; }

        public string Version { get; set; }

        public string TargetName { get; set; }

        public string GssApiData { get; set; }

        internal static WwwAuthenticateHeader FromNative(osip_www_authenticate_t* native)
        {
            var header = new WwwAuthenticateHeader
            {
                AuthenticationType = Marshal.PtrToStringAnsi(native->auth_type),
                Realm = Marshal.PtrToStringAnsi(native->realm),
                Domain = Marshal.PtrToStringAnsi(native->domain),
                Nonce = Marshal.PtrToStringAnsi(native->nonce),
                Opaque = Marshal.PtrToStringAnsi(native->opaque),
                Stale = Marshal.PtrToStringAnsi(native->stale),
                Algorithm = Marshal.PtrToStringAnsi(native->algorithm),
                QopOptions = Marshal.PtrToStringAnsi(native->qop_options),
                Version = Marshal.PtrToStringAnsi(native->version),
                TargetName = Marshal.PtrToStringAnsi(native->targetname),
                GssApiData = Marshal.PtrToStringAnsi(native->gssapi_data)
            };

            return header;
        }

        internal osip_www_authenticate_t* ToNative()
        {
            osip_www_authenticate_t* native;
            NativeMethods.osip_www_authenticate_init(&native).ThrowOnError();

            native->auth_type = Marshal.StringToHGlobalAnsi(AuthenticationType);
            native->realm = Marshal.StringToHGlobalAnsi(Realm);
            native->domain = Marshal.StringToHGlobalAnsi(Domain);
            native->nonce = Marshal.StringToHGlobalAnsi(Nonce);
            native->opaque = Marshal.StringToHGlobalAnsi(Opaque);
            native->stale = Marshal.StringToHGlobalAnsi(Stale);
            native->algorithm = Marshal.StringToHGlobalAnsi(Algorithm);
            native->qop_options = Marshal.StringToHGlobalAnsi(QopOptions);
            native->version = Marshal.StringToHGlobalAnsi(Version);
            native->targetname = Marshal.StringToHGlobalAnsi(TargetName);
            native->gssapi_data = Marshal.StringToHGlobalAnsi(GssApiData);

            return native;
        }

        public static WwwAuthenticateHeader Parse(string str)
        {
            TryParseCore(str, out WwwAuthenticateHeader header).ThrowOnError();
            return header;
        }

        public static bool TryParse(string str, out WwwAuthenticateHeader header)
        {
            return TryParseCore(str, out header).EnsureSuccess();
        }

        private static ErrorCode TryParseCore(string str, out WwwAuthenticateHeader header)
        {
            osip_www_authenticate_t* native = null;
            var strPtr = Marshal.StringToHGlobalAnsi(str);

            try
            {
                ErrorCode errorCode = NativeMethods.osip_www_authenticate_init(&native);
                if (!errorCode.EnsureSuccess())
                {
                    header = null;
                    return errorCode;
                }

                errorCode = NativeMethods.osip_www_authenticate_parse(native, strPtr);
                if (!errorCode.EnsureSuccess())
                {
                    header = null;
                    return errorCode;
                }

                header = FromNative(native);
                return errorCode;
            }
            finally
            {
                NativeMethods.osip_www_authenticate_free(native);
                Marshal.FreeHGlobal(strPtr);
            }
        }

        public WwwAuthenticateHeader DeepClone()
        {
            osip_www_authenticate_t* native = ToNative();

            try
            {
                return FromNative(native);
            }
            finally
            {
                NativeMethods.osip_www_authenticate_free(native);
            }
        }

        public override string ToString()
        {
            IntPtr ptr = IntPtr.Zero;
            osip_www_authenticate_t* native = ToNative();

            try
            {
                NativeMethods.osip_www_authenticate_to_str(native, &ptr).ThrowOnError();
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                NativeMethods.osip_www_authenticate_free(native);
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}