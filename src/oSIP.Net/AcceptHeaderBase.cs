using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public abstract unsafe class AcceptHeaderBase : OwnershipDisposable
    {
        private osip_accept_encoding_t* _native;

        protected AcceptHeaderBase() : this(Create(), true)
        {
        }

        internal AcceptHeaderBase(osip_accept_encoding_t* native, bool isOwner) : base(isOwner)
        {
            _native = native;
            Parameters = new LinkedList<GenericParameter>(
                &_native->gen_params,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new GenericParameter((osip_uri_param_t*) ptr, false));
        }

        private static osip_accept_encoding_t* Create()
        {
            osip_accept_encoding_t* encoding;
            NativeMethods.osip_accept_encoding_init(&encoding).ThrowOnError();
            return encoding;
        }

        public string Element
        {
            get => Marshal.PtrToStringAnsi(_native->element);
            set
            {
                Marshal.FreeHGlobal(_native->element);
                _native->element = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public LinkedList<GenericParameter> Parameters { get; }

        protected static T Parse<T>(string str) where T : AcceptHeaderBase, new()
        {
            var header = new T();

            var strPtr = Marshal.StringToHGlobalAnsi(str);
            NativeMethods.osip_accept_encoding_parse(header._native, strPtr).ThrowOnError();
            Marshal.FreeHGlobal(strPtr);

            return header;
        }

        internal osip_accept_encoding_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        protected T DeepClone<T>(Func<IntPtr, T> func) where T : AcceptHeaderBase
        {
            osip_accept_encoding_t* encoding;
            NativeMethods.osip_accept_encoding_clone(_native, &encoding).ThrowOnError();
            return func(new IntPtr(encoding));
        }

        public override string ToString()
        {
            IntPtr ptr;
            NativeMethods.osip_accept_encoding_to_str(_native, &ptr).ThrowOnError();

            string str = Marshal.PtrToStringAnsi(ptr);
            Marshal.FreeHGlobal(ptr);

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_accept_encoding_free(_native);
            _native = osip_accept_encoding_t.Null;
        }
    }
}