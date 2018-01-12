using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class GenericHeader : OwnershipDisposable
    {
        private osip_header_t* _native;

        public GenericHeader(string name, string value) : this(Create(), true)
        {
            Name = name;
            Value = value;
        }

        internal GenericHeader(osip_header_t* native, bool isOwner) : base(isOwner)
        {
            _native = native;
        }

        private static osip_header_t* Create()
        {
            osip_header_t* native;
            NativeMethods.osip_header_init(&native).ThrowOnError();
            return native;
        }

        public string Name
        {
            get => Marshal.PtrToStringAnsi(_native->hname);
            set
            {
                Marshal.FreeHGlobal(_native->hname);
                _native->hname = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Value
        {
            get => Marshal.PtrToStringAnsi(_native->hvalue);
            set
            {
                Marshal.FreeHGlobal(_native->hvalue);
                _native->hvalue = Marshal.StringToHGlobalAnsi(value);
            }
        }

        internal osip_header_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        public GenericHeader DeepClone()
        {
            osip_header_t* native;
            NativeMethods.osip_header_clone(_native, &native).ThrowOnError();
            return new GenericHeader(native, true);
        }

        public override string ToString()
        {
            IntPtr ptr;
            NativeMethods.osip_header_to_str(_native, &ptr).ThrowOnError();

            string str = Marshal.PtrToStringAnsi(ptr);
            Marshal.FreeHGlobal(ptr);

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_header_free(_native);
            _native = osip_header_t.Null;
        }
    }
}