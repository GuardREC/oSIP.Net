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
            TryParseCore(str, out T header).ThrowOnError(header);
            return header;
        }

        protected static bool TryParse<T>(string str, out T header) where T : ValueHeaderBase, new()
        {
            return TryParseCore(str, out header).EnsureSuccess(ref header);
        }

        private static ErrorCode TryParseCore<T>(string str, out T header) where T : ValueHeaderBase, new()
        {
            var strPtr = Marshal.StringToHGlobalAnsi(str);
            try
            {
                header = new T();
                return NativeMethods.osip_content_length_parse(header._native, strPtr);
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
            }
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
            Marshal.FreeHGlobal(ptr);

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_content_length_free(_native);
            _native = osip_content_length_t.Null;
        }
    }
}