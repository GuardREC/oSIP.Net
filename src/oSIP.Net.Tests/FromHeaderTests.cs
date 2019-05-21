using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class FromHeaderTests
    {
        [Test]
        public void Shall_set_url_to_null()
        {
            var header = new NameAddressHeader
            {
                Url = null
            };
            Assert.That(header.Url, Is.Null);
        }

        [Test]
        public void Shall_stringify_header()
        {
            var header = new NameAddressHeader
            {
                DisplayName = "Mr President",
                Url = SipUri.Parse("sip:donald@trump.usa"),
                Parameters =
                {
                    new GenericParameter("foo", "bar")
                }
            };

            Assert.That(
                header.ToString(),
                Is.EqualTo("Mr President <sip:donald@trump.usa>;foo=bar"));
        }

        [Test]
        public void Shall_parse_header()
        {
            const string str = "Mr President <sip:donald@trump.gov>;foo=bar";

            Assert.That(NameAddressHeader.TryParse(str, out NameAddressHeader header), Is.True);
            Assert.That(header.DisplayName, Is.EqualTo("Mr President"));
            Assert.That(header.Url.ToString(), Is.EqualTo("sip:donald@trump.gov"));
            Assert.That(header.Parameters[0].Name, Is.EqualTo("foo"));
            Assert.That(header.Parameters[0].Value, Is.EqualTo("bar"));
        }

        [Test]
        public void Shall_clone_header()
        {
            NameAddressHeader original = NameAddressHeader.Parse("Mr President <sip:donald@trump.gov>");
            NameAddressHeader cloned = original.DeepClone();

            original.DisplayName = "Barack Obama";
            original.Url = SipUri.Parse("sip:barack@obama.gov");
            original.Parameters.Add(new GenericParameter("foo", "bar"));

            Assert.That(cloned.ToString(), Is.EqualTo("Mr President <sip:donald@trump.gov>"));
            Assert.That(original.ToString(), Is.EqualTo("Barack Obama <sip:barack@obama.gov>;foo=bar"));
        }
    }
}