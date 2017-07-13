using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>Base class for builders that are not root level.</summary>
    internal abstract class DescendantBuilder : Builder
    {
        /// <summary>Get the ancestor builder.</summary>
        protected readonly Builder Ancestor;

        /// <summary>Initialize the message builder base class.</summary>
        /// <param name="ancestor">Ancestor from which configuration will be obtained.</param>
        /// <param name="index">Index in the parent.</param>
        internal DescendantBuilder(Builder ancestor, int index)
            : base(ancestor.Encoding, index)
        {
            Ancestor = ancestor;
        }

        /// <summary>Initialize the message builder base class.</summary>
        /// <param name="config">Configuration to use.</param>
        /// <param name="index">Index for new builder.</param>
        protected DescendantBuilder(IEncoding config, int index)
            : base(config, index)
        {
            Ancestor = null;
        }

        /// <summary>Get or set the component delimiter character.</summary>
        public sealed override char ComponentDelimiter
        {
            get => Encoding.ComponentDelimiter;
            set => Encoding.ComponentDelimiter = value;
        }

        /// <summary>Get or set the escape delimiter character.</summary>
        public sealed override char EscapeCharacter
        {
            get => Encoding.EscapeCharacter;
            set => Encoding.EscapeCharacter = value;
        }

        /// <summary>Get or set the field delimiter character.</summary>
        public sealed override char FieldDelimiter
        {
            get => Encoding.FieldDelimiter;
            set => Encoding.FieldDelimiter = value;
        }

        /// <summary>Get or set the repetition delimiter character.</summary>
        public sealed override char RepetitionDelimiter
        {
            get => Encoding.RepetitionDelimiter;
            set => Encoding.RepetitionDelimiter = value;
        }

        /// <summary>Get or set the subcomponent delimiter character.</summary>
        public sealed override char SubcomponentDelimiter
        {
            get => Encoding.SubcomponentDelimiter;
            set => Encoding.SubcomponentDelimiter = value;
        }

        /// <summary>Get or set the builder's value.</summary>
        public abstract override string Value { get; set; }

        /// <summary>Get or set the builder's sub-values.</summary>
        public abstract override IEnumerable<string> Values { get; set; }

        /// <summary>Get the ancestor element.</summary>
        /// <returns></returns>
        protected override IElement GetAncestor()
        {
            return Ancestor;
        }

        /// <summary>Deep clone this builder.</summary>
        /// <returns>Cloned builder.</returns>
        public abstract override IElement Clone();
    }
}