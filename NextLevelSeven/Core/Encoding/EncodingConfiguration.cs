namespace NextLevelSeven.Core.Encoding
{
    /// <summary>
    ///     Provides information about the characters used to encode an HL7 message.
    /// </summary>
    internal class EncodingConfiguration
    {
        /// <summary>
        ///     Default encoding configuration.
        /// </summary>
        public static readonly EncodingConfiguration Default = new EncodingConfiguration();

        /// <summary>
        ///     An encoding configuration with all characters set to zero.
        /// </summary>
        public static readonly EncodingConfiguration Empty = new EncodingConfiguration('\0', '\0', '\0', '\0', '\0');

        /// <summary>
        ///     Create a default encoding configuration.
        /// </summary>
        public EncodingConfiguration()
        {
            InitializeCtor();
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
        ///     Create an encoding configuration with the specified characters.
        /// </summary>
        /// <param name="field">Field delimiter.</param>
        /// <param name="repetition">Repetition delimiter.</param>
        /// <param name="component">Component delimiter.</param>
        /// <param name="subcomponent">Subcomponent delimiter.</param>
        /// <param name="escape">Escape character.</param>
        public EncodingConfiguration(char field, char repetition, char component, char subcomponent, char escape)
        {
            InitializeCtor(field, repetition, component, subcomponent, escape);
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
        ///     Get the escape character used to separate fields.
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
        ///     Initialize defaults (to be called from constructor.)
        /// </summary>
        private void InitializeCtor()
        {
            Initialize();
        }

        /// <summary>
        ///     Initialize defaults.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="repetition"></param>
        /// <param name="component"></param>
        /// <param name="subcomponent"></param>
        /// <param name="escape"></param>
        private void InitializeCtor(char field, char repetition, char component, char subcomponent, char escape)
        {
            InitializeCtor();
            FieldDelimiter = field;
            RepetitionDelimiter = repetition;
            ComponentDelimiter = component;
            SubcomponentDelimiter = subcomponent;
            EscapeDelimiter = escape;
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