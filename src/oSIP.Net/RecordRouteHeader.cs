namespace oSIP.Net
{
    public class RecordRouteHeader: NameAddressHeaderBase
    {
        public RecordRouteHeader()
        {
        }

        internal unsafe RecordRouteHeader(osip_from_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static RecordRouteHeader Parse(string str)
        {
            return Parse<RecordRouteHeader>(str);
        }

        public static bool TryParse(string str, out RecordRouteHeader header)
        {
            return TryParse<RecordRouteHeader>(str, out header);
        }

        public unsafe RecordRouteHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_from_t*) ptr.ToPointer();
                return new RecordRouteHeader(native, true);
            });
        }
    }
}