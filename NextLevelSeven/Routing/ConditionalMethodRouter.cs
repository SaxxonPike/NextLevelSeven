using System;
using NextLevelSeven.Core;
using NextLevelSeven.Native;

namespace NextLevelSeven.Routing
{
    /// <summary>
    /// A router that routes messages to a method conditionally.
    /// </summary>
    sealed public class ConditionalMethodRouter : IRouter
    {
        /// <summary>
        /// Create a conditional router targeting the specified method.
        /// </summary>
        /// <param name="condition">Condition that must be met for messages to be routed.</param>
        /// <param name="action">Action to run if the condition is met.</param>
        public ConditionalMethodRouter(Func<INativeMessage, bool> condition, Action<INativeMessage> action)
        {
            Action = action;
            Condition = condition;
        }

        /// <summary>
        /// Method to route messages to if the condition is met.
        /// </summary>
        public readonly Action<INativeMessage> Action;

        /// <summary>
        /// Condition to check messages against. Returns true if met.
        /// </summary>
        public readonly Func<INativeMessage, bool> Condition;

        /// <summary>
        /// If the condition is met, route the message and return true. Returns false otherwise.
        /// </summary>
        /// <param name="message">Message to route.</param>
        /// <returns>True if the message was handled.</returns>
        public bool Route(INativeMessage message)
        {
            if (Condition(message))
            {
                if (Action != null)
                {
                    Action(message);                    
                }
                return true;                
            }
            return false;
        }
    }
}
