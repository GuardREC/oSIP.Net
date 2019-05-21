using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class ViaHeader
    {
        public ViaHeader()
        {
            Parameters = new List<GenericParameter>();
        }

        public string Version { get; set; }

        public string Protocol { get; set; }

        public string Host { get; set; }

        public string Port { get; set; }

        public string Comment { get; set; }

        public List<GenericParameter> Parameters { get; }

        internal static ViaHeader FromNative(osip_via_t* native)
        {
            var header = new ViaHeader
            {
                Version = Marshal.PtrToStringAnsi(native->version),
                Protocol = Marshal.PtrToStringAnsi(native->protocol),
                Host = Marshal.PtrToStringAnsi(native->host),
                Port = Marshal.PtrToStringAnsi(native->port),
                Comment = Marshal.PtrToStringAnsi(native->comment)
            };

            int size = NativeMethods.osip_list_size(&native->via_params);
            for (int i = 0; i < size; i++)
            {
                osip_uri_param_t* param = (osip_uri_param_t*) NativeMethods.osip_list_get(&native->via_params, i);
                header.Parameters.Add(GenericParameter.FromNative(param));
            }

            return header;
        }

        internal osip_via_t* ToNative()
        {
            osip_via_t* native;
            NativeMethods.osip_via_init(&native).ThrowOnError();

            native->version = Marshal.StringToHGlobalAnsi(Version);
            native->protocol = Marshal.StringToHGlobalAnsi(Protocol);
            native->host = Marshal.StringToHGlobalAnsi(Host);
            native->port = Marshal.StringToHGlobalAnsi(Port);
            native->comment = Marshal.StringToHGlobalAnsi(Comment);

            for (int i = 0; i < Parameters.Count; i++)
            {
                osip_uri_param_t* param = Parameters[i].ToNative();
                NativeMethods.osip_list_add(&native->via_params, param, i).ThrowOnError();
            }

            return native;
        }

        public static ViaHeader Parse(string str)
        {
            TryParseCore(str, out ViaHeader header).ThrowOnError();
            return header;
        }

        public static bool TryParse(string str, out ViaHeader header)
        {
            return TryParseCore(str, out header).EnsureSuccess();
        }

        private static ErrorCode TryParseCore(string str, out ViaHeader header)
        {
            osip_via_t* native = null;
            var strPtr = Marshal.StringToHGlobalAnsi(str);

            try
            {
                ErrorCode errorCode = NativeMethods.osip_via_init(&native);
                if (!errorCode.EnsureSuccess())
                {
                    header = null;
                    return errorCode;
                }

                errorCode = NativeMethods.osip_via_parse(native, strPtr);
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
                NativeMethods.osip_via_free(native);
                Marshal.FreeHGlobal(strPtr);
            }
        }

        public ViaHeader DeepClone()
        {
            osip_via_t* native = ToNative();

            try
            {
                return FromNative(native);
            }
            finally
            {
                NativeMethods.osip_via_free(native);
            }
        }

        public override string ToString()
        {
            IntPtr ptr = IntPtr.Zero;
            osip_via_t* native = ToNative();

            try
            {
                NativeMethods.osip_via_to_str(native, &ptr).ThrowOnError();
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                NativeMethods.osip_via_free(native);
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}