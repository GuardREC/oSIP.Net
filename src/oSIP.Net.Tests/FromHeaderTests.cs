using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class FromHeaderTests
    {
        [Test]
        public void Shall_set_url_to_null()
        {
            using (var header = new FromHeader())
            {
                header.Url = null;
                Assert.That(header.Url, Is.Null);
            }
        }

        [Test]
        public void Shall_stringify_header()
        {
            using (var header = new FromHeader())
            {
                header.DisplayName = "Mr President";
                header.Url = SipUri.Parse("sip:donald@trump.usa");
                header.Parameters.Add(new GenericParameter("foo", "bar"));

                Assert.That(
                    header.ToString(),
                    Is.EqualTo("Mr President <sip:donald@trump.usa>;foo=bar"));
            }
        }

        [Test]
        public void Shall_parse_header()
        {
            using (FromHeader header = FromHeader.Parse("Mr President <sip:donald@trump.gov>;foo=bar"))
            {
                Assert.That(header.DisplayName, Is.EqualTo("Mr President"));
                Assert.That(header.Url.ToString(), Is.EqualTo("sip:donald@trump.gov"));
                Assert.That(header.Parameters[0].Name, Is.EqualTo("foo"));
                Assert.That(header.Parameters[0].Value, Is.EqualTo("bar"));
            }
        }

        [Test]
        public void Shall_clone_header()
        {
            using (FromHeader original = FromHeader.Parse("Mr President <sip:donald@trump.gov>"))
            using (FromHeader cloned = original.DeepClone())
            {
                original.DisplayName = "Barack Obama";
                original.Url = SipUri.Parse("sip:barack@obama.gov");
                original.Parameters.Add(new GenericParameter("foo", "bar"));

                Assert.That(cloned.ToString(), Is.EqualTo("Mr President <sip:donald@trump.gov>"));
                Assert.That(original.ToString(), Is.EqualTo("Barack Obama <sip:barack@obama.gov>;foo=bar"));
            }
        }
    }
}