using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class CSeqHeader : OwnershipDisposable
    {
        private osip_cseq_t* _native;

        public CSeqHeader() : this(Create(), true)
        {
        }

        internal CSeqHeader(osip_cseq_t* native, bool isOwner) : base(isOwner)
        {
            _native = native;
        }

        private static osip_cseq_t* Create()
        {
            osip_cseq_t* native;
            NativeMethods.osip_cseq_init(&native);
            return native;
        }

        public string Method
        {
            get => Marshal.PtrToStringAnsi(_native->method);
            set
            {
                Marshal.FreeHGlobal(_native->method);
                _native->method = Marshal.StringToHGlobalAnsi(value);
            }
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

        public static CSeqHeader Parse(string str)
        {
            var cseq = new CSeqHeader();

            var strPtr = Marshal.StringToHGlobalAnsi(str);
            NativeMethods.osip_cseq_parse(cseq._native, strPtr);
            Marshal.FreeHGlobal(strPtr);

            return cseq;
        }

        internal osip_cseq_t* TakeOwnership()
        {
            ReleaseOwnership();
            return _native;
        }

        public CSeqHeader DeepClone()
        {
            osip_cseq_t* cseq;
            NativeMethods.osip_cseq_clone(_native, &cseq);
            return new CSeqHeader(cseq, true);
        }

        public override string ToString()
        {
            IntPtr ptr;
            NativeMethods.osip_cseq_to_str(_native, &ptr);

            string str = Marshal.PtrToStringAnsi(ptr);
            NativeMethods.osip_free(ptr.ToPointer());

            return str;
        }

        protected override void OnDispose()
        {
            NativeMethods.osip_cseq_free(_native);
            _native = osip_cseq_t.Null;
        }
    }
}