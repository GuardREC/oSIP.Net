namespace oSIP.Net
{
    public class RouteHeader : NameAddressHeaderBase
    {
        public RouteHeader()
        {
        }

        internal unsafe RouteHeader(osip_from_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static RouteHeader Parse(string str)
        {
            return Parse<RouteHeader>(str);
        }

        public unsafe RouteHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_from_t*) ptr.ToPointer();
                return new RouteHeader(native, true);
            });
        }
    }
}