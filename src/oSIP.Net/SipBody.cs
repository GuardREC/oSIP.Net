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
        }

        private static osip_body_t* Create()
        {
            osip_body_t* native;
            NativeMethods.osip_body_init(&native);
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

        public static SipBody Parse(string str)
        {
            var body = new SipBody();

            var strPtr = Marshal.StringToHGlobalAnsi(str);
            NativeMethods.osip_body_parse(body._native, strPtr, (ulong) str.Length);
            Marshal.FreeHGlobal(strPtr);

            return body;
        }

        public static SipBody ParseMime(string str)
        {
            var body = new SipBody();

            var strPtr = Marshal.StringToHGlobalAnsi(str);
            NativeMethods.osip_body_parse_mime(body._native, strPtr, (ulong) str.Length);
            Marshal.FreeHGlobal(strPtr);

            return body;
        }

        internal osip_body_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        public SipBody DeepClone()
        {
            osip_body_t* native;
            NativeMethods.osip_body_clone(_native, &native);
            return new SipBody(native, true);
        }

        public override string ToString()
        {
            IntPtr ptr;
            ulong length;
            NativeMethods.osip_body_to_str(_native, &ptr, &length);

            string str = Marshal.PtrToStringAnsi(ptr, (int) length);
            NativeMethods.osip_free(ptr.ToPointer());

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_body_free(_native);
            _native = osip_body_t.Null;
        }
    }
}