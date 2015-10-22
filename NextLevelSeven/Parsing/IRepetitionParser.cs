using System.Collections.Generic;
using NextLevelSeven.Core;

namespace NextLevelSeven.Parsing
{
    /// <summary>A parser for HL7 messages at the field repetition level.</summary>
    public interface IRepetitionParser : IElementParser, IRepetition
    {
        /// <summary>Get the ancestor field. Null if the element is an orphan.</summary>
        new IFieldParser Ancestor { get; }

        /// <summary>
        ///     Get a descendant component at the specified index. Indices match the HL7 specification, and are not
        ///     necessarily zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        new IComponentParser this[int index] { get; }

        /// <summary>Get all components.</summary>
        new IEnumerable<IComponentParser> Components { get; }
    }
}