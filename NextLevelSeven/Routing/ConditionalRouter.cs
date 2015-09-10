using System;
using NextLevelSeven.Native;

namespace NextLevelSeven.Routing
{
    /// <summary>
    ///     A router that routes messages to another router conditionally.
    /// </summary>
    public sealed class ConditionalRouter : IRouter
    {
        /// <summary>
        ///     Condition to check messages against. Returns true if met.
        /// </summary>
        public readonly Func<INativeMessage, bool> Condition;

        /// <summary>
        ///     Router to route messages to when the condition is met.
        /// </summary>
        public IRouter TargetRouter;

        /// <summary>
        ///     Create a conditional router targeting the specified method.
        /// </summary>
        /// <param name="condition">Condition that must be met for messages to be routed.</param>
        /// <param name="targetRouter">Router to route messages to when they meet the condition.</param>
        public ConditionalRouter(Func<INativeMessage, bool> condition, IRouter targetRouter = null)
        {
            TargetRouter = targetRouter;
            Condition = condition;
        }

        /// <summary>
        ///     If the condition is met, route the message and return true. Returns false otherwise.
        /// </summary>
        /// <param name="message">Message to route.</param>
        /// <returns>True if the message was handled.</returns>
        public bool Route(INativeMessage message)
        {
            if (Condition(message))
            {
                return TargetRouter == null || TargetRouter.Route(message);
            }
            return false;
        }
    }
}