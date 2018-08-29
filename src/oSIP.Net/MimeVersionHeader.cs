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

        public static bool TryParse(string str, out MimeVersionHeader header)
        {
            return TryParse<MimeVersionHeader>(str, out header);
        }
    }
}