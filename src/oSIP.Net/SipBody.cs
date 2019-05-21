using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class SipBody
    {
        public SipBody()
        {
            Headers = new List<GenericHeader>();
        }

        public string Data { get; set; }

        public ContentTypeHeader ContentType { get; set; }

        public List<GenericHeader> Headers { get; }

        internal static SipBody FromNative(osip_body_t* native)
        {
            var body = new SipBody
            {
                Data = Marshal.PtrToStringAnsi(native->body, (int) native->length),
                ContentType = native->content_type != osip_content_type_t.Null
                    ? ContentTypeHeader.FromNative(native->content_type)
                    : null
            };

            int size = NativeMethods.osip_list_size(native->headers);
            for (int i = 0; i < size; i++)
            {
                osip_header_t* header = (osip_header_t*) NativeMethods.osip_list_get(native->headers, i);
                body.Headers.Add(GenericHeader.FromNative(header));
            }

            return body;
        }

        internal osip_body_t* ToNative()
        {
            osip_body_t* native;
            NativeMethods.osip_body_init(&native).ThrowOnError();

            native->body = Marshal.StringToHGlobalAnsi(Data);
            native->length = (ulong) Data.Length;
            native->content_type = ContentType != null
                ? ContentType.ToNative()
                : osip_content_type_t.Null;

            for (int i = 0; i < Headers.Count; i++)
            {
                osip_header_t* header = Headers[i].ToNative();
                NativeMethods.osip_list_add(native->headers, header, i).ThrowOnError();
            }

            return native;
        }

        public static SipBody Parse(string str)
        {
            TryParseCore(str, out SipBody body).ThrowOnError();
            return body;
        }

        public static bool TryParse(string str, out SipBody body)
        {
            return TryParseCore(str, out body).EnsureSuccess();
        }

        private static ErrorCode TryParseCore(string str, out SipBody body)
        {
            osip_body_t* native = null;
            var strPtr = Marshal.StringToHGlobalAnsi(str);

            try
            {
                ErrorCode errorCode = NativeMethods.osip_body_init(&native);
                if (!errorCode.EnsureSuccess())
                {
                    body = null;
                    return errorCode;
                }

                errorCode = NativeMethods.osip_body_parse(native, strPtr, (ulong) str.Length);
                if (!errorCode.EnsureSuccess())
                {
                    body = null;
                    return errorCode;
                }

                body = FromNative(native);
                return errorCode;
            }
            finally
            {
                NativeMethods.osip_body_free(native);
                Marshal.FreeHGlobal(strPtr);
            }
        }

        public static SipBody ParseMime(string str)
        {
            TryParseMimeCore(str, out SipBody body).ThrowOnError();
            return body;
        }

        public static bool TryParseMime(string str, out SipBody body)
        {
            return TryParseMimeCore(str, out body).EnsureSuccess();
        }

        private static ErrorCode TryParseMimeCore(string str, out SipBody body)
        {
            osip_body_t* native = null;
            var strPtr = Marshal.StringToHGlobalAnsi(str);

            try
            {
                ErrorCode errorCode = NativeMethods.osip_body_init(&native);
                if (!errorCode.EnsureSuccess())
                {
                    body = null;
                    return errorCode;
                }

                errorCode = NativeMethods.osip_body_parse_mime(native, strPtr, (ulong) str.Length);
                if (!errorCode.EnsureSuccess())
                {
                    body = null;
                    return errorCode;
                }

                body = FromNative(native);
                return errorCode;
            }
            finally
            {
                NativeMethods.osip_body_free(native);
                Marshal.FreeHGlobal(strPtr);
            }
        }

        public SipBody DeepClone()
        {
            osip_body_t* native = ToNative();

            try
            {
                return FromNative(native);
            }
            finally
            {
                NativeMethods.osip_body_free(native);
            }
        }

        public override string ToString()
        {
            IntPtr ptr = IntPtr.Zero;
            osip_body_t* native = ToNative();

            try
            {
                ulong length;
                NativeMethods.osip_body_to_str(native, &ptr, &length).ThrowOnError();
                return Marshal.PtrToStringAnsi(ptr, (int) length);
            }
            finally
            {
                NativeMethods.osip_body_free(native);
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}