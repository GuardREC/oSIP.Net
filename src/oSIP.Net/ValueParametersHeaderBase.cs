using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public abstract unsafe class ValueParametersHeaderBase : OwnershipDisposable
    {
        private osip_call_info_t* _native;

        protected ValueParametersHeaderBase() : this(Create(), true)
        {
        }

        internal ValueParametersHeaderBase(osip_call_info_t* native, bool isOwner) : base(isOwner)
        {
            _native = native;
            Parameters = new LinkedList<GenericParameter>(
                &_native->gen_params,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new GenericParameter((osip_uri_param_t*) ptr, false));
        }

        private static osip_call_info_t* Create()
        {
            osip_call_info_t* native;
            NativeMethods.osip_call_info_init(&native).ThrowOnError();
            return native;
        }

        public string Value
        {
            get => Marshal.PtrToStringAnsi(_native->element);
            set
            {
                Marshal.FreeHGlobal(_native->element);
                _native->element = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public LinkedList<GenericParameter> Parameters { get; }

        protected static T Parse<T>(string str) where T : ValueParametersHeaderBase, new()
        {
            TryParseCore(str, out T header).ThrowOnError(header);
            return header;
        }

        protected static bool TryParse<T>(string str, out T header) where T : ValueParametersHeaderBase, new()
        {
            return TryParseCore(str, out header).EnsureSuccess(ref header);
        }

        private static ErrorCode TryParseCore<T>(string str, out T header) where T : ValueParametersHeaderBase, new()
        {
            var strPtr = Marshal.StringToHGlobalAnsi(str);
            try
            {
                header = new T();
                return NativeMethods.osip_call_info_parse(header._native, strPtr);
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
            }
        }

        internal osip_call_info_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        protected T DeepClone<T>(Func<IntPtr, T> func) where T : ValueParametersHeaderBase
        {
            osip_call_info_t* native;
            NativeMethods.osip_call_info_clone(_native, &native).ThrowOnError();
            return func(new IntPtr(native));
        }

        public override string ToString()
        {
            IntPtr ptr;
            NativeMethods.osip_call_info_to_str(_native, &ptr).ThrowOnError();

            string str = Marshal.PtrToStringAnsi(ptr);
            Marshal.FreeHGlobal(ptr);

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_call_info_free(_native);
            _native = osip_call_info_t.Null;
        }
    }
}