using NextLevelSeven.Core.Encoding;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     An encoding configuration that gets its values from an HL7 message.
    /// </summary>
    internal sealed class NativeMessageEncodingConfiguration : ProxyEncodingConfiguration
    {
        /// <summary>
        ///     Create an encoding configuration from a message or segment.
        /// </summary>
        /// <param name="messageElement">Message or segment to pull the characters from.</param>
        public NativeMessageEncodingConfiguration(INativeElement messageElement)
            : base(
                () => messageElement.Value[3], () => messageElement.Value[6], () => messageElement.Value[5],
                () => messageElement.Value[4], () => messageElement.Value[7])
        {
        }
    }
}