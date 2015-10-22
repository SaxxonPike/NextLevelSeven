using System.Collections.Generic;
using NextLevelSeven.Core;

namespace NextLevelSeven.Parsing
{
    /// <summary>Common interface for the highest level element in an HL7 message: the message itself.</summary>
    public interface IMessageParser : IElementParser, IMessage
    {
        /// <summary>
        ///     Get a descendant segment at the specified index. Indices match the HL7 specification, and are not necessarily
        ///     zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        new ISegmentParser this[int index] { get; }

        /// <summary>Get all segments in the message.</summary>
        new IEnumerable<ISegmentParser> Segments { get; }

        /// <summary>Check for validity of the message. Returns true if the message can reasonably be parsed.</summary>
        /// <returns>True if the message can be parsed, false otherwise.</returns>
        bool Validate();
    }
}