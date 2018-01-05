namespace oSIP.Net
{
    public class AcceptEncodingHeader : AcceptHeaderBase
    {
        public AcceptEncodingHeader()
        {
        }

        internal unsafe AcceptEncodingHeader(osip_accept_encoding_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static AcceptEncodingHeader Parse(string str)
        {
            return Parse<AcceptEncodingHeader>(str);
        }

        public unsafe AcceptEncodingHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_accept_encoding_t*) ptr.ToPointer();
                return new AcceptEncodingHeader(native, true);
            });
        }
    }
}