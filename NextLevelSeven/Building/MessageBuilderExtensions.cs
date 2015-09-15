using NextLevelSeven.Native;
using NextLevelSeven.Native.Elements;

namespace NextLevelSeven.Building
{
    /// <summary>
    ///     Extensions to the IMessageBuilder interface.
    /// </summary>
    public static class MessageBuilderExtensions
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
    }
}