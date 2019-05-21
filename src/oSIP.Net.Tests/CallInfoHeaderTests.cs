using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class CallInfoHeaderTests
    {
        [Test]
        public void Shall_stringify_header()
        {
            var header = new CallInfoHeader();
            header.Element = "<http://www.abc.com/photo.png>";
            header.Parameters.Add(new GenericParameter("purpose", "icon"));

            Assert.That(
                header.ToString(),
                Is.EqualTo("<http://www.abc.com/photo.png>;purpose=icon"));
        }

        [Test]
        public void Shall_parse_header()
        {
            CallInfoHeader header = CallInfoHeader.Parse("<http://www.abc.com/photo.png>;purpose=icon");

            Assert.That(header.Element, Is.EqualTo("<http://www.abc.com/photo.png>"));
            Assert.That(header.Parameters[0].Name, Is.EqualTo("purpose"));
            Assert.That(header.Parameters[0].Value, Is.EqualTo("icon"));
        }

        [Test]
        public void Shall_clone_header()
        {
            CallInfoHeader original = CallInfoHeader.Parse("<http://www.abc.com/photo.png>;purpose=icon");
            CallInfoHeader cloned = original.DeepClone();

            original.Element = "<http://www.abc.com/info>";
            original.Parameters.RemoveAt(0);
            original.Parameters.Add(new GenericParameter("purpose", "info"));

            Assert.That(cloned.ToString(), Is.EqualTo("<http://www.abc.com/photo.png>;purpose=icon"));
            Assert.That(original.ToString(), Is.EqualTo("<http://www.abc.com/info>;purpose=info"));
        }
    }
}