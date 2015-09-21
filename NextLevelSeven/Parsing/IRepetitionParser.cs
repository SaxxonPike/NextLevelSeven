using System.Collections.Generic;
using NextLevelSeven.Core;

namespace NextLevelSeven.Native
{
    public interface IRepetitionParser : IElementParser, IRepetition
    {
        /// <summary>
        ///     Get a descendant component at the specified index. Indices match the HL7 specification, and are not necessarily
        ///     zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        new IComponentParser this[int index] { get; }

        /// <summary>
        ///     Get all components.
        /// </summary>
        new IEnumerable<IComponentParser> Components { get; }
    }
}