using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class CSeqHeader
    {
        public string Method { get; set; }

        public string Number { get; set; }

        internal static CSeqHeader FromNative(osip_cseq_t* native)
        {
            var header = new CSeqHeader
            {
                Method = Marshal.PtrToStringAnsi(native->method),
                Number = Marshal.PtrToStringAnsi(native->number)
            };

            return header;
        }

        internal osip_cseq_t* ToNative()
        {
            osip_cseq_t* native;
            NativeMethods.osip_cseq_init(&native).ThrowOnError();

            native->method = Marshal.StringToHGlobalAnsi(Method);
            native->number = Marshal.StringToHGlobalAnsi(Number);

            return native;
        }

        public static CSeqHeader Parse(string str)
        {
            TryParseCore(str, out CSeqHeader header).ThrowOnError();
            return header;
        }

        public static bool TryParse(string str, out CSeqHeader header)
        {
            return TryParseCore(str, out header).EnsureSuccess();
        }

        private static ErrorCode TryParseCore(string str, out CSeqHeader header)
        {
            osip_cseq_t* native = null;
            var strPtr = Marshal.StringToHGlobalAnsi(str);

            try
            {
                ErrorCode errorCode = NativeMethods.osip_cseq_init(&native);
                if (!errorCode.EnsureSuccess())
                {
                    header = null;
                    return errorCode;
                }

                errorCode = NativeMethods.osip_cseq_parse(native, strPtr);
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
                NativeMethods.osip_cseq_free(native);
                Marshal.FreeHGlobal(strPtr);
            }
        }

        public CSeqHeader DeepClone()
        {
            osip_cseq_t* native = ToNative();

            try
            {
                return FromNative(native);
            }
            finally
            {
                NativeMethods.osip_cseq_free(native);
            }
        }

        public override string ToString()
        {
            IntPtr ptr = IntPtr.Zero;
            osip_cseq_t* native = ToNative();

            try
            {
                NativeMethods.osip_cseq_to_str(native, &ptr).ThrowOnError();
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                NativeMethods.osip_cseq_free(native);
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}