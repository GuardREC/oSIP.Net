using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class AcceptEncodingHeaderTests
    {
        [Test]
        public void Shall_stringify_header()
        {
            var header = new AcceptEncodingHeader();
            header.Element = "gzip";
            header.Parameters.Add(new GenericParameter("foo", "bar"));
            Assert.That(header.ToString(), Is.EqualTo("gzip;foo=bar"));
        }

        [Test]
        public void Shall_parse_header()
        {
            AcceptEncodingHeader header = AcceptEncodingHeader.Parse("gzip;foo=bar");
            Assert.That(header.Element, Is.EqualTo("gzip"));
            Assert.That(header.Parameters[0].Name, Is.EqualTo("foo"));
            Assert.That(header.Parameters[0].Value, Is.EqualTo("bar"));
        }

        [Test]
        public void Shall_clone_header()
        {
            AcceptEncodingHeader original = AcceptEncodingHeader.Parse("gzip");
            AcceptEncodingHeader cloned = original.DeepClone();

            original.Element = "deflate";
            original.Parameters.Add(new GenericParameter("foo", "bar"));

            Assert.That(cloned.ToString(), Is.EqualTo("gzip"));
            Assert.That(original.ToString(), Is.EqualTo("deflate;foo=bar"));
        }
    }
}