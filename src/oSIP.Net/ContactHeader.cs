namespace oSIP.Net
{
    public class ContactHeader : NameAddressHeaderBase
    {
        public ContactHeader()
        {
        }

        internal unsafe ContactHeader(osip_from_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static ContactHeader Parse(string str)
        {
            return Parse<ContactHeader>(str);
        }

        public unsafe ContactHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_from_t*) ptr.ToPointer();
                return new ContactHeader(native, true);
            });
        }
    }
}