namespace oSIP.Net
{
    public unsafe class SipResponse : SipMessage
    {
        public int StatusCode { get; set; }

        public string ReasonPhrase { get; set; }

        public SipResponse DeepClone()
        {
            return DeepClone<SipResponse>();
        }
    }
}