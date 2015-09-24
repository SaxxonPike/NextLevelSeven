using System.Collections.Generic;
using NextLevelSeven.Core;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>
    ///     Base class for builders that are not root level.
    /// </summary>
    internal abstract class BuilderBaseDescendant : BuilderBase
    {
        /// <summary>
        ///     Initialize the message builder base class.
        /// </summary>
        /// <param name="ancestor">Ancestor from which configuration will be obtained.</param>
        /// <param name="index">Index in the parent.</param>
        internal BuilderBaseDescendant(BuilderBase ancestor, int index)
            : base(ancestor.Encoding, index)
        {
            Ancestor = ancestor;
        }

        /// <summary>
        ///     Get the ancestor builder.
        /// </summary>
        protected BuilderBase Ancestor { get; private set; }

        /// <summary>
        ///     Get or set the component delimiter character.
        /// </summary>
        public override sealed char ComponentDelimiter
        {
            get { return Ancestor.ComponentDelimiter; }
            set { Ancestor.ComponentDelimiter = value; }
        }

        /// <summary>
        ///     Get or set the escape delimiter character.
        /// </summary>
        public override sealed char EscapeCharacter
        {
            get { return Ancestor.EscapeCharacter; }
            set { Ancestor.EscapeCharacter = value; }
        }

        /// <summary>
        ///     Get or set the field delimiter character.
        /// </summary>
        public override sealed char FieldDelimiter
        {
            get { return Ancestor.FieldDelimiter; }
            set { Ancestor.FieldDelimiter = value; }
        }

        /// <summary>
        ///     Get or set the repetition delimiter character.
        /// </summary>
        public override sealed char RepetitionDelimiter
        {
            get { return Ancestor.RepetitionDelimiter; }
            set { Ancestor.RepetitionDelimiter = value; }
        }

        /// <summary>
        ///     Get or set the subcomponent delimiter character.
        /// </summary>
        public override sealed char SubcomponentDelimiter
        {
            get { return Ancestor.SubcomponentDelimiter; }
            set { Ancestor.SubcomponentDelimiter = value; }
        }

        /// <summary>
        ///     Get or set the builder's value.
        /// </summary>
        public abstract override string Value { get; set; }

        /// <summary>
        ///     Get or set the builder's sub-values.
        /// </summary>
        public abstract override IEnumerable<string> Values { get; set; }

        /// <summary>
        ///     Get the ancestor element.
        /// </summary>
        /// <returns></returns>
        protected override IElement GetAncestor()
        {
            return Ancestor;
        }

        /// <summary>
        ///     Deep clone this builder.
        /// </summary>
        /// <returns>Cloned builder.</returns>
        public abstract override IElement Clone();
    }
}