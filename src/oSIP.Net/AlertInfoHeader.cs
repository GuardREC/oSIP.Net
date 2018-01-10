namespace oSIP.Net
{
    public class AlertInfoHeader : ValueParametersHeaderBase
    {
        public AlertInfoHeader()
        {
        }

        internal unsafe AlertInfoHeader(osip_call_info_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static AlertInfoHeader Parse(string str)
        {
            return Parse<AlertInfoHeader>(str);
        }

        public unsafe AlertInfoHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_call_info_t*) ptr.ToPointer();
                return new AlertInfoHeader(native, true);
            });
        }
    }
}