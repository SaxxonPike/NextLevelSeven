using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Parsing.Dividers;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>Represents a generic HL7 descendant element with an ancestor.</summary>
    internal abstract class DescendantParser : Parser
    {
        /// <summary>Internal backing store for Ancestor.</summary>
        private readonly Parser _ancestor;

        /// <summary>Create a descendant element that is detached from an ancestor.</summary>
        /// <param name="config">Encoding configuration for the element.</param>
        protected DescendantParser(EncodingConfiguration config)
            : base(config)
        {
        }

        /// <summary>Create a descendant element.</summary>
        /// <param name="ancestor">Element's ancestor.</param>
        /// <param name="parentIndex">Index within the parent.</param>
        /// <param name="externalIndex">Index exposed externally.</param>
        protected DescendantParser(Parser ancestor, int parentIndex, int externalIndex)
        {
            _ancestor = ancestor;
            ParentIndex = parentIndex;
            Index = externalIndex;
        }

        /// <summary>Create a descendant element that uses an alternative encoding configuration.</summary>
        protected DescendantParser(Parser ancestor, int parentIndex, int externalIndex, EncodingConfiguration config)
            : base(config)
        {
            _ancestor = ancestor;
            ParentIndex = parentIndex;
            Index = externalIndex;
        }

        /// <summary>Ancestor element.</summary>
        protected override sealed Parser Ancestor
        {
            get
            {
                return _ancestor;
            }
        }

        /// <summary>Zero-based index within the parent element's raw data.</summary>
        protected int ParentIndex
        {
            get;
            set;
        }

        /// <summary>Get a string divider for this descendant element.</summary>
        /// <returns>Descendant string divider.</returns>
        protected override sealed IStringDivider GetDescendantDivider()
        {
            return (Ancestor == null)
                ? base.GetDescendantDivider()
                : new StringSubDivider(Ancestor.DescendantDivider, Delimiter, ParentIndex);
        }
    }
}