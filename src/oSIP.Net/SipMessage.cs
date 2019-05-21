using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public abstract unsafe class SipMessage
    {
        static SipMessage()
        {
            Parser.InitializeIfNecessary();
        }

        protected SipMessage()
        {
            Vias = new List<ViaHeader>();
            RecordRoutes = new List<NameAddressHeader>();
            Routes = new List<NameAddressHeader>();
            Contacts = new List<NameAddressHeader>();
            Authorizations = new List<AuthorizationHeader>();
            WwwAuthenticates = new List<WwwAuthenticateHeader>();
            ProxyAuthenticates = new List<WwwAuthenticateHeader>();
            ProxyAuthorizations = new List<AuthorizationHeader>();
            CallInfos = new List<CallInfoHeader>();
            Allows = new List<ContentLengthHeader>();
            ContentEncodings = new List<ContentLengthHeader>();
            AlertInfos = new List<CallInfoHeader>();
            ErrorInfos = new List<CallInfoHeader>();
            Accepts = new List<ContentTypeHeader>();
            AcceptEncodings = new List<AcceptEncodingHeader>();
            AcceptLanguages = new List<AcceptEncodingHeader>();
            AuthenticationInfos = new List<AuthenticationInfoHeader>();
            ProxyAuthenticationInfos = new List<AuthenticationInfoHeader>();
            OtherHeaders = new List<GenericHeader>();
            Bodies = new List<SipBody>();
        }

        private static T FromNative<T>(osip_message_t* native) where T : SipMessage, new ()
        {
            var message = new T
            {
                Version = Marshal.PtrToStringAnsi(native->sip_version),
                From = native->from != osip_from_t.Null
                    ? NameAddressHeader.FromNative(native->from)
                    : null,
                To = native->to != osip_from_t.Null
                    ? NameAddressHeader.FromNative(native->to)
                    : null,
                CallId = native->call_id != osip_call_id_t.Null
                    ? CallIdHeader.FromNative(native->call_id)
                    : null,
                CSeq = native->cseq != osip_cseq_t.Null
                    ? CSeqHeader.FromNative(native->cseq)
                    : null,
                ContentType = native->content_type != osip_content_type_t.Null
                    ? ContentTypeHeader.FromNative(native->content_type)
                    : null,
                MimeVersion = native->mime_version != osip_content_length_t.Null
                    ? ContentLengthHeader.FromNative(native->mime_version)
                    : null
            };

            if(message is SipRequest request)
            {
                request.Method = Marshal.PtrToStringAnsi(native->sip_method);
                request.RequestUri = native->req_uri != osip_uri_t.Null
                    ? SipUri.FromNative(native->req_uri)
                    : null;
            }
            else if(message is SipResponse response)
            {
                response.StatusCode = native->status_code;
                response.ReasonPhrase = Marshal.PtrToStringAnsi(native->reason_phrase);
            }

            int size = NativeMethods.osip_list_size(&native->vias);
            for (int i = 0; i < size; i++)
            {
                osip_via_t* header = (osip_via_t*)NativeMethods.osip_list_get(&native->vias, i);
                message.Vias.Add(ViaHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->record_routes);
            for (int i = 0; i < size; i++)
            {
                osip_from_t* header = (osip_from_t*)NativeMethods.osip_list_get(&native->record_routes, i);
                message.RecordRoutes.Add(NameAddressHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->routes);
            for (int i = 0; i < size; i++)
            {
                osip_from_t* header = (osip_from_t*)NativeMethods.osip_list_get(&native->routes, i);
                message.Routes.Add(NameAddressHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->contacts);
            for (int i = 0; i < size; i++)
            {
                osip_from_t* header = (osip_from_t*)NativeMethods.osip_list_get(&native->contacts, i);
                message.Contacts.Add(NameAddressHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->authorizations);
            for (int i = 0; i < size; i++)
            {
                osip_authorization_t* header = (osip_authorization_t*)NativeMethods.osip_list_get(&native->authorizations, i);
                message.Authorizations.Add(AuthorizationHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->www_authenticates);
            for (int i = 0; i < size; i++)
            {
                osip_www_authenticate_t* header = (osip_www_authenticate_t*)NativeMethods.osip_list_get(&native->www_authenticates, i);
                message.WwwAuthenticates.Add(WwwAuthenticateHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->proxy_authenticates);
            for (int i = 0; i < size; i++)
            {
                osip_www_authenticate_t* header = (osip_www_authenticate_t*)NativeMethods.osip_list_get(&native->proxy_authenticates, i);
                message.ProxyAuthenticates.Add(WwwAuthenticateHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->proxy_authorizations);
            for (int i = 0; i < size; i++)
            {
                osip_authorization_t* header = (osip_authorization_t*)NativeMethods.osip_list_get(&native->proxy_authorizations, i);
                message.ProxyAuthorizations.Add(AuthorizationHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->call_infos);
            for (int i = 0; i < size; i++)
            {
                osip_call_info_t* header = (osip_call_info_t*)NativeMethods.osip_list_get(&native->call_infos, i);
                message.CallInfos.Add(CallInfoHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->allows);
            for (int i = 0; i < size; i++)
            {
                osip_content_length_t* header = (osip_content_length_t*)NativeMethods.osip_list_get(&native->allows, i);
                message.Allows.Add(ContentLengthHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->content_encodings);
            for (int i = 0; i < size; i++)
            {
                osip_content_length_t* header = (osip_content_length_t*)NativeMethods.osip_list_get(&native->content_encodings, i);
                message.ContentEncodings.Add(ContentLengthHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->alert_infos);
            for (int i = 0; i < size; i++)
            {
                osip_call_info_t* header = (osip_call_info_t*)NativeMethods.osip_list_get(&native->alert_infos, i);
                message.AlertInfos.Add(CallInfoHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->error_infos);
            for (int i = 0; i < size; i++)
            {
                osip_call_info_t* header = (osip_call_info_t*)NativeMethods.osip_list_get(&native->error_infos, i);
                message.ErrorInfos.Add(CallInfoHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->accepts);
            for (int i = 0; i < size; i++)
            {
                osip_content_type_t* header = (osip_content_type_t*)NativeMethods.osip_list_get(&native->accepts, i);
                message.Accepts.Add(ContentTypeHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->accept_encodings);
            for (int i = 0; i < size; i++)
            {
                osip_accept_encoding_t* header = (osip_accept_encoding_t*)NativeMethods.osip_list_get(&native->accept_encodings, i);
                message.AcceptEncodings.Add(AcceptEncodingHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->accept_languages);
            for (int i = 0; i < size; i++)
            {
                osip_accept_encoding_t* header = (osip_accept_encoding_t*)NativeMethods.osip_list_get(&native->accept_languages, i);
                message.AcceptLanguages.Add(AcceptEncodingHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->authentication_infos);
            for (int i = 0; i < size; i++)
            {
                osip_authentication_info_t* header = (osip_authentication_info_t*)NativeMethods.osip_list_get(&native->authentication_infos, i);
                message.AuthenticationInfos.Add(AuthenticationInfoHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->proxy_authentication_infos);
            for (int i = 0; i < size; i++)
            {
                osip_authentication_info_t* header = (osip_authentication_info_t*)NativeMethods.osip_list_get(&native->proxy_authentication_infos, i);
                message.ProxyAuthenticationInfos.Add(AuthenticationInfoHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->headers);
            for (int i = 0; i < size; i++)
            {
                osip_header_t* header = (osip_header_t*)NativeMethods.osip_list_get(&native->headers, i);
                message.OtherHeaders.Add(GenericHeader.FromNative(header));
            }

            size = NativeMethods.osip_list_size(&native->bodies);
            for (int i = 0; i < size; i++)
            {
                osip_body_t* header = (osip_body_t*)NativeMethods.osip_list_get(&native->bodies, i);
                message.Bodies.Add(SipBody.FromNative(header));
            }

            return message;
        }

        private osip_message_t* ToNative()
        {
            osip_message_t* native;
            NativeMethods.osip_message_init(&native).ThrowOnError();

            native->sip_version = Marshal.StringToHGlobalAnsi(Version);
            native->from = From != null
                    ? From.ToNative()
                    : osip_from_t.Null;
            native->to = To != null
                    ? To.ToNative()
                    : osip_from_t.Null;
            native->call_id = CallId != null
                    ? CallId.ToNative()
                    : osip_call_id_t.Null;
            native->cseq = CSeq != null
                    ? CSeq.ToNative()
                    : osip_cseq_t.Null;
            native->content_type = ContentType != null
                    ? ContentType.ToNative()
                    : osip_content_type_t.Null;
            native->mime_version = MimeVersion != null
                    ? MimeVersion.ToNative()
                    : osip_content_length_t.Null;

            if(this is SipRequest request)
            {
                native->sip_method = native->sip_method = Marshal.StringToHGlobalAnsi(request.Method);
                native->req_uri = request.RequestUri != null
                    ? request.RequestUri.ToNative()
                    : osip_uri_t.Null;
            }
            else if(this is SipResponse response)
            {
                native->status_code = response.StatusCode;
                native->reason_phrase = Marshal.StringToHGlobalAnsi(response.ReasonPhrase);
            }

            for (int i = 0; i < Vias.Count; i++)
            {
                osip_via_t* header = Vias[i].ToNative();
                NativeMethods.osip_list_add(&native->vias, header, i).ThrowOnError();
            }

            for (int i = 0; i < RecordRoutes.Count; i++)
            {
                osip_from_t* header = RecordRoutes[i].ToNative();
                NativeMethods.osip_list_add(&native->record_routes, header, i).ThrowOnError();
            }

            for (int i = 0; i < Routes.Count; i++)
            {
                osip_from_t* header = Routes[i].ToNative();
                NativeMethods.osip_list_add(&native->routes, header, i).ThrowOnError();
            }

            for (int i = 0; i < Contacts.Count; i++)
            {
                osip_from_t* header = Contacts[i].ToNative();
                NativeMethods.osip_list_add(&native->contacts, header, i).ThrowOnError();
            }

            for (int i = 0; i < Authorizations.Count; i++)
            {
                osip_authorization_t* header = Authorizations[i].ToNative();
                NativeMethods.osip_list_add(&native->authorizations, header, i).ThrowOnError();
            }

            for (int i = 0; i < WwwAuthenticates.Count; i++)
            {
                osip_www_authenticate_t* header = WwwAuthenticates[i].ToNative();
                NativeMethods.osip_list_add(&native->www_authenticates, header, i).ThrowOnError();
            }

            for (int i = 0; i < ProxyAuthenticates.Count; i++)
            {
                osip_www_authenticate_t* header = ProxyAuthenticates[i].ToNative();
                NativeMethods.osip_list_add(&native->proxy_authenticates, header, i).ThrowOnError();
            }

            for (int i = 0; i < ProxyAuthorizations.Count; i++)
            {
                osip_authorization_t* header = ProxyAuthorizations[i].ToNative();
                NativeMethods.osip_list_add(&native->proxy_authorizations, header, i).ThrowOnError();
            }

            for (int i = 0; i < CallInfos.Count; i++)
            {
                osip_call_info_t* header = CallInfos[i].ToNative();
                NativeMethods.osip_list_add(&native->call_infos, header, i).ThrowOnError();
            }

            for (int i = 0; i < Allows.Count; i++)
            {
                osip_content_length_t* header = Allows[i].ToNative();
                NativeMethods.osip_list_add(&native->allows, header, i).ThrowOnError();
            }

            for (int i = 0; i < ContentEncodings.Count; i++)
            {
                osip_content_length_t* header = ContentEncodings[i].ToNative();
                NativeMethods.osip_list_add(&native->content_encodings, header, i).ThrowOnError();
            }

            for (int i = 0; i < AlertInfos.Count; i++)
            {
                osip_call_info_t* header = AlertInfos[i].ToNative();
                NativeMethods.osip_list_add(&native->alert_infos, header, i).ThrowOnError();
            }

            for (int i = 0; i < ErrorInfos.Count; i++)
            {
                osip_call_info_t* header = ErrorInfos[i].ToNative();
                NativeMethods.osip_list_add(&native->error_infos, header, i).ThrowOnError();
            }

            for (int i = 0; i < Accepts.Count; i++)
            {
                osip_content_type_t* header = Accepts[i].ToNative();
                NativeMethods.osip_list_add(&native->accepts, header, i).ThrowOnError();
            }

            for (int i = 0; i < AcceptEncodings.Count; i++)
            {
                osip_accept_encoding_t* header = AcceptEncodings[i].ToNative();
                NativeMethods.osip_list_add(&native->accept_encodings, header, i).ThrowOnError();
            }

            for (int i = 0; i < AcceptLanguages.Count; i++)
            {
                osip_accept_encoding_t* header = AcceptLanguages[i].ToNative();
                NativeMethods.osip_list_add(&native->accept_languages, header, i).ThrowOnError();
            }

            for (int i = 0; i < AuthenticationInfos.Count; i++)
            {
                osip_authentication_info_t* header = AuthenticationInfos[i].ToNative();
                NativeMethods.osip_list_add(&native->authentication_infos, header, i).ThrowOnError();
            }

            for (int i = 0; i < ProxyAuthenticationInfos.Count; i++)
            {
                osip_authentication_info_t* header = ProxyAuthenticationInfos[i].ToNative();
                NativeMethods.osip_list_add(&native->proxy_authentication_infos, header, i).ThrowOnError();
            }

            for (int i = 0; i < OtherHeaders.Count; i++)
            {
                osip_header_t* header = OtherHeaders[i].ToNative();
                NativeMethods.osip_list_add(&native->headers, header, i).ThrowOnError();
            }

            for (int i = 0; i < Bodies.Count; i++)
            {
                osip_body_t* header = Bodies[i].ToNative();
                NativeMethods.osip_list_add(&native->bodies, header, i).ThrowOnError();
            }

            return native;
        }

        public string Version { get; set; }

        public List<ViaHeader> Vias { get; }

        public List<NameAddressHeader> RecordRoutes { get; }

        public List<NameAddressHeader> Routes { get; }

        public NameAddressHeader From { get; set; }

        public NameAddressHeader To { get; set; }

        public CallIdHeader CallId { get; set; }

        public CSeqHeader CSeq { get; set; }

        public List<NameAddressHeader> Contacts { get; }

        public List<AuthorizationHeader> Authorizations { get; }

        public List<WwwAuthenticateHeader> WwwAuthenticates { get; }

        public List<WwwAuthenticateHeader> ProxyAuthenticates { get; }

        public List<AuthorizationHeader> ProxyAuthorizations { get; }

        public List<CallInfoHeader> CallInfos { get; }

        public ContentTypeHeader ContentType { get; set; }

        public ContentLengthHeader MimeVersion { get; set; }

        public List<ContentLengthHeader> Allows { get; }

        public List<ContentLengthHeader> ContentEncodings { get; }

        public List<CallInfoHeader> AlertInfos { get; }

        public List<CallInfoHeader> ErrorInfos { get; }

        public List<ContentTypeHeader> Accepts { get; }

        public List<AcceptEncodingHeader> AcceptEncodings { get; }

        public List<AcceptEncodingHeader> AcceptLanguages { get; }

        public List<AuthenticationInfoHeader> AuthenticationInfos { get; }

        public List<AuthenticationInfoHeader> ProxyAuthenticationInfos { get; }

        public List<GenericHeader> OtherHeaders { get; }

        public List<SipBody> Bodies { get; }

        public ContentLengthHeader ContentLength
        {
            get
            {
                IntPtr ptr = IntPtr.Zero;
                osip_message_t* native = ToNative();
                osip_message_t* native2 = osip_message_t.Null;

                try
                {
                    int length;
                    NativeMethods.osip_message_to_str(native, &ptr, &length).ThrowOnError();

                    ErrorCode errorCode = NativeMethods.osip_message_init(&native2);
                    if (!errorCode.EnsureSuccess())
                    {
                        return null;
                    }

                    errorCode = NativeMethods.osip_message_parse(native2, ptr, (ulong) length);
                    if (!errorCode.EnsureSuccess())
                    {
                        return null;
                    }

                    return native2->content_length != osip_content_length_t.Null
                        ? ContentLengthHeader.FromNative(native2->content_length)
                        : null;
                }
                finally
                {
                    NativeMethods.osip_message_free(native);
                    NativeMethods.osip_message_free(native2);
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

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
            TryParseCore(ptr, length, out SipMessage message).ThrowOnError();
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
            return TryParseCore(ptr, length, out message).EnsureSuccess();
        }

        private static ErrorCode TryParseCore(IntPtr ptr, ulong length, out SipMessage message)
        {
            osip_message_t* native = null;

            try
            {
                ErrorCode errorCode = NativeMethods.osip_message_init(&native);
                if (!errorCode.EnsureSuccess())
                {
                    message = null;
                    return errorCode;
                }

                errorCode = NativeMethods.osip_message_parse(native, ptr, length);
                if (!errorCode.EnsureSuccess())
                {
                    message = null;
                    return errorCode;
                }

                message = native->status_code == 0
                    ? FromNative<SipRequest>(native)
                    : (SipMessage)FromNative<SipResponse>(native);
                return errorCode;
            }
            finally
            {
                NativeMethods.osip_message_free(native);
            }
        }

        public T DeepClone<T>() where T : SipMessage, new ()
        {
            osip_message_t* native = ToNative();

            try
            {
                return FromNative<T>(native);
            }
            finally
            {
                NativeMethods.osip_message_free(native);
            }
        }

        public override string ToString()
        {
            IntPtr ptr = IntPtr.Zero;
            osip_message_t* native = ToNative();

            try
            {
                int length;
                NativeMethods.osip_message_to_str(native, &ptr, &length).ThrowOnError();
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                NativeMethods.osip_message_free(native);
                Marshal.FreeHGlobal(ptr);
            }
        }

        public bool TryCopyTo(byte[] buffer, int offset, out int count)
        {
            IntPtr ptr = IntPtr.Zero;
            osip_message_t* native = ToNative();

            try
            {
                int length;
                if (NativeMethods.osip_message_to_str(native, &ptr, &length) < 0 ||
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
                NativeMethods.osip_message_free(native);
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}