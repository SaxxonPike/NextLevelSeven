namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>
    ///     Represents an encoding configuration that uses no delimiters.
    /// </summary>
    sealed internal class NullParserEncodingConfiguration : ParserEncodingConfiguration
    {
        /// <summary>
        ///     Default parser encoding configuration which returns null for everything.
        /// </summary>
        new public static readonly NullParserEncodingConfiguration Default = new NullParserEncodingConfiguration();

        /// <summary>
        ///     Create a new NullParserEncodingConfiguration.
        /// </summary>
        private NullParserEncodingConfiguration()
            : base(null)
        {
        }

        /// <summary>
        ///     Returns 0.
        /// </summary>
        public override char ComponentDelimiter
        {
            get { return '\0'; }
            protected set { }
        }

        /// <summary>
        ///     Returns 0.
        /// </summary>
        public override char EscapeCharacter
        {
            get { return '\0'; }
            protected set { }
        }

        /// <summary>
        ///     Returns 0.
        /// </summary>
        public override char FieldDelimiter
        {
            get { return '\0'; }
            protected set { }
        }

        /// <summary>
        ///     Returns 0.
        /// </summary>
        public override char RepetitionDelimiter
        {
            get { return '\0'; }
            protected set { }
        }

        /// <summary>
        ///     Returns 0.
        /// </summary>
        public override char SubcomponentDelimiter
        {
            get { return '\0'; }
            protected set { }
        }
    }
}
