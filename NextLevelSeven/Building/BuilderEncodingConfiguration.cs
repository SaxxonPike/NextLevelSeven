using NextLevelSeven.Core;

namespace NextLevelSeven.Building
{
    internal sealed class BuilderEncodingConfiguration : ProxyEncodingConfiguration
    {
        /// <summary>
        ///     Create an encoding configuration from a message or segment.
        /// </summary>
        /// <param name="builder">Message builder to pull the characters from.</param>
        public BuilderEncodingConfiguration(MessageBuilder builder)
            : base(
                () => builder.EscapeDelimiter, () => builder.RepetitionDelimiter, () => builder.ComponentDelimiter,
                () => builder.SubcomponentDelimiter)
        {
        }
    }
}