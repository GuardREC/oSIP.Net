using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class GenericHeaderTests
    {
        [Test]
        public void Shall_stringify_header()
        {
            var header = new GenericHeader("From", "sip:john.smith@abc.com");
            Assert.That(header.ToString(), Is.EqualTo("From: sip:john.smith@abc.com"));
        }

        [Test]
        public void Shall_clone_header()
        {
            GenericHeader original = new GenericHeader("From", "sip:john.smith@abc.com");
            GenericHeader cloned = original.DeepClone();

            original.Name = "To";
            original.Value = "sip:joe.shmoe@abc.com";

            Assert.That(cloned.ToString(), Is.EqualTo("From: sip:john.smith@abc.com"));
            Assert.That(original.ToString(), Is.EqualTo("To: sip:joe.shmoe@abc.com"));
        }
    }
}