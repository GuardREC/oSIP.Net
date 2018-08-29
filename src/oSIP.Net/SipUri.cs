using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class SipUri : OwnershipDisposable
    {
        private osip_uri_t* _native;

        public SipUri() : this(Create(), true)
        {
        }

        internal SipUri(osip_uri_t* native, bool isOwner) : base(isOwner)
        {
            _native = native;
            Parameters = CreateParameterList(&_native->url_params);
            Headers = CreateParameterList(&_native->url_headers);
        }

        private static osip_uri_t* Create()
        {
            osip_uri_t* uri;
            NativeMethods.osip_uri_init(&uri).ThrowOnError();
            return uri;
        }

        private static LinkedList<SipUriParameter> CreateParameterList(osip_list_t* list)
        {
            return new LinkedList<SipUriParameter>(
                list,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new SipUriParameter((osip_uri_param_t*) ptr, false));
        }

        public string Scheme
        {
            get => Marshal.PtrToStringAnsi(_native->scheme);
            set
            {
                Marshal.FreeHGlobal(_native->scheme);
                _native->scheme = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Host
        {
            get => Marshal.PtrToStringAnsi(_native->host);
            set
            {
                Marshal.FreeHGlobal(_native->host);
                _native->host = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Port
        {
            get => Marshal.PtrToStringAnsi(_native->port);
            set
            {
                Marshal.FreeHGlobal(_native->port);
                _native->port = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Username
        {
            get => Marshal.PtrToStringAnsi(_native->username);
            set
            {
                Marshal.FreeHGlobal(_native->username);
                _native->username = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Password
        {
            get => Marshal.PtrToStringAnsi(_native->password);
            set
            {
                Marshal.FreeHGlobal(_native->password);
                _native->password = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public LinkedList<SipUriParameter> Parameters { get; }

        public LinkedList<SipUriParameter> Headers { get; }

        public static SipUri Parse(string str)
        {
            TryParseCore(str, out SipUri uri).ThrowOnError(uri);
            return uri;
        }

        public static bool TryParse(string str, out SipUri uri)
        {
            return TryParseCore(str, out uri).EnsureSuccess(ref uri);
        }

        private static ErrorCode TryParseCore(string str, out SipUri uri)
        {
            var strPtr = Marshal.StringToHGlobalAnsi(str);
            try
            {
                uri = new SipUri();
                return NativeMethods.osip_uri_parse(uri._native, strPtr);
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
            }
        }

        internal osip_uri_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        public SipUri DeepClone()
        {
            osip_uri_t* uri;
            NativeMethods.osip_uri_clone(_native, &uri).ThrowOnError();
            return new SipUri(uri, true);
        }

        public override string ToString()
        {
            IntPtr ptr;
            NativeMethods.osip_uri_to_str(_native, &ptr).ThrowOnError();

            string str = Marshal.PtrToStringAnsi(ptr);
            Marshal.FreeHGlobal(ptr);

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_uri_free(_native);
            _native = osip_uri_t.Null;
        }
    }
}