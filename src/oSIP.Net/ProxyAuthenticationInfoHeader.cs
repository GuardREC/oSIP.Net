namespace oSIP.Net
{
    public class ProxyAuthenticationInfoHeader : AuthenticationInfoHeaderBase
    {
        public ProxyAuthenticationInfoHeader()
        {
        }

        internal unsafe ProxyAuthenticationInfoHeader(osip_authentication_info_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static ProxyAuthenticationInfoHeader Parse(string str)
        {
            return Parse<ProxyAuthenticationInfoHeader>(str);
        }

        public static bool TryParse(string str, out ProxyAuthenticationInfoHeader header)
        {
            return TryParse<ProxyAuthenticationInfoHeader>(str, out header);
        }

        public unsafe ProxyAuthenticationInfoHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_authentication_info_t*) ptr.ToPointer();
                return new ProxyAuthenticationInfoHeader(native, true);
            });
        }
    }
}