using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Streaming;

namespace NextLevelSeven.Test.Streaming
{
    [TestClass]
    public class TextReaderTests
    {
        [TestMethod]
        public void Utility_TestAllMessagesInBin()
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
                            var reader = new HL7TextReader(mem);
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
