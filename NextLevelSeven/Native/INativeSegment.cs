using System.Collections.Generic;
using NextLevelSeven.Core;

namespace NextLevelSeven.Native
{
    /// <summary>
    ///     Common interface for the second depth level of an HL7 message, which contains segment type information.
    /// </summary>
    public interface INativeSegment : INativeElement, ISegment
    {
        /// <summary>
        ///     Get a descendant field at the specified index. Indices match the HL7 specification, and are not necessarily
        ///     zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        new INativeField this[int index] { get; }

        /// <summary>
        ///     Get all fields.
        /// </summary>
        new IEnumerable<INativeField> Fields { get; }
    }
}