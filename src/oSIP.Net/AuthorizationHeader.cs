using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class AuthorizationHeader
    {
        public string AuthenticationType { get; set; }

        public string Username { get; set; }

        public string Realm { get; set; }

        public string Nonce { get; set; }

        public string Uri { get; set; }

        public string Response { get; set; }

        public string Digest { get; set; }

        public string Algorithm { get; set; }

        public string CNonce { get; set; }

        public string Opaque { get; set; }

        public string MessageQop { get; set; }

        public string NonceCount { get; set; }

        public string Version { get; set; }

        public string TargetName { get; set; }

        public string GssApiData { get; set; }

        public string CRand { get; set; }

        public string CNum { get; set; }

        internal static AuthorizationHeader FromNative(osip_authorization_t* native)
        {
            var header = new AuthorizationHeader
            {
                AuthenticationType = Marshal.PtrToStringAnsi(native->auth_type),
                Username = Marshal.PtrToStringAnsi(native->username),
                Realm = Marshal.PtrToStringAnsi(native->realm),
                Nonce = Marshal.PtrToStringAnsi(native->nonce),
                Uri = Marshal.PtrToStringAnsi(native->uri),
                Response = Marshal.PtrToStringAnsi(native->response),
                Digest = Marshal.PtrToStringAnsi(native->digest),
                Algorithm = Marshal.PtrToStringAnsi(native->algorithm),
                CNonce = Marshal.PtrToStringAnsi(native->cnonce),
                Opaque = Marshal.PtrToStringAnsi(native->opaque),
                MessageQop = Marshal.PtrToStringAnsi(native->message_qop),
                NonceCount = Marshal.PtrToStringAnsi(native->nonce_count),
                Version = Marshal.PtrToStringAnsi(native->version),
                TargetName = Marshal.PtrToStringAnsi(native->targetname),
                GssApiData = Marshal.PtrToStringAnsi(native->gssapi_data),
                CRand = Marshal.PtrToStringAnsi(native->crand),
                CNum = Marshal.PtrToStringAnsi(native->cnum),
            };

            return header;
        }

        internal osip_authorization_t* ToNative()
        {
            osip_authorization_t* native;
            NativeMethods.osip_authorization_init(&native).ThrowOnError();

            native->auth_type = Marshal.StringToHGlobalAnsi(AuthenticationType);
            native->username = Marshal.StringToHGlobalAnsi(Username);
            native->realm = Marshal.StringToHGlobalAnsi(Realm);
            native->nonce = Marshal.StringToHGlobalAnsi(Nonce);
            native->uri = Marshal.StringToHGlobalAnsi(Uri);
            native->response = Marshal.StringToHGlobalAnsi(Response);
            native->digest = Marshal.StringToHGlobalAnsi(Digest);
            native->algorithm = Marshal.StringToHGlobalAnsi(Algorithm);
            native->cnonce = Marshal.StringToHGlobalAnsi(CNonce);
            native->opaque = Marshal.StringToHGlobalAnsi(Opaque);
            native->message_qop = Marshal.StringToHGlobalAnsi(MessageQop);
            native->nonce_count = Marshal.StringToHGlobalAnsi(NonceCount);
            native->version = Marshal.StringToHGlobalAnsi(Version);
            native->targetname = Marshal.StringToHGlobalAnsi(TargetName);
            native->gssapi_data = Marshal.StringToHGlobalAnsi(GssApiData);
            native->crand = Marshal.StringToHGlobalAnsi(CRand);
            native->cnum = Marshal.StringToHGlobalAnsi(CNum);

            return native;
        }

        public static AuthorizationHeader Parse(string str)
        {
            TryParseCore(str, out AuthorizationHeader header).ThrowOnError();
            return header;
        }

        public static bool TryParse(string str, out AuthorizationHeader header)
        {
            return TryParseCore(str, out header).EnsureSuccess();
        }

        private static ErrorCode TryParseCore(string str, out AuthorizationHeader header)
        {
            osip_authorization_t* native = null;
            var strPtr = Marshal.StringToHGlobalAnsi(str);

            try
            {
                ErrorCode errorCode = NativeMethods.osip_authorization_init(&native);
                if (!errorCode.EnsureSuccess())
                {
                    header = null;
                    return errorCode;
                }

                errorCode = NativeMethods.osip_authorization_parse(native, strPtr);
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
                NativeMethods.osip_authorization_free(native);
                Marshal.FreeHGlobal(strPtr);
            }
        }

        public AuthorizationHeader DeepClone()
        {
            osip_authorization_t* native = ToNative();

            try
            {
                return FromNative(native);
            }
            finally
            {
                NativeMethods.osip_authorization_free(native);
            }
        }

        public override string ToString()
        {
            IntPtr ptr = IntPtr.Zero;
            osip_authorization_t* native = ToNative();

            try
            {
                NativeMethods.osip_authorization_to_str(native, &ptr).ThrowOnError();
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                NativeMethods.osip_authorization_free(native);
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}