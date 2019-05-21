using System;
using System.Text;
using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class SipResponseTests
    {
        [Test]
        public void Shall_stringify_response()
        {
            var response = new SipResponse();
            response.Version = "SIP/2.0";
            response.StatusCode = 200;
            response.ReasonPhrase = "OK";

            Assert.That(response.ToString(), Is.EqualTo(
                "SIP/2.0 200 OK\r\nContent-Length: 0\r\n" +
                "\r\n"));
        }

        [Test]
        public void Shall_byteify_response()
        {
            var response = new SipResponse();
            response.Version = "SIP/2.0";
            response.StatusCode = 200;
            response.ReasonPhrase = "OK";

            var buffer = new byte[ushort.MaxValue];
            response.TryCopyTo(buffer, 0, out int length);

            var response2 = (SipResponse)SipMessage.Parse(new ArraySegment<byte>(buffer, 0, length));
            Assert.That(response2.ToString(), Is.EqualTo(
                "SIP/2.0 200 OK\r\nContent-Length: 0\r\n" +
                "\r\n"));
        }

        [Test]
        public void Shall_not_return_cached_stringified_response()
        {
            var response = new SipResponse();
            response.Version = "SIP/2.0";
            response.StatusCode = 200;
            response.ReasonPhrase = "OK";

            string before = response.ToString();

            response.StatusCode = 201;

            string after = response.ToString();

            Assert.That(before, Is.Not.EqualTo(after));
        }

        [Test]
        public void Shall_parse_response_string()
        {
            const string str =
                "SIP/2.0 200 OK\r\nContent-Length: 0\r\n" +
                "\r\n";
            var response = (SipResponse)SipMessage.Parse(str);
            Assert.That(response.Version, Is.EqualTo("SIP/2.0"));
            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.ReasonPhrase, Is.EqualTo("OK"));
        }

        [Test]
        public void Shall_parse_response_bytes()
        {
            const string str =
                "SIP/2.0 200 OK\r\nContent-Length: 0\r\n" +
                "\r\n";
            var response = (SipResponse)SipMessage.Parse(Encoding.UTF8.GetBytes(str));
            Assert.That(response.Version, Is.EqualTo("SIP/2.0"));
            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.ReasonPhrase, Is.EqualTo("OK"));
        }

        [Test]
        public void Shall_clone_response()
        {
            const string str =
                "SIP/2.0 200 OK\r\nContent-Length: 0\r\n" +
                "\r\n";
            var original = (SipResponse)SipMessage.Parse(str);
            var cloned = original.DeepClone();
            original.StatusCode = 183;
            original.ReasonPhrase = "Session Progress";

            Assert.That(cloned.ToString(), Does.Contain("200 OK"));
            Assert.That(original.ToString(), Does.Contain("183 Session Progress"));
        }
    }
}