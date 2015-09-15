using System.Collections.Generic;
using NextLevelSeven.Core;

namespace NextLevelSeven.Building
{
    /// <summary>
    ///     Base class for message builders.
    /// </summary>
    internal abstract class BuilderBase : IElement
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
            Index = 0;
        }

        /// <summary>
        ///     Initialize the message builder base class.
        /// </summary>
        /// <param name="config">Message's encoding configuration.</param>
        /// <param name="index">Index in the parent.</param>
        internal BuilderBase(EncodingConfiguration config, int index)
        {
            EncodingConfiguration = config;
            Index = index;
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

        public int Index { get; private set; }

        public abstract IElement Clone();

        public abstract string Value { get; set; }

        public abstract IEnumerable<string> Values { get; set; }

        public abstract IEncodedTypeConverter As { get; }

        public abstract int ValueCount { get; }

        public abstract char Delimiter { get; }

        public IElement this[int index]
        {
            get { return GetGenericElement(index); }
        }

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

        protected abstract IElement GetGenericElement(int index);
    }
}