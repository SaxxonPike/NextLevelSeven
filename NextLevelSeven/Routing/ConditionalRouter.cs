using System;
using NextLevelSeven.Core;

namespace NextLevelSeven.Routing
{
    /// <summary>
    /// A router that routes messages to another router conditionally.
    /// </summary>
    sealed public class ConditionalRouter : IRouter
    {
        /// <summary>
        /// Create a conditional router targeting the specified method.
        /// </summary>
        /// <param name="condition">Condition that must be met for messages to be routed.</param>
        /// <param name="targetRouter">Router to route messages to when they meet the condition.</param>
        public ConditionalRouter(Func<IMessage, bool> condition, IRouter targetRouter = null)
        {
            TargetRouter = targetRouter;
            Condition = condition;
        }

        /// <summary>
        /// Condition to check messages against. Returns true if met.
        /// </summary>
        public readonly Func<IMessage, bool> Condition;

        /// <summary>
        /// Router to route messages to when the condition is met.
        /// </summary>
        public IRouter TargetRouter;

        /// <summary>
        /// If the condition is met, route the message and return true. Returns false otherwise.
        /// </summary>
        /// <param name="message">Message to route.</param>
        /// <returns>True if the message was handled.</returns>
        public bool Route(IMessage message)
        {
            if (Condition(message))
            {
                return TargetRouter == null || TargetRouter.Route(message);
            }
            return false;
        }
    }
}
