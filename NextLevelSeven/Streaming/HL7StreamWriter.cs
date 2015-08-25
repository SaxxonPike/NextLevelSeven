using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Streaming
{
    public class HL7StreamWriter : StreamWrapperBase, IMessageWriter
    {
        public HL7StreamWriter(Stream baseStream) : base(baseStream)
        {
        }

        virtual protected byte[] Encode(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }

        virtual protected string Interpret(IMessage message)
        {
            return message.ToString();
        }

        virtual protected Stream Process(byte[] data)
        {
            return new MemoryStream(data);
        }

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
    }
}
