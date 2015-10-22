using System.Collections.Generic;
using NextLevelSeven.Core;

namespace NextLevelSeven.Parsing
{
    /// <summary>A parser for HL7 messages at the field level.</summary>
    public interface IFieldParser : IElementParser, IField
    {
        /// <summary>Get the ancestor segment. Null if the element is an orphan.</summary>
        new ISegmentParser Ancestor { get; }

        /// <summary>
        ///     Get a descendant field repetition at the specified index. Indices match the HL7 specification, and are not
        ///     necessarily zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        new IRepetitionParser this[int index] { get; }

        /// <summary>Get all field repetitions.</summary>
        new IEnumerable<IRepetitionParser> Repetitions { get; }
    }
}