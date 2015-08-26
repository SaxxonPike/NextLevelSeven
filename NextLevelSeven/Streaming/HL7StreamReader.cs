using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Streaming
{
    public class HL7StreamReader : StreamWrapperBase, IMessageReader
    {
        public HL7StreamReader(Stream baseStream) : base(baseStream)
        {
        }

        virtual protected IMessage Decode(byte[] data)
        {
            return Interpret(Encoding.UTF8.GetString(data));
        }

        virtual protected IMessage Interpret(string data)
        {
            return new Message(data);
        }

        virtual protected IMessage Process(Stream stream)
        {
            var length = (int)(stream.Length - stream.Position);
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

        virtual public IMessage Read()
        {
            return Process(BaseStream);
        }
    }
}
