using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Streaming;

namespace NextLevelSeven.Test.Streaming
{
    [TestClass]
    public class Hl7TextReaderTests
    {
        [TestMethod]
        public void HL7TextReader_CanReadAllMessagesInAStream()
        {
            var messageData = Encoding.UTF8.GetBytes(ExampleMessages.MultipleMessagesSeparatedByLines);
            using (var messageStream = new MemoryStream(messageData))
            {
                var reader = new MessageTextReader(messageStream);
                var messages = reader.ReadAll().ToArray();
                Assert.AreEqual(3, messages.Length);
                Assert.AreEqual("MSH|^~\\&|1", messages[0].Value, @"Message 1 was not read properly.");
                Assert.AreEqual("MSH|^~\\&|2", messages[1].Value, @"Message 2 was not read properly.");
                Assert.AreEqual("MSH|^~\\&|3", messages[2].Value, @"Message 3 was not read properly.");
            }
        }

        [TestMethod]
        public void HL7TextReader_CanReadMessagesOneByOneInAStream()
        {
            var messageData = Encoding.UTF8.GetBytes(ExampleMessages.MultipleMessagesSeparatedByLines);
            using (var messageStream = new MemoryStream(messageData))
            {
                var reader = new MessageTextReader(messageStream);
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
        public void HL7TextReader_CanTestAllMessagesInBin()
        {
            var fileCount = 0;
            var messageCount = 0;
            var byteCount = 0;
            var targetPath = Path.Combine(Environment.CurrentDirectory, @"TestMessages");
            var messages = new List<byte[]>();

            if (Directory.Exists(targetPath))
            {
                Debug.WriteLine("Loading...");
                foreach (var file in Directory.GetFiles(targetPath))
                {
                    messages.Add(File.ReadAllBytes(file));
                    Debug.WriteLine(file);
                }

                Debug.WriteLine("Converting...");
                Measure.ExecutionTime(() =>
                {
                    foreach (var rawMessage in messages)
                    {
                        using (var mem = new MemoryStream(rawMessage))
                        {
                            fileCount++;
                            byteCount += (int)mem.Length;
                            var reader = new MessageTextReader(mem);
                            while (true)
                            {
                                var message = reader.Read();
                                if (message == null)
                                {
                                    break;
                                }
                                messageCount++;
                            }
                        }
                    }
                });
            }

            Debug.WriteLine("Messages parsed: {0}. Files read: {1}. Bytes read: {2}", messageCount, fileCount, byteCount);
        }
    }
}
