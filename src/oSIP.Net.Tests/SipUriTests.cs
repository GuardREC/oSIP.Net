using System.Linq;
using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class SipUriTests
    {
        [Test]
        public void Shall_stringify_header()
        {
            var uri = new SipUri
            {
                Host = "1.2.3.4",
                Port = "1234",
                Username = "admin",
                Password = "pwd",
                Parameters =
                {
                    new SipUriParameter("param", "foo")
                },
                Headers =
                {
                    new SipUriParameter("header", "bar")
                }
            };

            Assert.That(
                uri.ToString(),
                Is.EqualTo("sip:admin:pwd@1.2.3.4:1234;param=foo?header=bar"));
        }

        [Test]
        public void Shall_parse_uri()
        {
            Assert.That(SipUri.TryParse("sip:admin:pwd@1.2.3.4:1234;param=foo?header=bar", out SipUri uri), Is.True);
            Assert.That(uri.Scheme, Is.EqualTo("sip"));
            Assert.That(uri.Host, Is.EqualTo("1.2.3.4"));
            Assert.That(uri.Port, Is.EqualTo("1234"));
            Assert.That(uri.Username, Is.EqualTo("admin"));
            Assert.That(uri.Password, Is.EqualTo("pwd"));
            Assert.That(uri.Parameters.Single().Name, Is.EqualTo("param"));
            Assert.That(uri.Parameters.Single().Value, Is.EqualTo("foo"));
            Assert.That(uri.Headers.Single().Name, Is.EqualTo("header"));
            Assert.That(uri.Headers.Single().Value, Is.EqualTo("bar"));
        }
    }
}