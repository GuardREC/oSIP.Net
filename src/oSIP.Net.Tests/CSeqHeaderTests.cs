using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class CSeqHeaderTests
    {
        [Test]
        public void Shall_stringify_header()
        {
            var header = new CSeqHeader
            {
                Method = "INVITE",
                Number = "1"
            };

            Assert.That(header.ToString(), Is.EqualTo("1 INVITE"));
        }

        [Test]
        public void Shall_parse_header()
        {
            Assert.That(CSeqHeader.TryParse("1 INVITE", out CSeqHeader header), Is.True);
            Assert.That(header.Method, Is.EqualTo("INVITE"));
            Assert.That(header.Number, Is.EqualTo("1"));
        }

        [Test]
        public void Shall_clone_header()
        {
            CSeqHeader original = CSeqHeader.Parse("1 INVITE");
            CSeqHeader cloned = original.DeepClone();

            original.Method = "SUBSCRIBE";
            original.Number = "2";

            Assert.That(cloned.ToString(), Is.EqualTo("1 INVITE"));
            Assert.That(original.ToString(), Is.EqualTo("2 SUBSCRIBE"));
        }
    }
}