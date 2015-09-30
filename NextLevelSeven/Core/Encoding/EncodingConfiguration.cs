namespace NextLevelSeven.Core.Encoding
{
    /// <summary>Provides information about the characters used to encode an HL7 message.</summary>
    internal abstract class EncodingConfiguration : IReadOnlyEncoding
    {
        /// <summary>Default encoding configuration.</summary>
        public static readonly EncodingConfiguration Default = new SimpleEncodingConfiguration();

        /// <summary>An encoding configuration with all characters set to zero.</summary>
        public static readonly EncodingConfiguration Empty = new SimpleEncodingConfiguration('\0', '\0', '\0', '\0',
            '\0');

        /// <summary>Get the delimiter character used to split components.</summary>
        public string ComponentDelimiterString
        {
            get { return new string(ComponentDelimiter, 1); }
        }

        /// <summary>Get the escape character used to mark encoded sequences.</summary>
        public string EscapeDelimiterString
        {
            get { return new string(EscapeCharacter, 1); }
        }

        /// <summary>Get the escape character used to separate fields.</summary>
        public string FieldDelimiterString
        {
            get { return new string(FieldDelimiter, 1); }
        }

        /// <summary>Get the repetition character used to separate multiple data in the same field.</summary>
        public string RepetitionDelimiterString
        {
            get { return new string(RepetitionDelimiter, 1); }
        }

        /// <summary>Get the segment delimiter used to separate segments in a message. This is non-negotiable in the HL7 standard.</summary>
        public static char SegmentDelimiter
        {
            get { return '\xD'; }
        }

        /// <summary>Get the segment delimiter used to separate segments in a message. This is non-negotiable in the HL7 standard.</summary>
        public static string SegmentDelimiterString
        {
            get { return "\xD"; }
        }

        /// <summary>Get the delimiter character used to split subcomponents.</summary>
        public string SubcomponentDelimiterString
        {
            get { return new string(SubcomponentDelimiter, 1); }
        }

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

        /// <summary>Initialize defaults.</summary>
        /// <param name="field"></param>
        /// <param name="repetition"></param>
        /// <param name="component"></param>
        /// <param name="subcomponent"></param>
        /// <param name="escape"></param>
        protected void InitializeWith(char field, char repetition, char component, char subcomponent, char escape)
        {
            FieldDelimiter = field;
            RepetitionDelimiter = repetition;
            ComponentDelimiter = component;
            SubcomponentDelimiter = subcomponent;
            EscapeCharacter = escape;
        }

        /// <summary>Clone defaults from another configuration.</summary>
        /// <param name="other">Source configuration.</param>
        protected void CopyFrom(EncodingConfiguration other)
        {
            ComponentDelimiter = other.ComponentDelimiter;
            EscapeCharacter = other.EscapeCharacter;
            FieldDelimiter = other.FieldDelimiter;
            RepetitionDelimiter = other.RepetitionDelimiter;
            SubcomponentDelimiter = other.SubcomponentDelimiter;
        }
    }
}