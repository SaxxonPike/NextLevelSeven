using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Streaming
{
    public class MLPStreamReader : HL7StreamReader
    {
        public MLPStreamReader(Stream baseStream) : base(baseStream)
        {
        }

        public override IMessage Read()
        {
            int vtByte = BaseStream.ReadByte();
            if (vtByte == -1)
            {
                throw new EndOfStreamException();
            }
            if (vtByte != 0x0B)
            {
                throw new MLPStreamException(@"Header byte is incorrect.");
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
                            throw new EndOfStreamException();
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
                                throw new EndOfStreamException();
                            }
                        }
                        mem.WriteByte((byte)buffer);
                    }
                }

                return Decode(mem.ToArray());
            }
        }
    }
}
