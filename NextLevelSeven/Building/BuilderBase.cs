using NextLevelSeven.Core;

namespace NextLevelSeven.Building
{
    /// <summary>
    ///     Base class for message builders.
    /// </summary>
    internal abstract class BuilderBase
    {
        /// <summary>
        ///     Encoding configuration for this message.
        /// </summary>
        internal readonly EncodingConfiguration EncodingConfiguration;

        /// <summary>
        ///     Initialize the message builder base class.
        /// </summary>
        internal BuilderBase()
        {
            EncodingConfiguration = new BuilderEncodingConfiguration(this);
        }

        /// <summary>
        ///     Initialize the message builder base class.
        /// </summary>
        /// <param name="config">Message's encoding configuration.</param>
        internal BuilderBase(EncodingConfiguration config)
        {
            EncodingConfiguration = config;
        }

        /// <summary>
        ///     Get or set the character used to separate component-level content.
        /// </summary>
        public virtual char ComponentDelimiter { get; set; }

        /// <summary>
        ///     Get or set the character used to signify escape sequences.
        /// </summary>
        public virtual char EscapeDelimiter { get; set; }

        /// <summary>
        ///     Get or set the character used to separate fields.
        /// </summary>
        public virtual char FieldDelimiter { get; set; }

        /// <summary>
        ///     Get or set the character used to separate field repetition content.
        /// </summary>
        public virtual char RepetitionDelimiter { get; set; }

        /// <summary>
        ///     Get or set the character used to separate subcomponent-level content.
        /// </summary>
        public virtual char SubcomponentDelimiter { get; set; }

        /// <summary>
        ///     Get an HL7 escaped string.
        /// </summary>
        /// <param name="s">String to escape.</param>
        /// <returns>Escaped string.</returns>
        public string Escape(string s)
        {
            return EncodingConfiguration.Escape(s);
        }

        /// <summary>
        ///     Get an unescaped HL7 string.
        /// </summary>
        /// <param name="s">String to unescape.</param>
        /// <returns>Unescaped string.</returns>
        public string UnEscape(string s)
        {
            return EncodingConfiguration.UnEscape(s);
        }
    }
}