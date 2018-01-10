using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class CallIdHeader : OwnershipDisposable
    {
        private osip_call_id_t* _native;

        public CallIdHeader() : this(Create(), true)
        {
        }

        internal CallIdHeader(osip_call_id_t* native, bool isOwner) : base(isOwner)
        {
            _native = native;
        }

        private static osip_call_id_t* Create()
        {
            osip_call_id_t* callId;
            NativeMethods.osip_call_id_init(&callId).ThrowOnError();
            return callId;
        }

        public string Number
        {
            get => Marshal.PtrToStringAnsi(_native->number);
            set
            {
                Marshal.FreeHGlobal(_native->number);
                _native->number = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public string Host
        {
            get => Marshal.PtrToStringAnsi(_native->host);
            set
            {
                Marshal.FreeHGlobal(_native->host);
                _native->host = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public static CallIdHeader Parse(string str)
        {
            var callId = new CallIdHeader();

            var strPtr = Marshal.StringToHGlobalAnsi(str);
            NativeMethods.osip_call_id_parse(callId._native, strPtr).ThrowOnError();
            Marshal.FreeHGlobal(strPtr);

            return callId;
        }

        internal osip_call_id_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        public CallIdHeader DeepClone()
        {
            osip_call_id_t* callId;
            NativeMethods.osip_call_id_clone(_native, &callId).ThrowOnError();
            return new CallIdHeader(callId, true);
        }

        public override string ToString()
        {
            IntPtr ptr;
            NativeMethods.osip_call_id_to_str(_native, &ptr).ThrowOnError();

            string str = Marshal.PtrToStringAnsi(ptr);
            NativeMethods.osip_free(ptr.ToPointer());

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_call_id_free(_native);
            _native = osip_call_id_t.Null;
        }
    }
}