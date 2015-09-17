using NextLevelSeven.Core.Encoding;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>
    ///     An encoding configuration wrapper that redirects character properties from a MessageBuilder.
    /// </summary>
    internal sealed class BuilderEncodingConfiguration : ProxyEncodingConfiguration
    {
        /// <summary>
        ///     Create an encoding configuration from a message or segment.
        /// </summary>
        /// <param name="builder">Message builder to pull the characters from.</param>
        public BuilderEncodingConfiguration(BuilderBase builder)
            : base(
                () => builder.FieldDelimiter,
                () => builder.EscapeDelimiter, () => builder.RepetitionDelimiter, () => builder.ComponentDelimiter,
                () => builder.SubcomponentDelimiter)
        {
        }
    }
}