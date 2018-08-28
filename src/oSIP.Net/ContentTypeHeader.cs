namespace oSIP.Net
{
    public class ContentTypeHeader : MediaHeaderBase
    {
        public ContentTypeHeader()
        {
        }

        internal unsafe ContentTypeHeader(osip_content_type_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static ContentTypeHeader Parse(string str)
        {
            return Parse<ContentTypeHeader>(str);
        }

        public static bool TryParse(string str, out ContentTypeHeader header)
        {
            return TryParse<ContentTypeHeader>(str, out header);
        }

        public unsafe ContentTypeHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_content_type_t*) ptr.ToPointer();
                return new ContentTypeHeader(native, true);
            });
        }
    }
}