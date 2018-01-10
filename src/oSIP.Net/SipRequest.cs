using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class SipRequest : SipMessage
    {
        public SipRequest()
        {
        }

        internal SipRequest(osip_message_t* native) : base(native)
        {
        }

        public string Method
        {
            get => Marshal.PtrToStringAnsi(Native->sip_method);
            set
            {
                Marshal.FreeHGlobal(Native->sip_method);
                Native->sip_method = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public SipUri RequestUri
        {
            get => Native->req_uri != osip_uri_t.Null
                ? new SipUri(Native->req_uri, false)
                : null;
            set
            {
                NativeMethods.osip_uri_free(Native->req_uri);
                Native->req_uri = value != null
                    ? value.TakeOwnership()
                    : osip_uri_t.Null;
            }
        }
    }
}