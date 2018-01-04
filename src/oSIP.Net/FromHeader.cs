namespace oSIP.Net
{
    public class FromHeader : NameAddressHeaderBase
    {
        public FromHeader()
        {
        }

        internal unsafe FromHeader(osip_from_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static FromHeader Parse(string str)
        {
            return Parse<FromHeader>(str);
        }

        public unsafe FromHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_from_t*) ptr.ToPointer();
                return new FromHeader(native, true);
            });
        }
    }
}