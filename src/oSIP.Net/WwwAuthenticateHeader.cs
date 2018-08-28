namespace oSIP.Net
{
    public class WwwAuthenticateHeader : AuthenticateHeaderBase
    {
        public WwwAuthenticateHeader()
        {
        }

        internal unsafe WwwAuthenticateHeader(osip_www_authenticate_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static WwwAuthenticateHeader Parse(string str)
        {
            return Parse<WwwAuthenticateHeader>(str);
        }

        public static bool TryParse(string str, out WwwAuthenticateHeader header)
        {
            return TryParse<WwwAuthenticateHeader>(str, out header);
        }

        public unsafe WwwAuthenticateHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_www_authenticate_t*) ptr.ToPointer();
                return new WwwAuthenticateHeader(native, true);
            });
        }
    }
}