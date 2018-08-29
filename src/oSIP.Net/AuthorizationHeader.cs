namespace oSIP.Net
{
    public class AuthorizationHeader : AuthorizationHeaderBase
    {
        public AuthorizationHeader()
        {
        }

        internal unsafe AuthorizationHeader(osip_authorization_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static AuthorizationHeader Parse(string str)
        {
            return Parse<AuthorizationHeader>(str);
        }

        public static bool TryParse(string str, out AuthorizationHeader header)
        {
            return TryParse<AuthorizationHeader>(str, out header);
        }

        public unsafe AuthorizationHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_authorization_t*) ptr.ToPointer();
                return new AuthorizationHeader(native, true);
            });
        }
    }
}