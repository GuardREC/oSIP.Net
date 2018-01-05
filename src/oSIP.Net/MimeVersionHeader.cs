namespace oSIP.Net
{
    public class MimeVersionHeader : ValueHeaderBase
    {
        public MimeVersionHeader()
        {
        }

        internal unsafe MimeVersionHeader(osip_content_length_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static MimeVersionHeader Parse(string str)
        {
            return Parse<MimeVersionHeader>(str);
        }
    }
}