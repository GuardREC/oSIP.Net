namespace oSIP.Net
{
    public class AcceptLanguageHeader : AcceptHeaderBase
    {
        public AcceptLanguageHeader()
        {
        }

        internal unsafe AcceptLanguageHeader(osip_accept_encoding_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static AcceptLanguageHeader Parse(string str)
        {
            return Parse<AcceptLanguageHeader>(str);
        }

        public unsafe AcceptLanguageHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_accept_encoding_t*) ptr.ToPointer();
                return new AcceptLanguageHeader(native, true);
            });
        }
    }
}