using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public abstract unsafe class SipMessage : IDisposable
    {
        static SipMessage()
        {
            Parser.InitializeIfNecessary();
        }

        protected SipMessage() : this(Create())
        {
        }

        internal SipMessage(osip_message_t* native)
        {
            Native = native;
            Vias = new LinkedList<ViaHeader>(
                &Native->vias,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new ViaHeader((osip_via_t*) ptr, false));
            RecordRoutes = new LinkedList<RecordRouteHeader>(
                &Native->record_routes,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new RecordRouteHeader((osip_from_t*) ptr, false));
            Routes = new LinkedList<RouteHeader>(
                &Native->routes,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new RouteHeader((osip_from_t*) ptr, false));
            Contacts = new LinkedList<ContactHeader>(
                &Native->contacts,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new ContactHeader((osip_from_t*) ptr, false));
            Authorizations = new LinkedList<AuthorizationHeader>(
                &Native->authorizations,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new AuthorizationHeader((osip_authorization_t*) ptr, false));
            WwwAuthenticates = new LinkedList<WwwAuthenticateHeader>(
                &Native->www_authenticates,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new WwwAuthenticateHeader((osip_www_authenticate_t*) ptr, false));
            ProxyAuthenticates = new LinkedList<ProxyAuthenticateHeader>(
                &Native->proxy_authenticates,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new ProxyAuthenticateHeader((osip_www_authenticate_t*) ptr, false));
            ProxyAuthorizations = new LinkedList<ProxyAuthorizationHeader>(
                &Native->proxy_authorizations,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new ProxyAuthorizationHeader((osip_authorization_t*) ptr, false));
            CallInfos = new LinkedList<CallInfoHeader>(
                &Native->call_infos,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new CallInfoHeader((osip_call_info_t*) ptr, false));
            Allows = new LinkedList<AllowHeader>(
                &Native->allows,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new AllowHeader((osip_content_length_t*) ptr, false));
            ContentEncodings = new LinkedList<ContentEncodingHeader>(
                &Native->content_encodings,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new ContentEncodingHeader((osip_content_length_t*) ptr, false));
            AlertInfos = new LinkedList<AlertInfoHeader>(
                &Native->alert_infos,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new AlertInfoHeader((osip_call_info_t*) ptr, false));
            ErrorInfos = new LinkedList<ErrorInfoHeader>(
                &Native->error_infos,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new ErrorInfoHeader((osip_call_info_t*) ptr, false));
            Accepts = new LinkedList<AcceptHeader>(
                &Native->accepts,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new AcceptHeader((osip_content_type_t*) ptr, false));
            AcceptEncodings = new LinkedList<AcceptEncodingHeader>(
                &Native->accept_encodings,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new AcceptEncodingHeader((osip_accept_encoding_t*) ptr, false));
            AcceptLanguages = new LinkedList<AcceptLanguageHeader>(
                &Native->accept_languages,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new AcceptLanguageHeader((osip_accept_encoding_t*) ptr, false));
            AuthenticationInfos = new LinkedList<AuthenticationInfoHeader>(
                &Native->authentication_infos,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new AuthenticationInfoHeader((osip_authentication_info_t*) ptr, false));
            ProxyAuthenticationInfos = new LinkedList<ProxyAuthenticationInfoHeader>(
                &Native->proxy_authentication_infos,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new ProxyAuthenticationInfoHeader((osip_authentication_info_t*) ptr, false));
            OtherHeaders = new LinkedList<GenericHeader>(
                &Native->headers,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new GenericHeader((osip_header_t*) ptr, false));
            Bodies = new LinkedList<SipBody>(
                &Native->bodies,
                parameter => new IntPtr(parameter.TakeOwnership()),
                ptr => new SipBody((osip_body_t*) ptr, false));
        }

        internal osip_message_t* Native { get; private set; }

        public string Version
        {
            get => Marshal.PtrToStringAnsi(Native->sip_version);
            set
            {
                Marshal.FreeHGlobal(Native->sip_version);
                Native->sip_version = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public LinkedList<ViaHeader> Vias { get; }

        public LinkedList<RecordRouteHeader> RecordRoutes { get; }

        public LinkedList<RouteHeader> Routes { get; }

        public FromHeader From
        {
            get => Native->from != osip_from_t.Null
                ? new FromHeader(Native->from, false)
                : null;
            set
            {
                NativeMethods.osip_from_free(Native->from);
                Native->from = value != null
                    ? value.TakeOwnership()
                    : osip_from_t.Null;
            }
        }

        public ToHeader To
        {
            get => Native->to != osip_from_t.Null
                ? new ToHeader(Native->to, false)
                : null;
            set
            {
                NativeMethods.osip_from_free(Native->to);
                Native->to = value != null
                    ? value.TakeOwnership()
                    : osip_from_t.Null;
            }
        }

        public CallIdHeader CallId
        {
            get => Native->call_id != osip_call_id_t.Null
                ? new CallIdHeader(Native->call_id, false)
                : null;
            set
            {
                NativeMethods.osip_call_id_free(Native->call_id);
                Native->call_id = value != null
                    ? value.TakeOwnership()
                    : osip_call_id_t.Null;
            }
        }

        public CSeqHeader CSeq
        {
            get => Native->cseq != osip_cseq_t.Null
                ? new CSeqHeader(Native->cseq, false)
                : null;
            set
            {
                NativeMethods.osip_cseq_free(Native->cseq);
                Native->cseq = value != null
                    ? value.TakeOwnership()
                    : osip_cseq_t.Null;
            }
        }

        public LinkedList<ContactHeader> Contacts { get; }

        public LinkedList<AuthorizationHeader> Authorizations { get; }

        public LinkedList<WwwAuthenticateHeader> WwwAuthenticates { get; }

        public LinkedList<ProxyAuthenticateHeader> ProxyAuthenticates { get; }

        public LinkedList<ProxyAuthorizationHeader> ProxyAuthorizations { get; }

        public LinkedList<CallInfoHeader> CallInfos { get; }

        public ContentTypeHeader ContentType
        {
            get => Native->content_type != osip_content_type_t.Null
                ? new ContentTypeHeader(Native->content_type, false)
                : null;
            set
            {
                NativeMethods.osip_content_type_free(Native->content_type);
                Native->content_type = value != null
                    ? value.TakeOwnership()
                    : osip_content_type_t.Null;
            }
        }

        public MimeVersionHeader MimeVersion
        {
            get => Native->mime_version != osip_content_length_t.Null
                ? new MimeVersionHeader(Native->mime_version, false)
                : null;
            set
            {
                NativeMethods.osip_content_length_free(Native->mime_version);
                Native->mime_version = value != null
                    ? value.TakeOwnership()
                    : osip_content_length_t.Null;
            }
        }

        public LinkedList<AllowHeader> Allows { get; }

        public LinkedList<ContentEncodingHeader> ContentEncodings { get; }

        public LinkedList<AlertInfoHeader> AlertInfos { get; }

        public LinkedList<ErrorInfoHeader> ErrorInfos { get; }

        public LinkedList<AcceptHeader> Accepts { get; }

        public LinkedList<AcceptEncodingHeader> AcceptEncodings { get; }

        public LinkedList<AcceptLanguageHeader> AcceptLanguages { get; }

        public LinkedList<AuthenticationInfoHeader> AuthenticationInfos { get; }

        public LinkedList<ProxyAuthenticationInfoHeader> ProxyAuthenticationInfos { get; }

        public LinkedList<GenericHeader> OtherHeaders { get; }

        public LinkedList<SipBody> Bodies { get; }

        public ContentLengthHeader ContentLength =>
            Native->content_length != osip_content_length_t.Null
                ? new ContentLengthHeader(Native->content_length, false)
                : null;

        public static SipMessage Parse(string str)
        {
            var strPtr = Marshal.StringToHGlobalAnsi(str);
            try
            {
                return Parse(strPtr, (ulong) str.Length);
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
            }
        }

        public static SipMessage Parse(ArraySegment<byte> buffer)
        {
            fixed (byte* ptr = &buffer.Array[buffer.Offset])
            {
                return Parse((IntPtr) ptr, (ulong) buffer.Count);
            }
        }

        private static SipMessage Parse(IntPtr ptr, ulong length)
        {
            TryParseCore(ptr, length, out SipMessage message).ThrowOnError(message);
            return message;
        }

        public static bool TryParse(ArraySegment<byte> buffer, out SipMessage message)
        {
            fixed (byte* ptr = &buffer.Array[buffer.Offset])
            {
                return TryParse((IntPtr)ptr, (ulong)buffer.Count, out message);
            }
        }

        public static bool TryParse(string str, out SipMessage message)
        {
            var strPtr = Marshal.StringToHGlobalAnsi(str);
            try
            {
                return TryParse(strPtr, (ulong) str.Length, out message);
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
            }
        }

        private static bool TryParse(IntPtr ptr, ulong length, out SipMessage message)
        {
            return TryParseCore(ptr, length, out message).EnsureSuccess(ref message);
        }

        private static ErrorCode TryParseCore(IntPtr ptr, ulong length, out SipMessage message)
        {
            osip_message_t* msg = Create();

            ErrorCode result = NativeMethods.osip_message_parse(msg, ptr, length);
            if ((int) result >= 0)
            {
                message = msg->status_code == 0
                    ? (SipMessage)new SipRequest(msg)
                    : new SipResponse(msg);
            }
            else
            {
                NativeMethods.osip_message_free(msg);
                message = null;
            }

            return result;
        }

        private static osip_message_t* Create()
        {
            osip_message_t* message;
            NativeMethods.osip_message_init(&message).ThrowOnError();
            return message;
        }

        protected T DeepClone<T>(Func<IntPtr, T> func) where T : SipMessage
        {
            osip_message_t* native;
            NativeMethods.osip_message_clone(Native, &native).ThrowOnError();
            return func(new IntPtr(native));
        }

        public override string ToString()
        {
            NativeMethods.osip_message_force_update(Native).ThrowOnError();

            IntPtr ptr;
            int length;
            NativeMethods.osip_message_to_str(Native, &ptr, &length).ThrowOnError();

            string str = Marshal.PtrToStringAnsi(ptr);
            Marshal.FreeHGlobal(ptr);

            return str;
        }

        public bool TryCopyTo(byte[] buffer, int offset, out int count)
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                int length;
                if (NativeMethods.osip_message_force_update(Native) < 0 ||
                    NativeMethods.osip_message_to_str(Native, &ptr, &length) < 0 ||
                    buffer.Length - offset < length)
                {
                    count = 0;
                    return false;
                }
            
                Marshal.Copy(ptr, buffer, offset, length);
                count = length;
                return true;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public void Dispose()
        {
            OnDispose();
            GC.SuppressFinalize(this);
        }

        ~SipMessage()
        {
            OnDispose();
        }

        private void OnDispose()
        {
            NativeMethods.osip_message_free(Native);
            Native = osip_message_t.Null;
        }
    }
}