using System.Diagnostics.CodeAnalysis;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>An encoding configuration that gets its values from an HL7 message.</summary>
    internal sealed class ParserEncodingConfiguration : ReadOnlyEncodingConfiguration
    {
        /// <summary>Base segment to pull values from.</summary>
        private readonly ISegment _segment;

        /// <summary>Create an encoding configuration from a message or segment.</summary>
        /// <param name="segment">Field to pull the characters from.</param>
        public ParserEncodingConfiguration(ISegment segment)
        {
            _segment = segment;
        }

        /// <summary>Get the encoding component delimiter from MSH-2.</summary>
        public override char ComponentDelimiter
        {
            get { return TryGetChar(_segment[2].Value, 0); }
            [ExcludeFromCodeCoverage] protected set { }
        }

        /// <summary>Get the encoding escape character from MSH-2.</summary>
        public override char EscapeCharacter
        {
            get { return TryGetChar(_segment[2].Value, 2); }
            [ExcludeFromCodeCoverage] protected set { }
        }

        /// <summary>Get the encoding field delimiter from the fourth character in the message.</summary>
        public override char FieldDelimiter
        {
            // important: do not change this to Segment[1].Value due to an infinite call loop.
            get { return TryGetChar(_segment.Ancestor.Value, 3); }
            [ExcludeFromCodeCoverage] protected set { }
        }

        /// <summary>Get the encoding repetition delimiter from MSH-2.</summary>
        public override char RepetitionDelimiter
        {
            get { return TryGetChar(_segment[2].Value, 1); }
            [ExcludeFromCodeCoverage] protected set { }
        }

        /// <summary>Get the encoding subcomponent character from MSH-2.</summary>
        public override char SubcomponentDelimiter
        {
            get { return TryGetChar(_segment[2].Value, 3); }
            [ExcludeFromCodeCoverage] protected set { }
        }

        /// <summary>Attempt to get a character from the specified string and index, and return 0 if not present.</summary>
        /// <param name="value">String to check.</param>
        /// <param name="index">Index within the specified string.</param>
        /// <returns></returns>
        private static char TryGetChar(string value, int index)
        {
            if (index < 0 || index >= value.Length)
            {
                return '\0';
            }
            return value[index];
        }
    }
}