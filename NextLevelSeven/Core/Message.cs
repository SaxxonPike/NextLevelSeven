using NextLevelSeven.Building;
using NextLevelSeven.Building.Elements;
using NextLevelSeven.Native;
using NextLevelSeven.Native.Elements;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Factory class. A starting point for all HL7 message related activities.
    /// </summary>
    public static class Message
    {
        /// <summary>
        ///     Create a message builder with a default MSH.
        /// </summary>
        /// <returns>New message builder.</returns>
        public static IMessageBuilder Build()
        {
            return new MessageBuilder();
        }

        /// <summary>
        ///     Create a message builder, initialized with the specified message data.
        /// </summary>
        /// <param name="message">Message data.</param>
        /// <returns>New message builder.</returns>
        public static IMessageBuilder Build(string message)
        {
            return new MessageBuilder(message);
        }

        /// <summary>
        ///     Create a message builder, initialized with the specified message.
        /// </summary>
        /// <param name="message">Message data.</param>
        /// <returns>New message builder.</returns>
        public static IMessageBuilder Build(INativeMessage message)
        {
            return new MessageBuilder(message);
        }

        /// <summary>
        ///     Create a new message with a default MSH.
        /// </summary>
        /// <returns>New message.</returns>
        public static INativeMessage Create()
        {
            return new NativeMessage();
        }

        /// <summary>
        ///     Create a new message, initialized with the specified message data.
        /// </summary>
        /// <param name="message">Message data.</param>
        /// <returns>New message builder.</returns>
        public static INativeMessage Create(string message)
        {
            return new NativeMessage(message);
        }
    }
}