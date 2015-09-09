using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Streaming;

namespace NextLevelSeven.Test.Streaming
{
    [TestClass]
    public class MlpStreamReaderTests
    {
        [TestMethod]
        public void MLPStreamReader_CanReadAllMessages()
        {
            var messageData = Encoding.UTF8.GetBytes(ExampleMessages.MultipleMessagesAsMlp);
            using (var messageStream = new MemoryStream(messageData))
            {
                var reader = new MlpStreamReader(messageStream);
                var messages = reader.ReadAll().ToArray();
                Assert.AreEqual(3, messages.Length);
                Assert.AreEqual("MSH|^~\\&|1", messages[0].Value);
                Assert.AreEqual("MSH|^~\\&|2", messages[1].Value);
                Assert.AreEqual("MSH|^~\\&|3", messages[2].Value);
            }
        }

        [TestMethod]
        public void MLPStreamReader_CanReadAllMultiLineMessage()
        {
            var messageData = Encoding.UTF8.GetBytes(ExampleMessages.MultipleMessagesWithMultipleLinesAsMlp);
            using (var messageStream = new MemoryStream(messageData))
            {
                var reader = new MlpStreamReader(messageStream);
                var messages = reader.ReadAll().ToArray();
                Assert.AreEqual(3, messages.Length);
                Assert.AreEqual("MSH|^~\\&|\xDPID|12341", messages[0].Value);
                Assert.AreEqual("MSH|^~\\&|\xDPID|12342", messages[1].Value);
                Assert.AreEqual("MSH|^~\\&|\xDPID|12343", messages[2].Value);
            }
        }

        [TestMethod]
        public void MLPStreamReader_CanReadMessagesOneByOneInAStream()
        {
            var messageData = Encoding.UTF8.GetBytes(ExampleMessages.MultipleMessagesAsMlp);
            using (var messageStream = new MemoryStream(messageData))
            {
                var reader = new MlpStreamReader(messageStream);
                var message1 = reader.Read();
                var message2 = reader.Read();
                var message3 = reader.Read();
                var message4 = reader.Read();
                Assert.IsNotNull(message1, "Message 1 was null.");
                Assert.IsNotNull(message2, "Message 2 was null.");
                Assert.IsNotNull(message3, "Message 3 was null.");
                Assert.IsNull(message4, "A message was found, but not expected.");
            }
        }

        [TestMethod]
        public void MLPStreamReader_ThrowsOnMalformedPacket()
        {
            var messageData = Encoding.UTF8.GetBytes("Bad packet");
            using (var messageStream = new MemoryStream(messageData))
            {
                var reader = new MlpStreamReader(messageStream);
                It.Throws<MlpStreamException>(() => reader.Read());
            }
        }

        [TestMethod]
        public void MLPStreamReader_ThrowsOnPacketMissingCarriageReturn()
        {
            var messageData = Encoding.UTF8.GetBytes("\xBMSH|^~\\&|\x1C");
            using (var messageStream = new MemoryStream(messageData))
            {
                var reader = new MlpStreamReader(messageStream);
                It.Throws<MlpStreamException>(() => reader.Read());
            }
        }

        [TestMethod]
        public void MLPStreamReader_ThrowsOnPacketMissingFileDivider()
        {
            var messageData = Encoding.UTF8.GetBytes("\xBMSH|^~\\&|\xD");
            using (var messageStream = new MemoryStream(messageData))
            {
                var reader = new MlpStreamReader(messageStream);
                It.Throws<MlpStreamException>(() => reader.Read());
            }
        }

        [TestMethod]
        public void MLPStreamReader_OnlyReadsOnePacketLength()
        {
            var messageData = Encoding.UTF8.GetBytes("\xBMSH|^~\\&|\x1C\xD ");
            using (var messageStream = new MemoryStream(messageData))
            {
                var reader = new MlpStreamReader(messageStream);
                reader.Read();
                Assert.IsTrue(messageStream.Position < messageStream.Length, "MLP text reader read too much data.");
            }
        }

    }
}