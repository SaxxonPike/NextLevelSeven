using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Routing
{
    /// <summary>
    /// A router that routes messages to a method unconditionally.
    /// </summary>
    sealed public class UnconditionalMethodRouter : IRouter
    {
        /// <summary>
        /// Create an unconditional router targeting the specified method.
        /// </summary>
        /// <param name="action">Method to route messages to.</param>
        public UnconditionalMethodRouter(Action<IMessage> action)
        {
            Action = action;
        }

        /// <summary>
        /// Method to route messages to.
        /// </summary>
        public readonly Action<IMessage> Action;

        /// <summary>
        /// Route a message. The message will always be considered handled.
        /// </summary>
        /// <param name="message">Message to route.</param>
        /// <returns>Always successful.</returns>
        public bool Route(IMessage message)
        {
            Action(message);
            return true;
        }
    }
}
