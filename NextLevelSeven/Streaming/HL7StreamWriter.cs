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
    /// A stream writer for HL7 data.
    /// </summary>
    public class HL7StreamWriter : StreamWrapperBase, IMessageWriter
    {
        /// <summary>
        /// Create an HL7 stream writer that puts its information into a base stream.
        /// </summary>
        /// <param name="baseStream">Stream to write to.</param>
        public HL7StreamWriter(Stream baseStream) : base(baseStream)
        {
        }

        /// <summary>
        /// Convert a string into raw bytes.
        /// </summary>
        /// <param name="data">Data to convert.</param>
        /// <returns>Converted bytes.</returns>
        virtual protected byte[] Encode(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }

        /// <summary>
        /// Convert an HL7 message into a single string.
        /// </summary>
        /// <param name="message">Message to convert.</param>
        /// <returns>Converted message.</returns>
        virtual protected string Interpret(IMessage message)
        {
            return message.ToString();
        }

        /// <summary>
        /// Convert raw data into a data stream.
        /// </summary>
        /// <param name="data">Data to convert.</param>
        /// <returns>Converted data stream.</returns>
        virtual protected Stream Process(byte[] data)
        {
            return new MemoryStream(data);
        }

        /// <summary>
        /// Write an HL7 message to the stream.
        /// </summary>
        /// <param name="message">Message to write.</param>
        virtual public void Write(IMessage message)
        {
            using (var stream = Process(Encode(Interpret(message))))
            {
                var length = (int)(stream.Length - stream.Position);
                var buffer = new byte[length];
                var offset = 0;
                while (offset < length)
                {
                    offset += stream.Read(buffer, offset, length - offset);
                }
                BaseStream.Write(buffer, 0, length);
            }
        }

        /// <summary>
        /// Write all messages to the stream.
        /// </summary>
        /// <param name="messages">Messages to write.</param>
        virtual public void WriteAll(IEnumerable<IMessage> messages)
        {
            foreach (var message in messages)
            {
                Write(message);
            }
        }
    }
}
