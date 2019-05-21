namespace oSIP.Net
{
    public unsafe class SipRequest : SipMessage
    {
        public string Method { get; set; }

        public SipUri RequestUri { get; set; }

        public SipRequest DeepClone()
        {
            return DeepClone<SipRequest>();
        }
    }
}