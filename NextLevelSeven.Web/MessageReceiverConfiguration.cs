using NextLevelSeven.Routing;

namespace NextLevelSeven.Web
{
    /// <summary>
    ///     Stores a configuration for a message receiver.
    /// </summary>
    public class MessageReceiverConfiguration : MessageTransportConfigurationBase
    {
        /// <summary>
        ///     If not null, this router will be used to process received messages. Unprocessed messages will still appear in the
        ///     queue.
        /// </summary>
        public IRouter ReceivedMessageRouter = null;
    }
}