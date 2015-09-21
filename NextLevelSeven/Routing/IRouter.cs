using NextLevelSeven.Core;

namespace NextLevelSeven.Routing
{
    /// <summary>
    ///     Interface for a message router.
    /// </summary>
    public interface IRouter
    {
        /// <summary>
        ///     Route a message. Returns true if the message was handled or rerouted.
        /// </summary>
        /// <param name="message">Message to route.</param>
        /// <returns>Whether or not the message was handled or rerouted.</returns>
        bool Route(IMessage message);
    }
}