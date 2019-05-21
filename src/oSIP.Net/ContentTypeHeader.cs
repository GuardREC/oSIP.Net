using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class ContentTypeHeader
    {
        public ContentTypeHeader()
        {
            Parameters = new List<GenericParameter>();
        }

        public string Type { get; set; }

        public string SubType { get; set; }

        public List<GenericParameter> Parameters { get; }

        internal static ContentTypeHeader FromNative(osip_content_type_t* native)
        {
            var header = new ContentTypeHeader
            {
                Type = Marshal.PtrToStringAnsi(native->type),
                SubType = Marshal.PtrToStringAnsi(native->subtype)
            };

            int size = NativeMethods.osip_list_size(&native->gen_params);
            for (int i = 0; i < size; i++)
            {
                osip_uri_param_t* param = (osip_uri_param_t*) NativeMethods.osip_list_get(&native->gen_params, i);
                header.Parameters.Add(GenericParameter.FromNative(param));
            }

            return header;
        }

        internal osip_content_type_t* ToNative()
        {
            osip_content_type_t* native;
            NativeMethods.osip_content_type_init(&native).ThrowOnError();

            native->type = Marshal.StringToHGlobalAnsi(Type);
            native->subtype = Marshal.StringToHGlobalAnsi(SubType);

            for (int i = 0; i < Parameters.Count; i++)
            {
                osip_uri_param_t* param = Parameters[i].ToNative();
                NativeMethods.osip_list_add(&native->gen_params, param, i).ThrowOnError();
            }

            return native;
        }

        public static ContentTypeHeader Parse(string str)
        {
            TryParseCore(str, out ContentTypeHeader header).ThrowOnError();
            return header;
        }

        public static bool TryParse(string str, out ContentTypeHeader header)
        {
            return TryParseCore(str, out header).EnsureSuccess();
        }

        private static ErrorCode TryParseCore(string str, out ContentTypeHeader header)
        {
            osip_content_type_t* native = null;
            var strPtr = Marshal.StringToHGlobalAnsi(str);

            try
            {
                ErrorCode errorCode = NativeMethods.osip_content_type_init(&native);
                if (!errorCode.EnsureSuccess())
                {
                    header = null;
                    return errorCode;
                }

                errorCode = NativeMethods.osip_content_type_parse(native, strPtr);
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
                NativeMethods.osip_content_type_free(native);
                Marshal.FreeHGlobal(strPtr);
            }
        }

        public ContentTypeHeader DeepClone()
        {
            osip_content_type_t* native = ToNative();

            try
            {
                return FromNative(native);
            }
            finally
            {
                NativeMethods.osip_content_type_free(native);
            }
        }

        public override string ToString()
        {
            IntPtr ptr = IntPtr.Zero;
            osip_content_type_t* native = ToNative();

            try
            {
                NativeMethods.osip_content_type_to_str(native, &ptr).ThrowOnError();
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                NativeMethods.osip_content_type_free(native);
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}