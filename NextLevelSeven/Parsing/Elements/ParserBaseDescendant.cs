using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Parsing.Dividers;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>
    ///     Represents a generic HL7 descendant element with an ancestor.
    /// </summary>
    internal abstract class ParserBaseDescendant : ParserBase
    {
        /// <summary>
        ///     Create a descendant element that is detached from an ancestor.
        /// </summary>
        /// <param name="config">Encoding configuration for the element.</param>
        protected ParserBaseDescendant(EncodingConfigurationBase config)
            : base(config)
        {
        }

        /// <summary>
        ///     Create a descendant element.
        /// </summary>
        /// <param name="ancestor">Element's ancestor.</param>
        /// <param name="parentIndex">Index within the parent.</param>
        /// <param name="externalIndex">Index exposed externally.</param>
        protected ParserBaseDescendant(ParserBase ancestor, int parentIndex, int externalIndex)
        {
            _ancestor = ancestor;
            ParentIndex = parentIndex;
            Index = externalIndex;
        }

        /// <summary>
        ///     Create a descendant element that uses an alternative encoding configuration.
        /// </summary>
        protected ParserBaseDescendant(ParserBase ancestor, int parentIndex, int externalIndex,
            EncodingConfigurationBase config)
            : base(config)
        {
            _ancestor = ancestor;
            ParentIndex = parentIndex;
            Index = externalIndex;
        }

        /// <summary>
        ///     Internal backing store for Ancestor.
        /// </summary>
        private readonly ParserBase _ancestor;

        /// <summary>
        ///     Ancestor element.
        /// </summary>
        sealed protected override ParserBase Ancestor
        {
            get { return _ancestor; }
        }

        /// <summary>
        ///     Get a string divider for this descendant element.
        /// </summary>
        /// <returns>Descendant string divider.</returns>
        sealed protected override IStringDivider GetDescendantDivider()
        {
            return (Ancestor == null)
                ? base.GetDescendantDivider()
                : new StringSubDivider(Ancestor.DescendantDivider, Delimiter, ParentIndex);
        }

        /// <summary>
        ///     Zero-based index within the parent element's raw data.
        /// </summary>
        protected int ParentIndex { get; set; }
    }
}