using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class GenericHeader
    {
        public GenericHeader()
        {
        }

        public GenericHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        internal static GenericHeader FromNative(osip_header_t* native)
        {
            var header = new GenericHeader
            {
                Name = Marshal.PtrToStringAnsi(native->hname),
                Value = Marshal.PtrToStringAnsi(native->hvalue)
            };

            return header;
        }

        internal osip_header_t* ToNative()
        {
            osip_header_t* native;
            NativeMethods.osip_header_init(&native).ThrowOnError();

            native->hname = Marshal.StringToHGlobalAnsi(Name);
            native->hvalue = Marshal.StringToHGlobalAnsi(Value);

            return native;
        }

        public GenericHeader DeepClone()
        {
            osip_header_t* native = ToNative();

            try
            {
                return FromNative(native);
            }
            finally
            {
                NativeMethods.osip_header_free(native);
            }
        }

        public override string ToString()
        {
            IntPtr ptr = IntPtr.Zero;
            osip_header_t* native = ToNative();

            try
            {
                NativeMethods.osip_header_to_str(native, &ptr).ThrowOnError();
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                NativeMethods.osip_header_free(native);
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}