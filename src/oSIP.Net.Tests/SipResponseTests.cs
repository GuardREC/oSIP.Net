using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class SipResponseTests
    {
        [Test]
        public void Shall_stringify_header()
        {
            using (var response = new SipResponse())
            {
                response.Version = "SIP/2.0";
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";

                Assert.That(response.ToString(), Is.EqualTo(
                    "SIP/2.0 200 OK\r\nContent-Length: 0\r\n" +
                    "\r\n"));
            }
        }

        [Test]
        public void Shall_parse_message()
        {
            const string str =
                "SIP/2.0 200 OK\r\nContent-Length: 0\r\n" +
                "\r\n";
            using (var response = (SipResponse) SipMessage.Parse(str))
            {
                Assert.That(response.Version, Is.EqualTo("SIP/2.0"));
                Assert.That(response.StatusCode, Is.EqualTo(200));
                Assert.That(response.ReasonPhrase, Is.EqualTo("OK"));
            }
        }
    }
}