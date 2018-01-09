using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class GenericParameter : OwnershipDisposable
    {
        private osip_uri_param_t* _native;

        public GenericParameter(string name, string value) : this(Create(), true)
        {
            Name = name;
            Value = value;
        }

        internal GenericParameter(osip_uri_param_t* native, bool isOwner) : base(isOwner)
        {
            _native = native;
        }

        private static osip_uri_param_t* Create()
        {
            osip_uri_param_t* param;
            NativeMethods.osip_uri_param_init(&param);
            return param;
        }

        public string Name
        {
            get => Marshal.PtrToStringAnsi(_native->gname);
            set => _native->gname = Marshal.StringToHGlobalAnsi(value?.Trim());
        }

        public string Value
        {
            get => Marshal.PtrToStringAnsi(_native->gvalue);
            set => _native->gvalue = Marshal.StringToHGlobalAnsi(value?.Trim());
        }

        internal osip_uri_param_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_uri_param_free(_native);
            _native = osip_uri_param_t.Null;
        }

    }
}