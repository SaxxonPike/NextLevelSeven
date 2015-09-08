using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Routing;

namespace NextLevelSeven.Test.Routing
{
    public class NullRouter : IRouter
    {
        public NullRouter(bool success)
        {
            Success = success;
        }

        public bool Route(IMessage message)
        {
            LastMessage = message;
            Routed = Routed || Success;
            Checked = true;
            return Success;
        }

        public bool Checked { get; private set; }

        public IMessage LastMessage { get; private set; }

        public bool Routed { get; private set; }

        public readonly bool Success;
    }
}
