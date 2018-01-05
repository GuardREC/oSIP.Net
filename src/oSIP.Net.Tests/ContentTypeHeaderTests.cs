using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class ContentTypeHeaderTests
    {
        [Test]
        public void Shall_stringify_header()
        {
            using (var header = new ContentTypeHeader())
            {
                header.Type = "application";
                header.SubType = "sdp";
                header.Parameters.Add(new GenericParameter("foo", "bar"));

                Assert.That(header.ToString(), Is.EqualTo("application/sdp; foo=bar"));
            }
        }

        [Test]
        public void Shall_parse_header()
        {
            using (ContentTypeHeader header = ContentTypeHeader.Parse("application/sdp; foo=bar"))
            {
                Assert.That(header.Type, Is.EqualTo("application"));
                Assert.That(header.SubType, Is.EqualTo("sdp"));
                Assert.That(header.Parameters[0].Name, Is.EqualTo("foo"));
                Assert.That(header.Parameters[0].Value, Is.EqualTo("bar"));
            }
        }

        [Test]
        public void Shall_clone_header()
        {
            using (ContentTypeHeader original = ContentTypeHeader.Parse("application/sdp"))
            using (ContentTypeHeader cloned = original.DeepClone())
            {
                original.Type = "multipart";
                original.SubType = "alternative";
                original.Parameters.Add(new GenericParameter("foo", "bar"));

                Assert.That(cloned.ToString(), Is.EqualTo("application/sdp"));
                Assert.That(original.ToString(), Is.EqualTo("multipart/alternative; foo=bar"));
            }
        }
    }
}