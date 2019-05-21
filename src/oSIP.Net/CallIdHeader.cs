using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class CallIdHeader
    {
        public string Number { get; set; }

        public string Host { get; set; }

        internal static CallIdHeader FromNative(osip_call_id_t* native)
        {
            var header = new CallIdHeader
            {
                Number = Marshal.PtrToStringAnsi(native->number),
                Host = Marshal.PtrToStringAnsi(native->host)
            };

            return header;
        }

        internal osip_call_id_t* ToNative()
        {
            osip_call_id_t* native;
            NativeMethods.osip_call_id_init(&native).ThrowOnError();

            native->number = Marshal.StringToHGlobalAnsi(Number);
            native->host = Marshal.StringToHGlobalAnsi(Host);

            return native;
        }

        public static CallIdHeader Parse(string str)
        {
            TryParseCore(str, out CallIdHeader header).ThrowOnError();
            return header;
        }

        public static bool TryParse(string str, out CallIdHeader header)
        {
            return TryParseCore(str, out header).EnsureSuccess();
        }

        private static ErrorCode TryParseCore(string str, out CallIdHeader header)
        {
            osip_call_id_t* native = null;
            var strPtr = Marshal.StringToHGlobalAnsi(str);

            try
            {
                ErrorCode errorCode = NativeMethods.osip_call_id_init(&native);
                if (!errorCode.EnsureSuccess())
                {
                    header = null;
                    return errorCode;
                }

                errorCode = NativeMethods.osip_call_id_parse(native, strPtr);
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
                NativeMethods.osip_call_id_free(native);
                Marshal.FreeHGlobal(strPtr);
            }
        }

        public CallIdHeader DeepClone()
        {
            osip_call_id_t* native = ToNative();

            try
            {
                return FromNative(native);
            }
            finally
            {
                NativeMethods.osip_call_id_free(native);
            }
        }

        public override string ToString()
        {
            IntPtr ptr = IntPtr.Zero;
            osip_call_id_t* native = ToNative();

            try
            {
                NativeMethods.osip_call_id_to_str(native, &ptr).ThrowOnError();
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                NativeMethods.osip_call_id_free(native);
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}