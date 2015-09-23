using NextLevelSeven.Core.Encoding;

namespace NextLevelSeven.Parsing.Elements
{
    abstract internal class ParserBaseDescendant : ParserBase
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
            : base(ancestor)
        {
            ParentIndex = parentIndex;
            Index = externalIndex;
        }

        /// <summary>
        ///     Create a descendant element that uses an alternative encoding configuration.
        /// </summary>
        protected ParserBaseDescendant(ParserBase ancestor, int parentIndex, int externalIndex, EncodingConfigurationBase config)
            : base(ancestor, config)
        {
            ParentIndex = parentIndex;
            Index = externalIndex;
        }
    }
}
