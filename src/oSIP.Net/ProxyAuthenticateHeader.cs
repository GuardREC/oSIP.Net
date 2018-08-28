namespace oSIP.Net
{
    public class ProxyAuthenticateHeader : AuthenticateHeaderBase
    {
        public ProxyAuthenticateHeader()
        {
        }

        internal unsafe ProxyAuthenticateHeader(osip_www_authenticate_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static ProxyAuthenticateHeader Parse(string str)
        {
            return Parse<ProxyAuthenticateHeader>(str);
        }

        public static bool TryParse(string str, out ProxyAuthenticateHeader header)
        {
            return TryParse<ProxyAuthenticateHeader>(str, out header);
        }

        public unsafe ProxyAuthenticateHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_www_authenticate_t*) ptr.ToPointer();
                return new ProxyAuthenticateHeader(native, true);
            });
        }
    }
}