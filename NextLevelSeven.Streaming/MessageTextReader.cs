using System.Collections.Generic;
using System.IO;
using NextLevelSeven.Parsing;

namespace NextLevelSeven.Streaming
{
    /// <summary>A reader that reads textual HL7 messages, separated by blank lines.</summary>
    public class MessageTextReader : MessageStreamReader
    {
        /// <summary>TextReader used to perform operations on the base stream.</summary>
        protected readonly TextReader Reader;

        /// <summary>Create a textual HL7 message reader using the specified stream as a source.</summary>
        /// <param name="baseStream">Stream to use as a source.</param>
        public MessageTextReader(Stream baseStream)
            : base(baseStream)
        {
            Reader = new StreamReader(baseStream);
        }

        /// <summary>Read one textual HL7 message from the stream.</summary>
        /// <returns>Message that was read, or null if there are no more messages.</returns>
        public override IMessageParser Read()
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

            return lines.Count == 0 ? null : Interpret(string.Join("\xD", lines));
        }

        /// <summary>Read all messages in the stream. If empty, there were no more messages.</summary>
        /// <returns>Messages that were read.</returns>
        public override IEnumerable<IMessageParser> ReadAll()
        {
            var messages = new List<IMessageParser>();
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