namespace oSIP.Net
{
    public class AcceptHeader : MediaHeaderBase
    {
        public AcceptHeader()
        {
        }

        internal unsafe AcceptHeader(osip_content_type_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static AcceptHeader Parse(string str)
        {
            return Parse<AcceptHeader>(str);
        }

        public static bool TryParse(string str, out AcceptHeader header)
        {
            return TryParse<AcceptHeader>(str, out header);
        }

        public unsafe AcceptHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_content_type_t*) ptr.ToPointer();
                return new AcceptHeader(native, true);
            });
        }
    }
}