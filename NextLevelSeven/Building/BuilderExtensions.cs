using NextLevelSeven.Building.Elements;

namespace NextLevelSeven.Building
{
    /// <summary>
    ///     Extensions to the IBuilder based interfaces.
    /// </summary>
    public static class BuilderExtensions
    {
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