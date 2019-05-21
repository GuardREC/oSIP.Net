using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class CallIdHeaderTests
    {
        [Test]
        public void Shall_stringify_header()
        {
            var header = new CallIdHeader
            {
                Number = "1",
                Host = "foo.bar.com"
            };

            Assert.That(header.ToString(), Is.EqualTo("1@foo.bar.com"));
        }

        [Test]
        public void Shall_parse_header()
        {
            Assert.That(CallIdHeader.TryParse("1@foo.bar.com", out CallIdHeader header), Is.True);
            Assert.That(header.Number, Is.EqualTo("1"));
            Assert.That(header.Host, Is.EqualTo("foo.bar.com"));
        }

        [Test]
        public void Shall_clone_header()
        {
            CallIdHeader original = CallIdHeader.Parse("1@foo.bar.com");
            CallIdHeader cloned = original.DeepClone();

            original.Number = "2";
            original.Host = "qwerty.dvorak.com";

            Assert.That(cloned.ToString(), Is.EqualTo("1@foo.bar.com"));
            Assert.That(original.ToString(), Is.EqualTo("2@qwerty.dvorak.com"));
        }
    }
}