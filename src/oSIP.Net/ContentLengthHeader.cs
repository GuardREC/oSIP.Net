using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class ContentLengthHeader
    {
        public string Value { get; set; }

        internal static ContentLengthHeader FromNative(osip_content_length_t* native)
        {
            var header = new ContentLengthHeader
            {
                Value = Marshal.PtrToStringAnsi(native->value)
            };

            return header;
        }

        internal osip_content_length_t* ToNative()
        {
            osip_content_length_t* native;
            NativeMethods.osip_content_length_init(&native).ThrowOnError();

            native->value = Marshal.StringToHGlobalAnsi(Value);

            return native;
        }

        public static ContentLengthHeader Parse(string str)
        {
            TryParseCore(str, out ContentLengthHeader header).ThrowOnError();
            return header;
        }

        public static bool TryParse(string str, out ContentLengthHeader header)
        {
            return TryParseCore(str, out header).EnsureSuccess();
        }

        private static ErrorCode TryParseCore(string str, out ContentLengthHeader header)
        {
            osip_content_length_t* native = null;
            var strPtr = Marshal.StringToHGlobalAnsi(str);

            try
            {
                ErrorCode errorCode = NativeMethods.osip_content_length_init(&native);
                if (!errorCode.EnsureSuccess())
                {
                    header = null;
                    return errorCode;
                }

                errorCode = NativeMethods.osip_content_length_parse(native, strPtr);
                if (!errorCode.EnsureSuccess())
                {
                    header = null;
                    return errorCode;
                }

                header = FromNative(native);
                return errorCode;
            }
            finally
            {
                NativeMethods.osip_content_length_free(native);
                Marshal.FreeHGlobal(strPtr);
            }
        }

        public ContentLengthHeader DeepClone()
        {
            osip_content_length_t* native = ToNative();

            try
            {
                return FromNative(native);
            }
            finally
            {
                NativeMethods.osip_content_length_free(native);
            }
        }

        public override string ToString()
        {
            IntPtr ptr = IntPtr.Zero;
            osip_content_length_t* native = ToNative();

            try
            {
                NativeMethods.osip_content_length_to_str(native, &ptr).ThrowOnError();
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                NativeMethods.osip_content_length_free(native);
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}