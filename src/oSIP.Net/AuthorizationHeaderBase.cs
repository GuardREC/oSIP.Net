using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public abstract unsafe class AuthorizationHeaderBase : OwnershipDisposable
    {
        private osip_authorization_t* _native;

        protected AuthorizationHeaderBase() : this(Create(), true)
        {
        }

        internal AuthorizationHeaderBase(osip_authorization_t* from, bool isOwner) : base(isOwner)
        {
            _native = from;
        }

        private static osip_authorization_t* Create()
        {
            osip_authorization_t* from;
            NativeMethods.osip_authorization_init(&from).ThrowOnError();
            return from;
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

        public string Username
        {
            get => Marshal.PtrToStringAnsi(_native->username);
            set
            {
                Marshal.FreeHGlobal(_native->username);
                _native->username = Marshal.StringToHGlobalAnsi(value);
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

        public string Nonce
        {
            get => Marshal.PtrToStringAnsi(_native->nonce);
            set
            {
                Marshal.FreeHGlobal(_native->nonce);
                _native->nonce = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Uri
        {
            get => Marshal.PtrToStringAnsi(_native->uri);
            set
            {
                Marshal.FreeHGlobal(_native->uri);
                _native->uri = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Response
        {
            get => Marshal.PtrToStringAnsi(_native->response);
            set
            {
                Marshal.FreeHGlobal(_native->response);
                _native->response = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Digest
        {
            get => Marshal.PtrToStringAnsi(_native->digest);
            set
            {
                Marshal.FreeHGlobal(_native->digest);
                _native->digest = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Algorithm
        {
            get => Marshal.PtrToStringAnsi(_native->algorithm);
            set
            {
                Marshal.FreeHGlobal(_native->algorithm);
                _native->algorithm = Marshal.StringToHGlobalAnsi(value);
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

        public string Opaque
        {
            get => Marshal.PtrToStringAnsi(_native->opaque);
            set
            {
                Marshal.FreeHGlobal(_native->opaque);
                _native->opaque = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string MessageQop
        {
            get => Marshal.PtrToStringAnsi(_native->message_qop);
            set
            {
                Marshal.FreeHGlobal(_native->message_qop);
                _native->message_qop = Marshal.StringToHGlobalAnsi(value);
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

        public string Version
        {
            get => Marshal.PtrToStringAnsi(_native->version);
            set
            {
                Marshal.FreeHGlobal(_native->version);
                _native->version = Marshal.StringToHGlobalAnsi(value);
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

        public string GssApiData
        {
            get => Marshal.PtrToStringAnsi(_native->gssapi_data);
            set
            {
                Marshal.FreeHGlobal(_native->gssapi_data);
                _native->gssapi_data = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string CRand
        {
            get => Marshal.PtrToStringAnsi(_native->crand);
            set
            {
                Marshal.FreeHGlobal(_native->crand);
                _native->crand = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string CNum
        {
            get => Marshal.PtrToStringAnsi(_native->cnum);
            set
            {
                Marshal.FreeHGlobal(_native->cnum);
                _native->cnum = Marshal.StringToHGlobalAnsi(value);
            }
        }

        protected static T Parse<T>(string str) where T : AuthorizationHeaderBase, new()
        {
            var from = new T();

            var strPtr = Marshal.StringToHGlobalAnsi(str);
            NativeMethods.osip_authorization_parse(from._native, strPtr).ThrowOnError();
            Marshal.FreeHGlobal(strPtr);

            return from;
        }

        internal osip_authorization_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        protected T DeepClone<T>(Func<IntPtr, T> func) where T : AuthorizationHeaderBase
        {
            osip_authorization_t* native;
            NativeMethods.osip_authorization_clone(_native, &native).ThrowOnError();
            return func(new IntPtr(native));
        }

        public override string ToString()
        {
            IntPtr ptr;
            NativeMethods.osip_authorization_to_str(_native, &ptr).ThrowOnError();

            string str = Marshal.PtrToStringAnsi(ptr);
            Marshal.FreeHGlobal(ptr);

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_authorization_free(_native);
            _native = osip_authorization_t.Null;
        }
    }
}