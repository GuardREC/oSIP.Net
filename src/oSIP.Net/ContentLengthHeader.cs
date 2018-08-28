namespace oSIP.Net
{
    public class ContentLengthHeader : ValueHeaderBase
    {
        public ContentLengthHeader()
        {
        }

        internal unsafe ContentLengthHeader(osip_content_length_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static ContentLengthHeader Parse(string str)
        {
            return Parse<ContentLengthHeader>(str);
        }

        public static bool TryParse(string str, out ContentLengthHeader header)
        {
            return TryParse<ContentLengthHeader>(str, out header);
        }
    }
}