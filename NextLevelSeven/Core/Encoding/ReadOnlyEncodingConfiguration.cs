namespace NextLevelSeven.Core.Encoding
{
    /// <summary>Provides information about the characters used to encode an HL7 message.</summary>
    internal abstract class ReadOnlyEncodingConfiguration : IReadOnlyEncoding
    {
        /// <summary>Get the segment delimiter used to separate segments in a message. This is non-negotiable in the HL7 standard.</summary>
        public const char SegmentDelimiter = '\xD';

        /// <summary>Get the segment delimiter used to separate segments in a message. This is non-negotiable in the HL7 standard.</summary>
        public const string SegmentDelimiterString = "\xD";

        /// <summary>Get the delimiter character used to split components.</summary>
        public abstract char ComponentDelimiter { get; protected set; }

        /// <summary>Get the escape character used to mark encoded sequences.</summary>
        public abstract char EscapeCharacter { get; protected set; }

        /// <summary>Get the escape character used to separate fields.</summary>
        public abstract char FieldDelimiter { get; protected set; }

        /// <summary>Get the repetition character used to separate multiple data in the same field.</summary>
        public abstract char RepetitionDelimiter { get; protected set; }

        /// <summary>Get the delimiter character used to split subcomponents.</summary>
        public abstract char SubcomponentDelimiter { get; protected set; }

        /// <summary>Clone defaults from another configuration.</summary>
        /// <param name="other">Source configuration.</param>
        protected void CopyFrom(IReadOnlyEncoding other)
        {
            ComponentDelimiter = other.ComponentDelimiter;
            EscapeCharacter = other.EscapeCharacter;
            FieldDelimiter = other.FieldDelimiter;
            RepetitionDelimiter = other.RepetitionDelimiter;
            SubcomponentDelimiter = other.SubcomponentDelimiter;
        }
    }
}