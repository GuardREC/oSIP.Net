namespace oSIP.Net
{
    public class ToHeader : NameAddressHeaderBase
    {
        public ToHeader()
        {
        }

        internal unsafe ToHeader(osip_from_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static ToHeader Parse(string str)
        {
            return Parse<ToHeader>(str);
        }

        public static bool TryParse(string str, out ToHeader header)
        {
            return TryParse<ToHeader>(str, out header);
        }

        public unsafe ToHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_from_t*) ptr.ToPointer();
                return new ToHeader(native, true);
            });
        }
    }
}