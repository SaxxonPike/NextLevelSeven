using System.Collections.Generic;
using NextLevelSeven.Core;

namespace NextLevelSeven.Parsing
{
    /// <summary>Common interface for the second depth level of an HL7 message, which contains segment type information.</summary>
    public interface ISegmentParser : IElementParser, ISegment
    {
        /// <summary>Get the ancestor message. Null if the element is an orphan.</summary>
        new IMessageParser Ancestor { get; }

        /// <summary>
        ///     Get a descendant field at the specified index. Indices match the HL7 specification, and are not necessarily
        ///     zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        new IFieldParser this[int index] { get; }

        /// <summary>Get all fields.</summary>
        new IEnumerable<IFieldParser> Fields { get; }
    }
}