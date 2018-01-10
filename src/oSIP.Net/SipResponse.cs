using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class SipResponse : SipMessage
    {
        public SipResponse()
        {
        }

        internal SipResponse(osip_message_t* native) : base(native)
        {
        }

        public int StatusCode
        {
            get => Native->status_code;
            set => Native->status_code = value;
        }

        public string ReasonPhrase
        {
            get => Marshal.PtrToStringAnsi(Native->reason_phrase);
            set
            {
                Marshal.FreeHGlobal(Native->reason_phrase);
                Native->reason_phrase = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public SipResponse DeepClone()
        {
            return DeepClone(ptr =>
            {
                var native = (osip_message_t*) ptr.ToPointer();
                return new SipResponse(native);
            });
        }
    }
}