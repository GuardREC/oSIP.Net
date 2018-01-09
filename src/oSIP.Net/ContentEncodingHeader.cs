namespace oSIP.Net
{
    public class ContentEncodingHeader : ValueHeaderBase
    {
        public ContentEncodingHeader()
        {
        }

        internal unsafe ContentEncodingHeader(osip_content_length_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static ContentEncodingHeader Parse(string str)
        {
            return Parse<ContentEncodingHeader>(str);
        }
    }
}