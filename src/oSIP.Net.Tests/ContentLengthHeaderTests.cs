using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class ContentLengthHeaderTests
    {
        [Test]
        public void Shall_stringify_header()
        {
            using (var header = new ContentLengthHeader())
            {
                header.Value = "42";

                Assert.That(header.ToString(), Is.EqualTo("42"));
            }
        }

        [Test]
        public void Shall_parse_header()
        {
            using (ContentLengthHeader header = ContentLengthHeader.Parse("42"))
            {
                Assert.That(header.Value, Is.EqualTo("42"));
            }
        }
    }
}