using System;

namespace oSIP.Net
{
    public class SipException : Exception
    {
        public SipException(string message) : base(message)
        {
        }
    }
}