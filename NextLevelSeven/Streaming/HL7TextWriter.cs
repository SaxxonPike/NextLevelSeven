using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Streaming
{
    public class HL7TextWriter : HL7StreamWriter
    {
        public HL7TextWriter(Stream baseStream) : base(baseStream)
        {
            Writer = new StreamWriter(baseStream);
        }

        public override void Write(IMessage message)
        {
            Writer.Write(Interpret(message));
        }

        protected TextWriter Writer
        {
            get;
            private set;
        }
    }
}
