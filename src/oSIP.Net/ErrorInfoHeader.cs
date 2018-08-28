namespace oSIP.Net
{
    public class ErrorInfoHeader : ValueParametersHeaderBase
    {
        public ErrorInfoHeader()
        {
        }

        internal unsafe ErrorInfoHeader(osip_call_info_t* native, bool isOwner)
            :
            base(native, isOwner)
        {
        }

        public static ErrorInfoHeader Parse(string str)
        {
            return Parse<ErrorInfoHeader>(str);
        }

        public static bool TryParse(string str, out ErrorInfoHeader header)
        {
            return TryParse<ErrorInfoHeader>(str, out header);
        }

        public unsafe ErrorInfoHeader DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_call_info_t*) ptr.ToPointer();
                return new ErrorInfoHeader(native, true);
            });
        }
    }
}