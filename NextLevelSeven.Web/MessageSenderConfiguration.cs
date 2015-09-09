using NextLevelSeven.Routing;

namespace NextLevelSeven.Web
{
    internal class MessageSenderConfiguration : MessageTransportConfigurationBase
    {
        /// <summary>
        /// URI to send requests to.
        /// </summary>
        public string Address = null;

        /// <summary>
        /// If not null, this router will be used to process responses.
        /// </summary>
        public IRouter ResponseMessageRouter = null;
    }
}