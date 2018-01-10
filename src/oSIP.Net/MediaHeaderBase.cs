using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public abstract unsafe class MediaHeaderBase : OwnershipDisposable
    {
        private osip_content_type_t* _native;

        protected MediaHeaderBase() : this(Create(), true)
        {
        }

        internal MediaHeaderBase(osip_content_type_t* native, bool isOwner) : base(isOwner)
        {
            _native = native;
            Parameters = new LinkedList<GenericParameter>(
                &_native->gen_params,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new GenericParameter((osip_uri_param_t*) ptr, false));
        }

        private static osip_content_type_t* Create()
        {
            osip_content_type_t* contentType;
            NativeMethods.osip_content_type_init(&contentType).ThrowOnError();
            return contentType;
        }

        public string Type
        {
            get => Marshal.PtrToStringAnsi(_native->type);
            set
            {
                Marshal.FreeHGlobal(_native->type);
                _native->type = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string SubType
        {
            get => Marshal.PtrToStringAnsi(_native->subtype);
            set
            {
                Marshal.FreeHGlobal(_native->subtype);
                _native->subtype = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public LinkedList<GenericParameter> Parameters { get; }

        protected static T Parse<T>(string str) where T : MediaHeaderBase, new()
        {
            var header = new T();

            var strPtr = Marshal.StringToHGlobalAnsi(str);
            NativeMethods.osip_content_type_parse(header._native, strPtr).ThrowOnError();
            Marshal.FreeHGlobal(strPtr);

            return header;
        }

        internal osip_content_type_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        protected T DeepClone<T>(Func<IntPtr, T> func) where T : MediaHeaderBase
        {
            osip_content_type_t* native;
            NativeMethods.osip_content_type_clone(_native, &native).ThrowOnError();
            return func(new IntPtr(native));
        }

        public override string ToString()
        {
            IntPtr ptr;
            NativeMethods.osip_content_type_to_str(_native, &ptr).ThrowOnError();

            string str = Marshal.PtrToStringAnsi(ptr);
            NativeMethods.osip_free(ptr.ToPointer());

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_content_type_free(_native);
            _native = osip_content_type_t.Null;
        }
    }
}