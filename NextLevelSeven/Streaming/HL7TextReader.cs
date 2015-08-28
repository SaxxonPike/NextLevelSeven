using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Streaming
{
    /// <summary>
    /// An HL7StreamReader that reads textual HL7 messages, separated by blank lines.
    /// </summary>
    public class HL7TextReader : HL7StreamReader
    {
        /// <summary>
        /// Create a textual HL7 message reader using the specified stream as a source.
        /// </summary>
        /// <param name="baseStream">Stream to use as a source.</param>
        public HL7TextReader(Stream baseStream) : base(baseStream)
        {
            Reader = new StreamReader(baseStream);
        }

        /// <summary>
        /// Read one textual HL7 message from the stream.
        /// </summary>
        /// <returns>Message that was read, or null if there are no more messages.</returns>
        public override IMessage Read()
        {
            var lines = new List<string>();
            var foundLine = false;

            while (true)
            {
                var line = Reader.ReadLine();
                if (line == null)
                {
                    break;
                }

                if (line.Length == 0)
                {
                    if (foundLine)
                    {
                        break;
                    }
                }
                else
                {
                    foundLine = true;
                    lines.Add(line);
                }
            }

            if (lines.Count == 0)
            {
                return null;
            }

            return Interpret(string.Join("\xD", lines));
        }

        /// <summary>
        /// TextReader used to perform operations on the base stream.
        /// </summary>
        protected TextReader Reader
        {
            get;
            private set;
        }

        /// <summary>
        /// Read all messages in the stream. If empty, there were no more messages.
        /// </summary>
        /// <returns>Messages that were read.</returns>
        public override IEnumerable<IMessage> ReadAll()
        {
            var messages = new List<IMessage>();
            while (true)
            {
                var message = Read();
                if (message == null)
                {
                    return messages;
                }
                messages.Add(message);
            }
        }
    }
}
