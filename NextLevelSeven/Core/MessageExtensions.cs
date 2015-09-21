using NextLevelSeven.Building;
using NextLevelSeven.Building.Elements;
using NextLevelSeven.Native;
using NextLevelSeven.Native.Elements;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Extensions to the IMessage interface.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        ///     Copy the contents of this message to a new message builder.
        /// </summary>
        /// <param name="message">Message to get data from.</param>
        /// <returns>Converted message.</returns>
        public static IMessageBuilder ToBuilder(this IMessage message)
        {
            return new MessageBuilder(message.Value);
        }

        /// <summary>
        ///     Copy the contents of this message to a new native HL7 message.
        /// </summary>
        /// <param name="message">Message to get data from.</param>
        /// <returns>Converted message.</returns>
        public static INativeMessage ToNativeMessage(this IMessage message)
        {
            return new NativeMessage(message.Value);
        }
    }
}