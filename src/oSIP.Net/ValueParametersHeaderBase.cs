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
            NativeMethods.osip_call_info_init(&native);
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
            var from = new T();

            var strPtr = Marshal.StringToHGlobalAnsi(str);
            NativeMethods.osip_call_info_parse(from._native, strPtr);
            Marshal.FreeHGlobal(strPtr);

            return from;
        }

        internal osip_call_info_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        protected T DeepClone<T>(Func<IntPtr, T> foo) where T : ValueParametersHeaderBase
        {
            osip_call_info_t* native;
            NativeMethods.osip_call_info_clone(_native, &native);
            return foo(new IntPtr(native));
        }

        public override string ToString()
        {
            IntPtr ptr;
            NativeMethods.osip_call_info_to_str(_native, &ptr);

            string str = Marshal.PtrToStringAnsi(ptr);
            NativeMethods.osip_free(ptr.ToPointer());

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_call_info_free(_native);
            _native = osip_call_info_t.Null;
        }
    }
}