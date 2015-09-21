namespace NextLevelSeven.Core.Encoding
{
    /// <summary>
    ///     Provides information about the characters used to encode an HL7 message.
    /// </summary>
    internal abstract class EncodingConfigurationBase
    {
        /// <summary>
        ///     Get the delimiter character used to split components.
        /// </summary>
        public abstract char ComponentDelimiter { get; protected set; }

        /// <summary>
        ///     Get the escape character used to mark encoded sequences.
        /// </summary>
        public abstract char EscapeDelimiter { get; protected set; }

        /// <summary>
        ///     Get the escape character used to separate fields.
        /// </summary>
        public abstract char FieldDelimiter { get; protected set; }

        /// <summary>
        ///     Get the repetition character used to separate multiple data in the same field.
        /// </summary>
        public abstract char RepetitionDelimiter { get; protected set; }

        /// <summary>
        ///     Get the delimiter character used to split subcomponents.
        /// </summary>
        public abstract char SubcomponentDelimiter { get; protected set; }

        /// <summary>
        ///     Initialize defaults.
        /// </summary>
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
            EscapeDelimiter = escape;
        }

        /// <summary>
        ///     Clone defaults from another configuration.
        /// </summary>
        /// <param name="other">Source configuration.</param>
        protected void CopyFrom(EncodingConfigurationBase other)
        {
            ComponentDelimiter = other.ComponentDelimiter;
            EscapeDelimiter = other.EscapeDelimiter;
            FieldDelimiter = other.FieldDelimiter;
            RepetitionDelimiter = other.RepetitionDelimiter;
            SubcomponentDelimiter = other.SubcomponentDelimiter;
        }
    }
}