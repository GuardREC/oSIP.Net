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
        public void Shall_not_return_cached_stringified_response()
        {
            using (var response = new SipResponse())
            {
                response.Version = "SIP/2.0";
                response.StatusCode = 200;
                response.ReasonPhrase = "OK";

                string before = response.ToString();

                response.StatusCode = 201;

                string after = response.ToString();

                Assert.That(before, Is.Not.EqualTo(after));
            }
        }

        [Test]
        public void Shall_parse_response_string()
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

        [Test]
        public void Shall_parse_response_bytes()
        {
            const string str =
                "SIP/2.0 200 OK\r\nContent-Length: 0\r\n" +
                "\r\n";
            using (var response = (SipResponse)SipMessage.Parse(Encoding.UTF8.GetBytes(str)))
            {
                Assert.That(response.Version, Is.EqualTo("SIP/2.0"));
                Assert.That(response.StatusCode, Is.EqualTo(200));
                Assert.That(response.ReasonPhrase, Is.EqualTo("OK"));
            }
        }

        [Test]
        public void Shall_clone_response()
        {
            const string str =
                "SIP/2.0 200 OK\r\nContent-Length: 0\r\n" +
                "\r\n";
            using (var original = (SipResponse) SipMessage.Parse(str))
            using (var cloned = original.DeepClone())
            {
                original.StatusCode = 183;
                original.ReasonPhrase = "Session Progress";

                Assert.That(cloned.ToString(), Does.Contain("200 OK"));
                Assert.That(original.ToString(), Does.Contain("183 Session Progress"));
            }
        }
    }
}