using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public abstract unsafe class NameAddressHeaderBase : OwnershipDisposable
    {
        private osip_from_t* _native;

        protected NameAddressHeaderBase() : this(Create(), true)
        {
        }

        internal NameAddressHeaderBase(osip_from_t* from, bool isOwner) : base(isOwner)
        {
            _native = from;
            Parameters = new LinkedList<GenericParameter>(
                &_native->gen_params,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new GenericParameter((osip_uri_param_t*) ptr, false));
        }

        private static osip_from_t* Create()
        {
            osip_from_t* from;
            NativeMethods.osip_from_init(&from);
            return from;
        }

        public string DisplayName
        {
            get => Marshal.PtrToStringAnsi(_native->displayname);
            set
            {
                Marshal.FreeHGlobal(_native->displayname);
                _native->displayname = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public SipUri Url
        {
            get => _native->url != osip_uri_t.Null
                ? new SipUri(_native->url, false)
                : null;
            set
            {
                if (_native->url != osip_uri_t.Null)
                {
                    NativeMethods.osip_uri_free(_native->url);
                }
                _native->url = value != null
                    ? value.TakeOwnership()
                    : osip_uri_t.Null;
            }
        }

        public LinkedList<GenericParameter> Parameters { get; }

        protected static T Parse<T>(string str) where T : NameAddressHeaderBase, new()
        {
            var from = new T();

            var strPtr = Marshal.StringToHGlobalAnsi(str);
            NativeMethods.osip_from_parse(from._native, strPtr);
            Marshal.FreeHGlobal(strPtr);

            return from;
        }

        internal osip_from_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        protected T DeepClone<T>(Func<IntPtr, T> foo) where T : NameAddressHeaderBase
        {
            osip_from_t* native;
            NativeMethods.osip_from_clone(_native, &native);
            return foo(new IntPtr(native));
        }

        public override string ToString()
        {
            IntPtr ptr;
            NativeMethods.osip_from_to_str(_native, &ptr);

            string str = Marshal.PtrToStringAnsi(ptr);
            NativeMethods.osip_free(ptr.ToPointer());

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_from_free(_native);
            _native = osip_from_t.Null;
        }
    }
}