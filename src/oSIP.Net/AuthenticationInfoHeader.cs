using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class AuthenticationInfoHeader
    {
        public string AuthenticationType { get; set; }

        public string NextNonce { get; set; }

        public string QopOptions { get; set; }

        public string RspAuth { get; set; }

        public string CNonce { get; set; }

        public string NonceCount { get; set; }

        public string Snum { get; set; }

        public string Srand { get; set; }

        public string Realm { get; set; }

        public string TargetName { get; set; }

        public string Opaque { get; set; }

        internal static AuthenticationInfoHeader FromNative(osip_authentication_info_t* native)
        {
            var header = new AuthenticationInfoHeader
            {
                AuthenticationType = Marshal.PtrToStringAnsi(native->auth_type),
                NextNonce = Marshal.PtrToStringAnsi(native->nextnonce),
                QopOptions = Marshal.PtrToStringAnsi(native->qop_options),
                RspAuth = Marshal.PtrToStringAnsi(native->rspauth),
                CNonce = Marshal.PtrToStringAnsi(native->cnonce),
                NonceCount = Marshal.PtrToStringAnsi(native->nonce_count),
                Snum = Marshal.PtrToStringAnsi(native->snum),
                Srand = Marshal.PtrToStringAnsi(native->srand),
                Realm = Marshal.PtrToStringAnsi(native->realm),
                TargetName = Marshal.PtrToStringAnsi(native->targetname),
                Opaque = Marshal.PtrToStringAnsi(native->opaque),
            };

            return header;
        }

        internal osip_authentication_info_t* ToNative()
        {
            osip_authentication_info_t* native;
            NativeMethods.osip_authentication_info_init(&native).ThrowOnError();

            native->auth_type = Marshal.StringToHGlobalAnsi(AuthenticationType);
            native->nextnonce = Marshal.StringToHGlobalAnsi(NextNonce);
            native->qop_options = Marshal.StringToHGlobalAnsi(QopOptions);
            native->rspauth = Marshal.StringToHGlobalAnsi(RspAuth);
            native->cnonce = Marshal.StringToHGlobalAnsi(CNonce);
            native->nonce_count = Marshal.StringToHGlobalAnsi(NonceCount);
            native->snum = Marshal.StringToHGlobalAnsi(Snum);
            native->srand = Marshal.StringToHGlobalAnsi(Srand);
            native->realm = Marshal.StringToHGlobalAnsi(Realm);
            native->targetname = Marshal.StringToHGlobalAnsi(TargetName);
            native->opaque = Marshal.StringToHGlobalAnsi(Opaque);

            return native;
        }

        public static AuthenticationInfoHeader Parse(string str)
        {
            TryParseCore(str, out AuthenticationInfoHeader header).ThrowOnError();
            return header;
        }

        public static bool TryParse(string str, out AuthenticationInfoHeader header)
        {
            return TryParseCore(str, out header).EnsureSuccess();
        }

        private static ErrorCode TryParseCore(string str, out AuthenticationInfoHeader header)
        {
            osip_authentication_info_t* native = null;
            var strPtr = Marshal.StringToHGlobalAnsi(str);

            try
            {
                ErrorCode errorCode = NativeMethods.osip_authentication_info_init(&native);
                if (!errorCode.EnsureSuccess())
                {
                    header = null;
                    return errorCode;
                }

                errorCode = NativeMethods.osip_authentication_info_parse(native, strPtr);
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
                NativeMethods.osip_authentication_info_free(native);
                Marshal.FreeHGlobal(strPtr);
            }
        }

        public AuthenticationInfoHeader DeepClone()
        {
            osip_authentication_info_t* native = ToNative();

            try
            {
                return FromNative(native);
            }
            finally
            {
                NativeMethods.osip_authentication_info_free(native);
            }
        }

        public override string ToString()
        {
            IntPtr ptr = IntPtr.Zero;
            osip_authentication_info_t* native = ToNative();

            try
            {
                NativeMethods.osip_authentication_info_to_str(native, &ptr).ThrowOnError();
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                NativeMethods.osip_authentication_info_free(native);
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}