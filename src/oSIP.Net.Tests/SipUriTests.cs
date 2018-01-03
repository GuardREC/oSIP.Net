using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class SipUriTests
    {
        [Test]
        public void Shall_stringify_header()
        {
            using (var uri = new SipUri())
            {
                uri.Scheme = "sip";
                uri.Host = "1.2.3.4";
                uri.Port = "1234";
                uri.Username = "admin";
                uri.Password = "pwd";
                uri.Parameters.Add(new SipUriParameter("param", "foo"));
                uri.Headers.Add(new SipUriParameter("header", "bar"));

                Assert.That(
                    uri.ToString(),
                    Is.EqualTo("sip:admin:pwd@1.2.3.4:1234;param=foo?header=bar"));
            }
        }

        [Test]
        public void Shall_parse_uri()
        {
            using (var uri = SipUri.Parse("sip:admin:pwd@1.2.3.4:1234;param=foo?header=bar"))
            {
                Assert.That(uri.Scheme, Is.EqualTo("sip"));
                Assert.That(uri.Host, Is.EqualTo("1.2.3.4"));
                Assert.That(uri.Port, Is.EqualTo("1234"));
                Assert.That(uri.Username, Is.EqualTo("admin"));
                Assert.That(uri.Password, Is.EqualTo("pwd"));
                Assert.That(uri.Parameters[0].Name, Is.EqualTo("param"));
                Assert.That(uri.Parameters[0].Value, Is.EqualTo("foo"));
                Assert.That(uri.Headers[0].Name, Is.EqualTo("header"));
                Assert.That(uri.Headers[0].Value, Is.EqualTo("bar"));
            }
        }
    }
}