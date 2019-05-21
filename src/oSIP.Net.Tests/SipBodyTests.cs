using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class SipBodyTests
    {
        [Test]
        public void Shall_stringify_body()
        {
            var body = new SipBody();
            body.Data = "abc";
            body.ContentType = ContentTypeHeader.Parse("text/plain");
            body.Headers.Add(new GenericHeader("foo", "bar"));

            Assert.That(body.ToString(), Is.EqualTo("content-type: text/plain\r\nFoo: bar\r\n\r\nabc"));
        }

        [Test]
        public void Shall_parse_body()
        {
            var body = SipBody.Parse("abc");

            Assert.That(body.Data, Is.EqualTo("abc"));
            Assert.That(body.ContentType, Is.Null);
        }

        [Test]
        public void Shall_parse_mime_body()
        {
            var body = SipBody.ParseMime("content-type: text/plain\r\n\r\nabc");

            Assert.That(body.Data, Is.EqualTo("abc"));
            Assert.That(body.ContentType.ToString(), Is.EqualTo("text/plain"));
        }

        [Test]
        public void Shall_clone_body()
        {
            var original = SipBody.Parse("abc");
            var cloned = original.DeepClone();

            original.Data = "def";

            Assert.That(cloned.Data, Is.EqualTo("abc"));
            Assert.That(original.Data, Is.EqualTo("def"));
        }
    }
}