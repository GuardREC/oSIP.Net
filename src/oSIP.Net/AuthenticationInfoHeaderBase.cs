using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public abstract unsafe class AuthenticationInfoHeaderBase : OwnershipDisposable
    {
        private osip_authentication_info_t* _native;

        protected AuthenticationInfoHeaderBase() : this(Create(), true)
        {
        }

        internal AuthenticationInfoHeaderBase(osip_authentication_info_t* native, bool isOwner) : base(isOwner)
        {
            _native = native;
        }

        private static osip_authentication_info_t* Create()
        {
            osip_authentication_info_t* info;
            NativeMethods.osip_authentication_info_init(&info).ThrowOnError();
            return info;
        }

        public string AuthenticationType
        {
            get => Marshal.PtrToStringAnsi(_native->auth_type);
            set
            {
                Marshal.FreeHGlobal(_native->auth_type);
                _native->auth_type = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string NextNonce
        {
            get => Marshal.PtrToStringAnsi(_native->nextnonce);
            set
            {
                Marshal.FreeHGlobal(_native->nextnonce);
                _native->nextnonce = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string QopOptions
        {
            get => Marshal.PtrToStringAnsi(_native->qop_options);
            set
            {
                Marshal.FreeHGlobal(_native->qop_options);
                _native->qop_options = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string RspAuth
        {
            get => Marshal.PtrToStringAnsi(_native->rspauth);
            set
            {
                Marshal.FreeHGlobal(_native->rspauth);
                _native->rspauth = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string CNonce
        {
            get => Marshal.PtrToStringAnsi(_native->cnonce);
            set
            {
                Marshal.FreeHGlobal(_native->cnonce);
                _native->cnonce = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string NonceCount
        {
            get => Marshal.PtrToStringAnsi(_native->nonce_count);
            set
            {
                Marshal.FreeHGlobal(_native->nonce_count);
                _native->nonce_count = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Snum
        {
            get => Marshal.PtrToStringAnsi(_native->snum);
            set
            {
                Marshal.FreeHGlobal(_native->snum);
                _native->snum = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Srand
        {
            get => Marshal.PtrToStringAnsi(_native->srand);
            set
            {
                Marshal.FreeHGlobal(_native->srand);
                _native->srand = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Realm
        {
            get => Marshal.PtrToStringAnsi(_native->realm);
            set
            {
                Marshal.FreeHGlobal(_native->realm);
                _native->realm = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string TargetName
        {
            get => Marshal.PtrToStringAnsi(_native->targetname);
            set
            {
                Marshal.FreeHGlobal(_native->targetname);
                _native->targetname = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Opaque
        {
            get => Marshal.PtrToStringAnsi(_native->opaque);
            set
            {
                Marshal.FreeHGlobal(_native->opaque);
                _native->opaque = Marshal.StringToHGlobalAnsi(value);
            }
        }

        protected static T Parse<T>(string str) where T : AuthenticationInfoHeaderBase, new()
        {
            TryParseCore(str, out T header).ThrowOnError(header);
            return header;
        }

        protected static bool TryParse<T>(string str, out T header) where T : AuthenticationInfoHeaderBase, new()
        {
            return TryParseCore(str, out header).EnsureSuccess(ref header);
        }

        private static ErrorCode TryParseCore<T>(string str, out T header) where T : AuthenticationInfoHeaderBase, new()
        {
            var strPtr = Marshal.StringToHGlobalAnsi(str);
            try
            {
                header = new T();
                return NativeMethods.osip_authentication_info_parse(header._native, strPtr);
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
            }
        }

        internal osip_authentication_info_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        protected T DeepClone<T>(Func<IntPtr, T> func) where T : AuthenticationInfoHeaderBase
        {
            osip_authentication_info_t* native;
            NativeMethods.osip_authentication_info_clone(_native, &native).ThrowOnError();
            return func(new IntPtr(native));
        }

        public override string ToString()
        {
            IntPtr ptr;
            NativeMethods.osip_authentication_info_to_str(_native, &ptr).ThrowOnError();

            string str = Marshal.PtrToStringAnsi(ptr);
            Marshal.FreeHGlobal(ptr);

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_authentication_info_free(_native);
            _native = osip_authentication_info_t.Null;
        }
    }
}