using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class GenericParameter
    {
        public GenericParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        internal static GenericParameter FromNative(osip_uri_param_t* native)
        {
            return new GenericParameter(
                Marshal.PtrToStringAnsi(native->gname),
                Marshal.PtrToStringAnsi(native->gvalue));
        }

        internal osip_uri_param_t* ToNative()
        {
            osip_uri_param_t* native;
            NativeMethods.osip_uri_param_init(&native).ThrowOnError();

            native->gname = Marshal.StringToHGlobalAnsi(Name);
            native->gvalue = Marshal.StringToHGlobalAnsi(Value);

            return native;
        }

        public string Name { get; }

        public string Value { get; }
    }
}