using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public abstract unsafe class ValueHeaderBase : OwnershipDisposable
    {
        private osip_content_length_t* _native;

        protected ValueHeaderBase() : this(Create(), true)
        {
        }

        internal ValueHeaderBase(osip_content_length_t* native, bool isOwner) : base(isOwner)
        {
            _native = native;
        }

        private static osip_content_length_t* Create()
        {
            osip_content_length_t* contentLength;
            NativeMethods.osip_content_length_init(&contentLength).ThrowOnError();
            return contentLength;
        }

        public string Value
        {
            get => Marshal.PtrToStringAnsi(_native->value);
            set
            {
                Marshal.FreeHGlobal(_native->value);
                _native->value = Marshal.StringToHGlobalAnsi(value);
            }
        }

        protected static T Parse<T>(string str) where T : ValueHeaderBase, new()
        {
            var header = new T();

            var strPtr = Marshal.StringToHGlobalAnsi(str);
            NativeMethods.osip_content_length_parse(header._native, strPtr).ThrowOnError();
            Marshal.FreeHGlobal(strPtr);

            return header;
        }

        internal osip_content_length_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        public override string ToString()
        {
            IntPtr ptr;
            NativeMethods.osip_content_length_to_str(_native, &ptr).ThrowOnError();

            string str = Marshal.PtrToStringAnsi(ptr);
            NativeMethods.osip_free(ptr.ToPointer());

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_content_length_free(_native);
            _native = osip_content_length_t.Null;
        }
    }
}