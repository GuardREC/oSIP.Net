using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class WwwAuthenticateHeaderTests
    {
        [Test]
        public void Shall_stringify_header()
        {
            using (var header = new WwwAuthenticateHeader())
            {
                header.AuthenticationType = "Basic";
                header.Realm = "\"a\"";
                header.Domain = "\"b\"";
                header.Nonce = "\"c\"";
                header.Opaque = "\"d\"";
                header.Stale = "\"e\"";
                header.Algorithm = "\"f\"";
                header.QopOptions = "\"g\"";
                header.Version = "\"h\"";
                header.TargetName = "\"i\"";
                header.GssApiData = "\"j\"";

                Assert.That(
                    header.ToString(),
                    Is.EqualTo(
                        "Basic realm=\"a\", domain=\"b\", nonce=\"c\", opaque=\"d\", stale=\"e\", algorithm=\"f\", " +
                        "qop=\"g\", version=\"h\", targetname=\"i\", gssapi-data=\"j\""));
            }
        }

        [Test]
        public void Shall_parse_header()
        {
            const string str =
                "Basic realm=\"a\", domain=\"b\", nonce=\"c\", opaque=\"d\", stale=\"e\", algorithm=\"f\", " +
                "qop=\"g\", version=\"h\", targetname=\"i\", gssapi-data=\"j\"";
            using (var header = WwwAuthenticateHeader.Parse(str))
            {
                Assert.That(header.AuthenticationType, Is.EqualTo("Basic"));
                Assert.That(header.Realm, Is.EqualTo("\"a\""));
                Assert.That(header.Domain, Is.EqualTo("\"b\""));
                Assert.That(header.Nonce, Is.EqualTo("\"c\""));
                Assert.That(header.Opaque, Is.EqualTo("\"d\""));
                Assert.That(header.Stale, Is.EqualTo("\"e\""));
                Assert.That(header.Algorithm, Is.EqualTo("\"f\""));
                Assert.That(header.QopOptions, Is.EqualTo("\"g\""));
                Assert.That(header.Version, Is.EqualTo("\"h\""));
                Assert.That(header.TargetName, Is.EqualTo("\"i\""));
                Assert.That(header.GssApiData, Is.EqualTo("\"j\""));
            }
        }

        [Test]
        public void Shall_clone_header()
        {
            using (var original = WwwAuthenticateHeader.Parse("Basic qop=\"auth\""))
            using (var cloned = original.DeepClone())
            {
                original.AuthenticationType = "Digest";
                original.QopOptions = null;
                original.Nonce = "\"abc\"";

                Assert.That(cloned.ToString(), Is.EqualTo("Basic  qop=\"auth\""));
                Assert.That(original.ToString(), Is.EqualTo("Digest  nonce=\"abc\""));
            }
        }
    }
}