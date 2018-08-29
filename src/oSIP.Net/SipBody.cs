using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class SipBody : OwnershipDisposable
    {
        private osip_body_t* _native;

        public SipBody() : this(Create(), true)
        {
        }

        internal SipBody(osip_body_t* native, bool isOwner) : base(isOwner)
        {
            _native = native;
            Headers = new LinkedList<GenericHeader>(
                _native->headers,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new GenericHeader((osip_header_t*) ptr, false));
        }

        private static osip_body_t* Create()
        {
            osip_body_t* native;
            NativeMethods.osip_body_init(&native).ThrowOnError();
            return native;
        }

        public string Data
        {
            get => Marshal.PtrToStringAnsi(_native->body, (int) _native->length);
            set
            {
                Marshal.FreeHGlobal(_native->body);
                _native->body = Marshal.StringToHGlobalAnsi(value);
                _native->length = (ulong) value.Length;
            }
        }

        public ContentTypeHeader ContentType
        {
            get => _native->content_type != osip_content_type_t.Null
                ? new ContentTypeHeader(_native->content_type, false)
                : null;
            set
            {
                NativeMethods.osip_content_type_free(_native->content_type);
                _native->content_type = value != null
                    ? value.TakeOwnership()
                    : osip_content_type_t.Null;
            }
        }

        public LinkedList<GenericHeader> Headers { get; }

        public static SipBody Parse(string str)
        {
            TryParseCore(str, out SipBody body).ThrowOnError(body);
            return body;
        }

        public static bool TryParse(string str, out SipBody body)
        {
            return TryParseCore(str, out body).EnsureSuccess(ref body);
        }

        private static ErrorCode TryParseCore(string str, out SipBody body)
        {
            var strPtr = Marshal.StringToHGlobalAnsi(str);
            try
            {
                body = new SipBody();
                return NativeMethods.osip_body_parse(body._native, strPtr, (ulong) str.Length);
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
            }
        }

        public static SipBody ParseMime(string str)
        {
            TryParseMimeCore(str, out SipBody body).ThrowOnError(body);
            return body;
        }

        public static bool TryParseMime(string str, out SipBody body)
        {
            return TryParseMimeCore(str, out body).EnsureSuccess(ref body);
        }

        private static ErrorCode TryParseMimeCore(string str, out SipBody body)
        {
            var strPtr = Marshal.StringToHGlobalAnsi(str);
            try
            {
                body = new SipBody();
                return NativeMethods.osip_body_parse_mime(body._native, strPtr, (ulong) str.Length);
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
            }
        }

        internal osip_body_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        public SipBody DeepClone()
        {
            osip_body_t* native;
            NativeMethods.osip_body_clone(_native, &native).ThrowOnError();
            return new SipBody(native, true);
        }

        public override string ToString()
        {
            IntPtr ptr;
            ulong length;
            NativeMethods.osip_body_to_str(_native, &ptr, &length).ThrowOnError();

            string str = Marshal.PtrToStringAnsi(ptr, (int) length);
            Marshal.FreeHGlobal(ptr);

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_body_free(_native);
            _native = osip_body_t.Null;
        }
    }
}