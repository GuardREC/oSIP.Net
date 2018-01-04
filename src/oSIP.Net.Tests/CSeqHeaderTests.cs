using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class CSeqHeaderTests
    {
        [Test]
        public void Shall_stringify_header()
        {
            using (var header = new CSeqHeader())
            {
                header.Method = "INVITE";
                header.Number = "1";

                Assert.That(header.ToString(), Is.EqualTo("1 INVITE"));
            }
        }

        [Test]
        public void Shall_parse_header()
        {
            using (CSeqHeader header = CSeqHeader.Parse("1 INVITE"))
            {
                Assert.That(header.Method, Is.EqualTo("INVITE"));
                Assert.That(header.Number, Is.EqualTo("1"));
            }
        }

        [Test]
        public void Shall_clone_header()
        {
            using (CSeqHeader original = CSeqHeader.Parse("1 INVITE"))
            using (CSeqHeader cloned = original.DeepClone())
            {
                original.Method = "SUBSCRIBE";
                original.Number = "2";

                Assert.That(cloned.ToString(), Is.EqualTo("1 INVITE"));
                Assert.That(original.ToString(), Is.EqualTo("2 SUBSCRIBE"));
            }
        }
    }
}