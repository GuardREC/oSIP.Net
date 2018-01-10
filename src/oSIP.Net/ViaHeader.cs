using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class ViaHeader : OwnershipDisposable
    {
        private osip_via_t* _native;

        public ViaHeader() : this(Create(), true)
        {
        }

        internal ViaHeader(osip_via_t* native, bool isOwner) : base(isOwner)
        {
            _native = native;
            Parameters = new LinkedList<GenericParameter>(
                &_native->via_params,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new GenericParameter((osip_uri_param_t*) ptr, false));
        }

        private static osip_via_t* Create()
        {
            osip_via_t* native;
            NativeMethods.osip_via_init(&native).ThrowOnError();
            return native;
        }

        public string Version
        {
            get => Marshal.PtrToStringAnsi(_native->version);
            set
            {
                Marshal.FreeHGlobal(_native->version);
                _native->version = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Protocol
        {
            get => Marshal.PtrToStringAnsi(_native->protocol);
            set
            {
                Marshal.FreeHGlobal(_native->protocol);
                _native->protocol = Marshal.StringToHGlobalAnsi(value);
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

        public string Comment
        {
            get => Marshal.PtrToStringAnsi(_native->comment);
            set
            {
                Marshal.FreeHGlobal(_native->comment);
                _native->comment = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public LinkedList<GenericParameter> Parameters { get; }

        public static ViaHeader Parse(string str)
        {
            var header = new ViaHeader();

            var strPtr = Marshal.StringToHGlobalAnsi(str);
            NativeMethods.osip_via_parse(header._native, strPtr).ThrowOnError();
            Marshal.FreeHGlobal(strPtr);

            return header;
        }

        internal osip_via_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }
        
        public ViaHeader DeepClone()
        {
            osip_via_t* via;
            NativeMethods.osip_via_clone(_native, &via).ThrowOnError();
            return new ViaHeader(via, true);
        }

        public override string ToString()
        {
            IntPtr ptr;
            NativeMethods.osip_via_to_str(_native, &ptr).ThrowOnError();

            string str = Marshal.PtrToStringAnsi(ptr);
            NativeMethods.osip_free(ptr.ToPointer());

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_via_free(_native);
            _native = osip_via_t.Null;
        }
    }
}