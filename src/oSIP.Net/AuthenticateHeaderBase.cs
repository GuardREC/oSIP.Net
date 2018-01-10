using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public abstract unsafe class AuthenticateHeaderBase : OwnershipDisposable
    {
        private osip_www_authenticate_t* _native;

        protected AuthenticateHeaderBase() : this(Create(), true)
        {
        }

        internal AuthenticateHeaderBase(osip_www_authenticate_t* native, bool isOwner) : base(isOwner)
        {
            _native = native;
        }

        private static osip_www_authenticate_t* Create()
        {
            osip_www_authenticate_t* native;
            NativeMethods.osip_www_authenticate_init(&native).ThrowOnError();
            return native;
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

        public string Realm
        {
            get => Marshal.PtrToStringAnsi(_native->realm);
            set
            {
                Marshal.FreeHGlobal(_native->realm);
                _native->realm = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Domain
        {
            get => Marshal.PtrToStringAnsi(_native->domain);
            set
            {
                Marshal.FreeHGlobal(_native->domain);
                _native->domain = Marshal.StringToHGlobalAnsi(value);
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

        public string Opaque
        {
            get => Marshal.PtrToStringAnsi(_native->opaque);
            set
            {
                Marshal.FreeHGlobal(_native->opaque);
                _native->opaque = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Stale
        {
            get => Marshal.PtrToStringAnsi(_native->stale);
            set
            {
                Marshal.FreeHGlobal(_native->stale);
                _native->stale = Marshal.StringToHGlobalAnsi(value);
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

        public string QopOptions
        {
            get => Marshal.PtrToStringAnsi(_native->qop_options);
            set
            {
                Marshal.FreeHGlobal(_native->qop_options);
                _native->qop_options = Marshal.StringToHGlobalAnsi(value);
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

        protected static T Parse<T>(string str) where T : AuthenticateHeaderBase, new()
        {
            var from = new T();

            var strPtr = Marshal.StringToHGlobalAnsi(str);
            NativeMethods.osip_www_authenticate_parse(from._native, strPtr).ThrowOnError();
            Marshal.FreeHGlobal(strPtr);

            return from;
        }

        internal osip_www_authenticate_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        protected T DeepClone<T>(Func<IntPtr, T> func) where T : AuthenticateHeaderBase
        {
            osip_www_authenticate_t* native;
            NativeMethods.osip_www_authenticate_clone(_native, &native).ThrowOnError();
            return func(new IntPtr(native));
        }

        public override string ToString()
        {
            IntPtr ptr;
            NativeMethods.osip_www_authenticate_to_str(_native, &ptr).ThrowOnError();

            string str = Marshal.PtrToStringAnsi(ptr);
            NativeMethods.osip_free(ptr.ToPointer());

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_www_authenticate_free(_native);
            _native = osip_www_authenticate_t.Null;
        }
    }
}