using System.Collections.Generic;
using System.IO;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;

namespace NextLevelSeven.Streaming
{
    /// <summary>A stream reader for HL7 data.</summary>
    public class MessageStreamReader : StreamWrapperBase, IMessageReader
    {
        /// <summary>Create an HL7 stream reader that gets its information from a base stream.</summary>
        /// <param name="baseStream"></param>
        public MessageStreamReader(Stream baseStream)
            : base(baseStream)
        {
        }

        /// <summary>Read the next message in the stream.</summary>
        /// <returns>Message that was read, or null if there were no more.</returns>
        public virtual IMessageParser Read()
        {
            return Process(BaseStream);
        }

        /// <summary>
        ///     Read all messages in the stream. If empty, there were no more messages. (Using the base stream reader, the
        ///     entire stream will be read as one message.)
        /// </summary>
        /// <returns>Messages that were read.</returns>
        public virtual IEnumerable<IMessageParser> ReadAll()
        {
            var message = Read();
            return message != null
                ? new[]
                {
                    message
                }
                : new IMessageParser[]
                {
                };
        }

        /// <summary>Decode raw data bytes to an HL7 message.</summary>
        /// <param name="data">Data to decode.</param>
        /// <returns>Decoded message.</returns>
        protected virtual IMessageParser Decode(byte[] data)
        {
            return Interpret(Encoding.UTF8.GetString(data));
        }

        /// <summary>Interpret textual data into an HL7 message.</summary>
        /// <param name="data">Text to interpret.</param>
        /// <returns>Interpreted message.</returns>
        protected virtual IMessageParser Interpret(string data)
        {
            return Message.Parse(data);
        }

        /// <summary>Read raw data from a stream and convert it into an HL7 message.</summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>Processed message.</returns>
        protected virtual IMessageParser Process(Stream stream)
        {
            var length = (int) (stream.Length - stream.Position);
            var buffer = new byte[length];
            var offset = 0;

            if (length <= 0)
            {
                return null;
            }

            while (offset < length)
            {
                offset += stream.Read(buffer, offset, length - offset);
            }

            return Decode(buffer);
        }
    }
}