namespace oSIP.Net
{
    public unsafe class SipUriParameter : GenericParameter
    {
        public SipUriParameter(string name, string value) : base(name, value)
        {
        }
 
        internal SipUriParameter(osip_uri_param_t* native, bool isOwner) : base(native, isOwner)
        {
        }
    }
}