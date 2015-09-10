using System;
using NextLevelSeven.Core;
using NextLevelSeven.Native;

namespace NextLevelSeven.Web
{
    /// <summary>
    ///     Event arguments for transport-realted events.
    /// </summary>
    public class MessageTransportEventArgs : EventArgs
    {
        /// <summary>
        ///     Get the received message that is associated to the event.
        /// </summary>
        public readonly string ReceivedMessage;

        /// <summary>
        ///     Get the sent message that is associated to the event.
        /// </summary>
        public readonly string SentMessage;

        /// <summary>
        ///     Create transport event arguments with the specified message.
        /// </summary>
        /// <param name="sentMessage">Sent message to include.</param>
        /// <param name="receivedMessage">Received message to include.</param>
        public MessageTransportEventArgs(string sentMessage, string receivedMessage)
        {
            SentMessage = sentMessage;
            ReceivedMessage = receivedMessage;
        }

        public MessageTransportEventArgs(INativeMessage sentMessage, INativeMessage receivedMessage)
        {
            SentMessage = sentMessage.Value;
            ReceivedMessage = receivedMessage.Value;
        }
    }
}