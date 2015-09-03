namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Provides information about the characters used to encode an HL7 message.
    /// </summary>
    internal class EncodingConfiguration
    {
        /// <summary>
        ///     Create a default encoding configuration.
        /// </summary>
        public EncodingConfiguration()
        {
            Initialize();
        }

        /// <summary>
        ///     Clone an existing encoding configuration.
        /// </summary>
        /// <param name="other">Source configuration to pull values from.</param>
        public EncodingConfiguration(EncodingConfiguration other)
        {
            InitializeFrom(other);
        }

        /// <summary>
        ///     Get the delimiter character used to split components.
        /// </summary>
        public virtual char ComponentDelimiter { get; protected set; }

        /// <summary>
        ///     Get the escape character used to mark encoded sequences.
        /// </summary>
        public virtual char EscapeDelimiter { get; protected set; }

        /// <summary>
        /// Get the escape character used to separate fields.
        /// </summary>
        public virtual char FieldDelimiter { get; protected set; }

        /// <summary>
        ///     Get the repetition character used to separate multiple data in the same field.
        /// </summary>
        public virtual char RepetitionDelimiter { get; protected set; }

        /// <summary>
        ///     Get the delimiter character used to split subcomponents.
        /// </summary>
        public virtual char SubcomponentDelimiter { get; protected set; }

        /// <summary>
        ///     Initialize defaults.
        /// </summary>
        protected virtual void Initialize()
        {
            ComponentDelimiter = '^';
            EscapeDelimiter = '\\';
            FieldDelimiter = '|';
            RepetitionDelimiter = '~';
            SubcomponentDelimiter = '&';
        }

        /// <summary>
        ///     Clone defaults from another configuration.
        /// </summary>
        /// <param name="other">Source configuration.</param>
        protected void InitializeFrom(EncodingConfiguration other)
        {
            ComponentDelimiter = other.ComponentDelimiter;
            EscapeDelimiter = other.EscapeDelimiter;
            FieldDelimiter = other.FieldDelimiter;
            RepetitionDelimiter = other.RepetitionDelimiter;
            SubcomponentDelimiter = other.SubcomponentDelimiter;
        }
    }
}