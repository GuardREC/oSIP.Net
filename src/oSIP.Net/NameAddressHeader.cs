using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class NameAddressHeader
    {
        public NameAddressHeader()
        {
            Parameters = new List<GenericParameter>();
        }

        public string DisplayName { get; set; }

        public SipUri Url { get; set; }

        public List<GenericParameter> Parameters { get; }

        internal static NameAddressHeader FromNative(osip_from_t* native)
        {
            var header = new NameAddressHeader
            {
                DisplayName = Marshal.PtrToStringAnsi(native->displayname),
                Url = native->url != osip_uri_t.Null
                    ? SipUri.FromNative(native->url)
                    : null
            };

            int size = NativeMethods.osip_list_size(&native->gen_params);
            for (int i = 0; i < size; i++)
            {
                osip_uri_param_t* param = (osip_uri_param_t*)NativeMethods.osip_list_get(&native->gen_params, i);
                header.Parameters.Add(GenericParameter.FromNative(param));
            }

            return header;
        }

        internal osip_from_t* ToNative()
        {
            osip_from_t* native;
            NativeMethods.osip_from_init(&native).ThrowOnError();

            native->displayname = Marshal.StringToHGlobalAnsi(DisplayName);
            native->url = Url != null
                ? Url.ToNative()
                : osip_uri_t.Null;

            for (int i = 0; i < Parameters.Count; i++)
            {
                osip_uri_param_t* param = Parameters[i].ToNative();
                NativeMethods.osip_list_add(&native->gen_params, param, i).ThrowOnError();
            }

            return native;
        }

        public static NameAddressHeader Parse(string str)
        {
            TryParseCore(str, out NameAddressHeader header).ThrowOnError();
            return header;
        }

        public static bool TryParse(string str, out NameAddressHeader header)
        {
            return TryParseCore(str, out header).EnsureSuccess();
        }

        private static ErrorCode TryParseCore(string str, out NameAddressHeader header)
        {
            osip_from_t* native = null;
            var strPtr = Marshal.StringToHGlobalAnsi(str);

            try
            {
                ErrorCode errorCode = NativeMethods.osip_from_init(&native);
                if (!errorCode.EnsureSuccess())
                {
                    header = null;
                    return errorCode;
                }

                errorCode = NativeMethods.osip_from_parse(native, strPtr);
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
                NativeMethods.osip_from_free(native);
                Marshal.FreeHGlobal(strPtr);
            }
        }

        public NameAddressHeader DeepClone()
        {
            osip_from_t* native = ToNative();

            try
            {
                return FromNative(native);
            }
            finally
            {
                NativeMethods.osip_from_free(native);
            }
        }

        public override string ToString()
        {
            IntPtr ptr = IntPtr.Zero;
            osip_from_t* native = ToNative();

            try
            {
                NativeMethods.osip_from_to_str(native, &ptr).ThrowOnError();
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                NativeMethods.osip_from_free(native);
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}