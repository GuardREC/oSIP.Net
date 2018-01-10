using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class SipRequestTests
    {
        [Test]
        public void Shall_stringify_request()
        {
            using (var request = new SipRequest())
            {
                request.Version = "SIP/2.0";
                request.Method = "INVITE";
                request.Vias.Add(ViaHeader.Parse("SIP/2.0/UDP foo.bar.com"));
                request.RecordRoutes.Add(RecordRouteHeader.Parse("Tommy Atkins <sip:tommy.atkins@abc.com>"));
                request.Routes.Add(RouteHeader.Parse("John Doe <sip:john.doe@abc.com>"));
                request.RequestUri = SipUri.Parse("sip:john.smith@abc.com");
                request.From = FromHeader.Parse("John Smith <sip:john.smith@abc.com>");
                request.To = ToHeader.Parse("Joe Shmoe <sip:joe.shmoe@abc.com>");
                request.CallId = CallIdHeader.Parse("1@foo.bar.com");
                request.CSeq = CSeqHeader.Parse("1 INVITE");
                request.Contacts.Add(ContactHeader.Parse("Prisoner X <sip:prisoner.x@abc.com>"));
                request.Authorizations.Add(AuthorizationHeader.Parse("Digest username=\"Alice\""));
                request.WwwAuthenticates.Add(WwwAuthenticateHeader.Parse("Digest realm=\"abc.com\""));
                request.ProxyAuthenticates.Add(ProxyAuthenticateHeader.Parse("Digest realm=\"xyz.com\""));
                request.ProxyAuthorizations.Add(ProxyAuthorizationHeader.Parse("Digest username=\"Bob\""));
                request.CallInfos.Add(CallInfoHeader.Parse("<http://www.abc.com/photo.png>;purpose=icon"));
                request.ContentType = ContentTypeHeader.Parse("text/plain");
                request.MimeVersion = MimeVersionHeader.Parse("1.0");
                request.Allows.Add(AllowHeader.Parse("INVITE, ACK, BYE"));
                request.ContentEncodings.Add(ContentEncodingHeader.Parse("deflate"));
                request.AlertInfos.Add(AlertInfoHeader.Parse("<http://www.abc.com/sound.wav>"));
                request.ErrorInfos.Add(ErrorInfoHeader.Parse("<sip:not-in-service@abc.com>"));
                request.Accepts.Add(AcceptHeader.Parse("application/sdp"));
                request.AcceptEncodings.Add(AcceptEncodingHeader.Parse("gzip"));
                request.AcceptLanguages.Add(AcceptLanguageHeader.Parse("en"));
                request.AuthenticationInfos.Add(AuthenticationInfoHeader.Parse("nextnonce=\"abc\""));
                request.ProxyAuthenticationInfos.Add(ProxyAuthenticationInfoHeader.Parse("nextnonce=\"def\""));
                request.OtherHeaders.Add(new GenericHeader("P-Asserted-Identity", "sip:alan.smithee@abc.com"));
                request.Bodies.Add(SipBody.Parse("Hello world!"));

                Assert.That(request.ToString(), Is.EqualTo(
                    "INVITE sip:john.smith@abc.com SIP/2.0\r\n" +
                    "Via: SIP/2.0/UDP foo.bar.com\r\n" +
                    "Record-Route: Tommy Atkins <sip:tommy.atkins@abc.com>\r\n" +
                    "Route: John Doe <sip:john.doe@abc.com>\r\n" +
                    "From: John Smith <sip:john.smith@abc.com>\r\n" +
                    "To: Joe Shmoe <sip:joe.shmoe@abc.com>\r\n" +
                    "Call-ID: 1@foo.bar.com\r\n" +
                    "CSeq: 1 INVITE\r\n" +
                    "Contact: Prisoner X <sip:prisoner.x@abc.com>\r\n" +
                    "Authorization: Digest username=\"Alice\"\r\n" +
                    "WWW-Authenticate: Digest realm=\"abc.com\"\r\n" +
                    "Proxy-Authenticate: Digest realm=\"xyz.com\"\r\n" +
                    "Proxy-Authorization: Digest username=\"Bob\"\r\n" +
                    "Call-Info: <http://www.abc.com/photo.png>;purpose=icon\r\n" +
                    "Content-Type: text/plain\r\n" +
                    "Mime-Version: 1.0\r\n" +
                    "Allow: INVITE, ACK, BYE\r\n" +
                    "Content-Encoding: deflate\r\n" +
                    "Alert-Info: <http://www.abc.com/sound.wav>\r\n" +
                    "Error-Info: <sip:not-in-service@abc.com>\r\n" +
                    "Accept: application/sdp\r\n" +
                    "Accept-Encoding: gzip\r\n" +
                    "Accept-Language: en\r\n" +
                    "Authentication-Info: nextnonce=\"abc\"\r\n" +
                    "Proxy-Authentication-Info: nextnonce=\"def\"\r\n" +
                    "P-Asserted-Identity: sip:alan.smithee@abc.com\r\n" +
                    "Content-Length:    12\r\n" +
                    "\r\n" +
                    "Hello world!"));
            }
        }

        [Test]
        public void Shall_parse_request()
        {
            const string str =
                "INVITE sip:john.smith@abc.com SIP/2.0\r\n" +
                "Via: SIP/2.0/UDP foo.bar.com\r\n" +
                "Record-Route: Tommy Atkins <sip:tommy.atkins@abc.com>\r\n" +
                "Route: John Doe <sip:john.doe@abc.com>\r\n" +
                "From: John Smith <sip:john.smith@abc.com>\r\n" +
                "To: Joe Shmoe <sip:joe.shmoe@abc.com>\r\n" +
                "Call-ID: 1@foo.bar.com\r\n" +
                "CSeq: 1 INVITE\r\n" +
                "Contact: Prisoner X <sip:prisoner.x@abc.com>\r\n" +
                "Authorization: Digest username=\"Alice\"\r\n" +
                "WWW-Authenticate: Digest realm=\"abc.com\"\r\n" +
                "Proxy-Authenticate: Digest realm=\"xyz.com\"\r\n" +
                "Proxy-Authorization: Digest username=\"Bob\"\r\n" +
                "Call-Info: <http://www.abc.com/photo.png>;purpose=icon\r\n" +
                "Content-Type: text/plain\r\n" +
                "Mime-Version: 1.0\r\n" +
                "Allow: INVITE, BYE\r\n" +
                "Content-Encoding: deflate\r\n" +
                "Alert-Info: <http://www.abc.com/sound.wav>\r\n" +
                "Error-Info: <sip:not-in-service@abc.com>\r\n" +
                "Accept: application/sdp\r\n" +
                "Accept-Encoding: gzip\r\n" +
                "Accept-Language: en\r\n" +
                "Authentication-Info: nextnonce=\"abc\"\r\n" +
                "Proxy-Authentication-Info: nextnonce=\"def\"\r\n" +
                "P-Asserted-Identity: sip:alan.smithee@abc.com\r\n" +
                "Content-Length:    12\r\n" +
                "\r\n" +
                "Hello world!";
            using (var request = (SipRequest) SipMessage.Parse(str))
            {
                Assert.That(request.Version, Is.EqualTo("SIP/2.0"));
                Assert.That(request.Method, Is.EqualTo("INVITE"));
                Assert.That(request.RequestUri.ToString(), Is.EqualTo("sip:john.smith@abc.com"));
                Assert.That(request.Vias[0].ToString(), Is.EqualTo("SIP/2.0/UDP foo.bar.com"));
                Assert.That(request.RecordRoutes[0].ToString(), Is.EqualTo("Tommy Atkins <sip:tommy.atkins@abc.com>"));
                Assert.That(request.Routes[0].ToString(), Is.EqualTo("John Doe <sip:john.doe@abc.com>"));
                Assert.That(request.From.ToString(), Is.EqualTo("John Smith <sip:john.smith@abc.com>"));
                Assert.That(request.To.ToString(), Is.EqualTo("Joe Shmoe <sip:joe.shmoe@abc.com>"));
                Assert.That(request.CallId.ToString(), Is.EqualTo("1@foo.bar.com"));
                Assert.That(request.CSeq.ToString(), Is.EqualTo("1 INVITE"));
                Assert.That(request.Contacts[0].ToString(), Is.EqualTo("Prisoner X <sip:prisoner.x@abc.com>"));
                Assert.That(request.Authorizations[0].ToString(), Is.EqualTo("Digest username=\"Alice\""));
                Assert.That(request.WwwAuthenticates[0].ToString(), Is.EqualTo("Digest realm=\"abc.com\""));
                Assert.That(request.ProxyAuthenticates[0].ToString(), Is.EqualTo("Digest realm=\"xyz.com\""));
                Assert.That(request.ProxyAuthorizations[0].ToString(), Is.EqualTo("Digest username=\"Bob\""));
                Assert.That(request.CallInfos[0].ToString(), Is.EqualTo("<http://www.abc.com/photo.png>;purpose=icon"));
                Assert.That(request.ContentType.ToString(), Is.EqualTo("text/plain"));
                Assert.That(request.MimeVersion.ToString(), Is.EqualTo("1.0"));
                Assert.That(request.Allows.Size, Is.EqualTo(2));
                Assert.That(request.Allows[0].ToString(), Is.EqualTo("INVITE"));
                Assert.That(request.Allows[1].ToString(), Is.EqualTo("BYE"));
                Assert.That(request.ContentEncodings[0].ToString(), Is.EqualTo("deflate"));
                Assert.That(request.AlertInfos[0].ToString(), Is.EqualTo("<http://www.abc.com/sound.wav>"));
                Assert.That(request.ErrorInfos[0].ToString(), Is.EqualTo("<sip:not-in-service@abc.com>"));
                Assert.That(request.Accepts[0].ToString(), Is.EqualTo("application/sdp"));
                Assert.That(request.AcceptEncodings[0].ToString(), Is.EqualTo("gzip"));
                Assert.That(request.AcceptLanguages[0].ToString(), Is.EqualTo("en"));
                Assert.That(request.AuthenticationInfos[0].ToString(), Is.EqualTo("nextnonce=\"abc\""));
                Assert.That(request.ProxyAuthenticationInfos[0].ToString(), Is.EqualTo("nextnonce=\"def\""));
                Assert.That(request.OtherHeaders[0].Name, Is.EqualTo("p-asserted-identity"));
                Assert.That(request.OtherHeaders[0].Value, Is.EqualTo("sip:alan.smithee@abc.com"));
                Assert.That(request.ContentLength.ToString(), Is.EqualTo("12"));
                Assert.That(request.Bodies[0].ToString(), Is.EqualTo("Hello world!"));
            }
        }

        [Test]
        public void Shall_clone_request()
        {
            const string str =
                "INVITE sip:john.smith@abc.com SIP/2.0\r\n" +
                "From: John Smith <sip:john.smith@abc.com>\r\n" +
                "To: Joe Shmoe <sip:joe.shmoe@abc.com>\r\n\r\n";
            using (var original = (SipRequest) SipMessage.Parse(str))
            using (var cloned = original.DeepClone())
            {
                original.Method = "REGISTER";

                Assert.That(cloned.ToString(), Does.Contain("INVITE"));
                Assert.That(original.Method, Does.Contain("REGISTER"));
            }
        }
    }
}