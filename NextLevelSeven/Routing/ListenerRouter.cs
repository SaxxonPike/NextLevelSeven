using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Routing
{
    /// <summary>
    /// Router that calls a method each time a message is passed through. 
    /// </summary>
    public class ListenerRouter : IRouter
    {
        public ListenerRouter(Action<IMessage> action, IRouter targetRouter)
        {
            Action = action;
            TargetRouter = targetRouter;
        }

        /// <summary>
        /// Method to relay messages to.
        /// </summary>
        public readonly Action<IMessage> Action;

        /// <summary>
        /// Router to route messages to when the condition is met.
        /// </summary>
        public readonly IRouter TargetRouter;

        /// <summary>
        /// Route a message through the listener and to the target router.
        /// </summary>
        /// <param name="message">Message to route.</param>
        /// <returns></returns>
        public bool Route(IMessage message)
        {
            Action(message);
            return TargetRouter.Route(message);
        }
    }
}
