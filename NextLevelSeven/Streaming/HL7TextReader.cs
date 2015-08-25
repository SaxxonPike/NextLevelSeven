using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Streaming
{
    public class HL7TextReader : HL7StreamReader
    {
        public HL7TextReader(Stream baseStream) : base(baseStream)
        {
            Reader = new StreamReader(baseStream);
        }

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

            return Interpret(string.Join("\xD", lines));
        }

        protected TextReader Reader
        {
            get;
            private set;
        }
    }
}
