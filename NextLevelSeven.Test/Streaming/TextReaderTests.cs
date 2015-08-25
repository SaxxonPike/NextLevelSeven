using System;
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

            if (Directory.Exists(targetPath))
            {
                foreach (var file in Directory.GetFiles(targetPath))
                {
                    using (var mem = new MemoryStream(File.ReadAllBytes(file)))
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
            }

            Debug.WriteLine("Messages parsed: {0}. Files read: {1}. Bytes read: {2}", messageCount, fileCount, byteCount);
        }
    }
}
