using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class AcceptEncodingHeader
    {
        public AcceptEncodingHeader()
        {
            Parameters = new List<GenericParameter>();
        }

        public string Element { get; set; }

        public List<GenericParameter> Parameters { get; }

        internal static AcceptEncodingHeader FromNative(osip_accept_encoding_t* native)
        {
            var header = new AcceptEncodingHeader()
            {
                Element = Marshal.PtrToStringAnsi(native->element)
            };

            int size = NativeMethods.osip_list_size(&native->gen_params);
            for (int i = 0; i < size; i++)
            {
                osip_uri_param_t* param = (osip_uri_param_t*) NativeMethods.osip_list_get(&native->gen_params, i);
                header.Parameters.Add(GenericParameter.FromNative(param));
            }

            return header;
        }

        internal osip_accept_encoding_t* ToNative()
        {
            osip_accept_encoding_t* native;
            NativeMethods.osip_accept_encoding_init(&native).ThrowOnError();

            native->element = Marshal.StringToHGlobalAnsi(Element);

            for (int i = 0; i < Parameters.Count; i++)
            {
                osip_uri_param_t* param = Parameters[i].ToNative();
                NativeMethods.osip_list_add(&native->gen_params, param, i).ThrowOnError();
            }

            return native;
        }

        public static AcceptEncodingHeader Parse(string str)
        {
            TryParseCore(str, out AcceptEncodingHeader header).ThrowOnError();
            return header;
        }

        public static bool TryParse(string str, out AcceptEncodingHeader header)
        {
            return TryParseCore(str, out header).EnsureSuccess();
        }

        private static ErrorCode TryParseCore(string str, out AcceptEncodingHeader header)
        {
            osip_accept_encoding_t* native = null;
            var strPtr = Marshal.StringToHGlobalAnsi(str);

            try
            {
                ErrorCode errorCode = NativeMethods.osip_accept_encoding_init(&native);
                if (!errorCode.EnsureSuccess())
                {
                    header = null;
                    return errorCode;
                }

                errorCode = NativeMethods.osip_accept_encoding_parse(native, strPtr);
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
                NativeMethods.osip_accept_encoding_free(native);
                Marshal.FreeHGlobal(strPtr);
            }
        }

        public AcceptEncodingHeader DeepClone()
        {
            osip_accept_encoding_t* native = ToNative();

            try
            {
                return FromNative(native);
            }
            finally
            {
                NativeMethods.osip_accept_encoding_free(native);
            }
        }

        public override string ToString()
        {
            IntPtr ptr = IntPtr.Zero;
            osip_accept_encoding_t* native = ToNative();

            try
            {
                NativeMethods.osip_accept_encoding_to_str(native, &ptr).ThrowOnError();
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                NativeMethods.osip_accept_encoding_free(native);
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}