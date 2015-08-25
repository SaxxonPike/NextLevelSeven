using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Streaming
{
    abstract public class StreamWrapperBase
    {
        public StreamWrapperBase(Stream baseStream)
        {
            BaseStream = baseStream;
        }

        public Stream BaseStream
        {
            get;
            private set;
        }
    }
}
