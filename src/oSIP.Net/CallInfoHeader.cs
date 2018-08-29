namespace oSIP.Net
{
    public class CallInfoHeader : ValueParametersHeaderBase
    {
        public CallInfoHeader()
        {
        }

        internal unsafe CallInfoHeader(osip_call_info_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static CallInfoHeader Parse(string str)
        {
            return Parse<CallInfoHeader>(str);
        }

        public static bool TryParse(string str, out CallInfoHeader header)
        {
            return TryParse<CallInfoHeader>(str, out header);
        }

        public unsafe CallInfoHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_call_info_t*) ptr.ToPointer();
                return new CallInfoHeader(native, true);
            });
        }
    }
}