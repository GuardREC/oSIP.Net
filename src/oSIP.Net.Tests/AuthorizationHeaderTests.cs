using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class AuthorizationHeaderTests
    {
        [Test]
        public void Shall_stringify_header()
        {
            var header = new AuthorizationHeader
            {
                AuthenticationType = "Basic",
                Username = "\"a\"",
                Realm = "\"b\"",
                Nonce = "\"c\"",
                Uri = "\"d\"",
                Response = "\"e\"",
                Digest = "\"f\"",
                Algorithm = "\"g\"",
                CNonce = "\"h\"",
                Opaque = "\"i\"",
                MessageQop = "\"j\"",
                NonceCount = "00000001",
                Version = "\"k\"",
                TargetName = "\"l\"",
                GssApiData = "\"m\"",
                CRand = "\"n\"",
                CNum = "\"o\""
            };

            Assert.That(
                header.ToString(),
                Is.EqualTo(
                    "Basic username=\"a\", realm=\"b\", nonce=\"c\", uri=\"d\", response=\"e\", digest=\"f\", " +
                    "algorithm=\"g\", cnonce=\"h\", opaque=\"i\", qop=\"j\", nc=00000001, version=\"k\", " +
                    "targetname=\"l\", gssapi-data=\"m\", crand=\"n\", cnum=\"o\""));
        }

        [Test]
        public void Shall_parse_header()
        {
            const string str =
                "Basic username=\"a\", realm=\"b\", nonce=\"c\", uri=\"d\", response=\"e\", digest=\"f\", " +
                "algorithm=\"g\", cnonce=\"h\", opaque=\"i\", qop=\"j\", nc=00000001, version=\"k\", " +
                "targetname=\"l\", gssapi-data=\"m\", crand=\"n\", cnum=\"o\"";

            Assert.That(AuthorizationHeader.TryParse(str, out AuthorizationHeader header), Is.True);
            Assert.That(header.AuthenticationType, Is.EqualTo("Basic"));
            Assert.That(header.Username, Is.EqualTo("\"a\""));
            Assert.That(header.Realm, Is.EqualTo("\"b\""));
            Assert.That(header.Nonce, Is.EqualTo("\"c\""));
            Assert.That(header.Uri, Is.EqualTo("\"d\""));
            Assert.That(header.Response, Is.EqualTo("\"e\""));
            Assert.That(header.Digest, Is.EqualTo("\"f\""));
            Assert.That(header.Algorithm, Is.EqualTo("\"g\""));
            Assert.That(header.CNonce, Is.EqualTo("\"h\""));
            Assert.That(header.Opaque, Is.EqualTo("\"i\""));
            Assert.That(header.MessageQop, Is.EqualTo("\"j\""));
            Assert.That(header.NonceCount, Is.EqualTo("00000001"));
            Assert.That(header.Version, Is.EqualTo("\"k\""));
            Assert.That(header.TargetName, Is.EqualTo("\"l\""));
            Assert.That(header.GssApiData, Is.EqualTo("\"m\""));
            Assert.That(header.CRand, Is.EqualTo("\"n\""));
            Assert.That(header.CNum, Is.EqualTo("\"o\""));
        }

        [Test]
        public void Shall_clone_header()
        {
            var original = AuthorizationHeader.Parse("Basic qop=\"auth\"");
            var cloned = original.DeepClone();

            original.AuthenticationType = "Digest";
            original.MessageQop = null;
            original.CNonce = "\"abc\"";

            Assert.That(cloned.ToString(), Is.EqualTo("Basic qop=\"auth\""));
            Assert.That(original.ToString(), Is.EqualTo("Digest cnonce=\"abc\""));
        }
    }
}