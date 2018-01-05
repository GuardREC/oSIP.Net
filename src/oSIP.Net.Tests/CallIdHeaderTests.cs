using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class CallIdHeaderTests
    {
        [Test]
        public void Shall_stringify_header()
        {
            using (var header = new CallIdHeader())
            {
                header.Number = "1";
                header.Host = "foo.bar.com";

                Assert.That(header.ToString(), Is.EqualTo("1@foo.bar.com"));
            }
        }

        [Test]
        public void Shall_parse_header()
        {
            using (CallIdHeader header = CallIdHeader.Parse("1@foo.bar.com"))
            {
                Assert.That(header.Number, Is.EqualTo("1"));
                Assert.That(header.Host, Is.EqualTo("foo.bar.com"));
            }
        }

        [Test]
        public void Shall_clone_header()
        {
            using (CallIdHeader original = CallIdHeader.Parse("1@foo.bar.com"))
            using (CallIdHeader cloned = original.DeepClone())
            {
                original.Number = "2";
                original.Host = "qwerty.dvorak.com";

                Assert.That(cloned.ToString(), Is.EqualTo("1@foo.bar.com"));
                Assert.That(original.ToString(), Is.EqualTo("2@qwerty.dvorak.com"));
            }
        }
    }
}