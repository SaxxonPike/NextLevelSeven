using System.Collections.Generic;
using System.IO;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Parsing;

namespace NextLevelSeven.Streaming
{
    /// <summary>
    ///     A reader that pulls messages from the MLP format.
    /// </summary>
    public class MlpStreamReader : MessageStreamReader
    {
        /// <summary>
        ///     Create an MLP stream reader that uses a stream as a data source.
        /// </summary>
        /// <param name="baseStream">Stream to get messages from.</param>
        public MlpStreamReader(Stream baseStream) : base(baseStream)
        {
        }

        /// <summary>
        ///     Read one MLP-encoded message. Returns null if no messages are available.
        /// </summary>
        /// <returns>Message that was read.</returns>
        public override IMessageParser Read()
        {
            int vtByte = BaseStream.ReadByte();
            if (vtByte == -1)
            {
                return null;
            }
            if (vtByte != 0x0B)
            {
                throw new MlpStreamException(ErrorCode.HeaderByteIsIncorrect);
            }

            using (var mem = new MemoryStream())
            {
                unchecked
                {
                    while (true)
                    {
                        var buffer = BaseStream.ReadByte();
                        if (buffer == -1)
                        {
                            throw new MlpStreamException(ErrorCode.MlpDataEndedPrematurely);
                        }

                        if (buffer == 0x1C)
                        {
                            buffer = BaseStream.ReadByte();
                            if (buffer == 0x0D)
                            {
                                break;
                            }
                            if (buffer == -1)
                            {
                                throw new MlpStreamException(ErrorCode.MlpDataEndedPrematurely);
                            }
                        }
                        mem.WriteByte((byte) buffer);
                    }
                }

                return Decode(mem.ToArray());
            }
        }

        /// <summary>
        ///     Read all messages in the stream. If empty, there were no more messages.
        /// </summary>
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