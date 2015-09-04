using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Specification.Processing
{
    public delegate void ProcessorEventHandler(object sender, ProcessorEventArgs e);

    public class ProcessorEventArgs : EventArgs
    {
        public ProcessorEventArgs(IMessage message)
        {
            Message = message;
        }

        public readonly IMessage Message;
    }
}
