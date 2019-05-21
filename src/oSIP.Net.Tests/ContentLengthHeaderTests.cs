using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class ContentLengthHeaderTests
    {
        [Test]
        public void Shall_stringify_header()
        {
            var header = new ContentLengthHeader
            {
                Value = "42"
            };

            Assert.That(header.ToString(), Is.EqualTo("42"));
        }

        [Test]
        public void Shall_parse_header()
        {
            Assert.That(ContentLengthHeader.TryParse("42", out ContentLengthHeader header), Is.True);
            Assert.That(header.Value, Is.EqualTo("42"));
        }
    }
}