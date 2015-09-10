using NextLevelSeven.Core;

namespace NextLevelSeven.Building
{
    /// <summary>
    /// Base class for message builders.
    /// </summary>
    abstract internal class BuilderBase
    {
        /// <summary>
        /// Initialize the message builder base class.
        /// </summary>
        internal BuilderBase()
        {
            EncodingConfiguration = new BuilderEncodingConfiguration(this);
        }

        /// <summary>
        /// Initialize the message builder base class.
        /// </summary>
        /// <param name="config">Message's encoding configuration.</param>
        internal BuilderBase(EncodingConfiguration config)
        {
            EncodingConfiguration = config;
        }

        /// <summary>
        ///     Encoding configuration for this message.
        /// </summary>
        internal readonly EncodingConfiguration EncodingConfiguration;

        /// <summary>
        ///     Get or set the character used to separate component-level content.
        /// </summary>
        virtual public char ComponentDelimiter { get; set; }

        /// <summary>
        ///     Get or set the character used to signify escape sequences.
        /// </summary>
        virtual public char EscapeDelimiter { get; set; }

        /// <summary>
        /// Get or set the character used to separate fields.
        /// </summary>
        virtual public char FieldDelimiter { get; set; }

        /// <summary>
        ///     Get or set the character used to separate field repetition content.
        /// </summary>
        virtual public char RepetitionDelimiter { get; set; }

        /// <summary>
        ///     Get or set the character used to separate subcomponent-level content.
        /// </summary>
        virtual public char SubcomponentDelimiter { get; set; }

        /// <summary>
        /// Get an HL7 escaped string.
        /// </summary>
        /// <param name="s">String to escape.</param>
        /// <returns>Escaped string.</returns>
        public string Escape(string s)
        {
            return EncodingConfiguration.Escape(s);
        }

        /// <summary>
        /// Get an unescaped HL7 string.
        /// </summary>
        /// <param name="s">String to unescape.</param>
        /// <returns>Unescaped string.</returns>
        public string UnEscape(string s)
        {
            return EncodingConfiguration.UnEscape(s);
        }
    }
}
