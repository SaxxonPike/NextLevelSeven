namespace NextLevelSeven.Building
{
    /// <summary>
    ///     Base class for builders that are not root level.
    /// </summary>
    internal abstract class BuilderBaseDescendant : BuilderBase
    {
        /// <summary>
        ///     Get the ancestor builder.
        /// </summary>
        private readonly BuilderBase _ancestor;

        /// <summary>
        ///     Initialize the message builder base class.
        /// </summary>
        /// <param name="ancestor">Ancestor from which configuration will be obtained.</param>
        internal BuilderBaseDescendant(BuilderBase ancestor)
            : base(ancestor.EncodingConfiguration)
        {
            _ancestor = ancestor;
        }

        /// <summary>
        ///     Get or set the component delimiter character.
        /// </summary>
        public override char ComponentDelimiter
        {
            get { return _ancestor.ComponentDelimiter; }
            set { _ancestor.ComponentDelimiter = value; }
        }

        /// <summary>
        ///     Get or set the escape delimiter character.
        /// </summary>
        public override char EscapeDelimiter
        {
            get { return _ancestor.EscapeDelimiter; }
            set { _ancestor.EscapeDelimiter = value; }
        }

        /// <summary>
        ///     Get or set the field delimiter character.
        /// </summary>
        public override char FieldDelimiter
        {
            get { return _ancestor.FieldDelimiter; }
            set { _ancestor.FieldDelimiter = value; }
        }

        /// <summary>
        ///     Get or set the repetition delimiter character.
        /// </summary>
        public override char RepetitionDelimiter
        {
            get { return _ancestor.RepetitionDelimiter; }
            set { _ancestor.RepetitionDelimiter = value; }
        }

        /// <summary>
        ///     Get or set the subcomponent delimiter character.
        /// </summary>
        public override char SubcomponentDelimiter
        {
            get { return _ancestor.SubcomponentDelimiter; }
            set { _ancestor.SubcomponentDelimiter = value; }
        }
    }
}