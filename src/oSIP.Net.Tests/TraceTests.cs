using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class TraceTests
    {
        [Test]
        public void Shall_intercept_trace()
        {
            try
            {
                TraceEvent traceEvent = null;
                Trace.SetLogger(e => { traceEvent = e; });

                SipMessage.Parse("INVITE sip:john.smith@abc.com SIP/0.0\r\n\r\n");

                Assert.That(traceEvent.FileName, Is.EqualTo("osip_message_parse.c"));
                Assert.That(traceEvent.LineNumber, Is.EqualTo(168));
                Assert.That(traceEvent.Level, Is.EqualTo(TraceLevel.Error));
                Assert.That(traceEvent.Message, Is.EqualTo("Wrong version number\n"));
            }
            finally
            {
                Trace.SetLogger(null);
            }
        }

        [Test]
        public void Shall_set_level()
        {
            try
            {
                TraceEvent traceEvent = null;

                Trace.SetLogger(e => { traceEvent = e; });
                Trace.SetLevel(TraceLevel.Fatal);

                SipMessage.Parse("INVITE sip:john.smith@abc.com SIP/0.0\r\n\r\n");

                Assert.That(traceEvent, Is.Null);
            }
            finally
            {
                Trace.SetLogger(null);
            }
        }
    }
}