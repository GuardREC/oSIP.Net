namespace oSIP.Net
{
    public class AllowHeader : ValueHeaderBase
    {
        public AllowHeader()
        {
        }

        internal unsafe AllowHeader(osip_content_length_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static AllowHeader Parse(string str)
        {
            return Parse<AllowHeader>(str);
        }
    }
}