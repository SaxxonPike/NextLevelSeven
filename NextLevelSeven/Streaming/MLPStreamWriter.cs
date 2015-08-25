using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Streaming
{
    public class MLPStreamWriter : HL7StreamWriter
    {
        public MLPStreamWriter(Stream baseStream) : base(baseStream)
        {
        }

        protected override Stream Process(byte[] data)
        {
            var mem = new MemoryStream();
            mem.WriteByte(0x0B);
            mem.Write(data, 0, data.Length);
            mem.WriteByte(0x1C);
            mem.WriteByte(0x0D);
            mem.Position = 0;
            return mem;
        }
    }
}
