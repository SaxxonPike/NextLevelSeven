using NextLevelSeven.Building.Elements;
using NextLevelSeven.Native;
using NextLevelSeven.Native.Elements;

namespace NextLevelSeven.Building
{
    /// <summary>
    ///     Extensions to the IBuilder based interfaces.
    /// </summary>
    public static class BuilderExtensions
    {
        /// <summary>
        ///     Copy the contents of this builder to an HL7 message.
        /// </summary>
        /// <param name="builder">Builder to get data from.</param>
        /// <returns>Converted message.</returns>
        public static INativeMessage ToNativeMessage(this IMessageBuilder builder)
        {
            return new NativeMessage(builder.Value);
        }

        /// <summary>
        ///     Deep clone the message builder.
        /// </summary>
        /// <param name="builder">Builder to clone.</param>
        /// <returns>Clone of the builder.</returns>
        public static IMessageBuilder Clone(this IMessageBuilder builder)
        {
            return new MessageBuilder(builder);
        }
    }
}