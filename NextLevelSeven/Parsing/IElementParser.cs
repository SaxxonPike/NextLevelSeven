using System;
using System.Collections.Generic;
using NextLevelSeven.Core;

namespace NextLevelSeven.Parsing
{
    /// <summary>
    ///     Common interface that represents any one of the HL7v2 message constructs that holds data.
    /// </summary>
    public interface IElementParser : IElement
    {
        /// <summary>
        ///     Get a descendant element at the specified index. Indices match the HL7 specification, and are not necessarily
        ///     zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        new IElementParser this[int index] { get; }

        /// <summary>
        ///     Get all descendant elements.
        /// </summary>
        IEnumerable<IElementParser> DescendantElements { get; }

        /// <summary>
        ///     If true, the element is considered to exist in the message. This is not dependent on the value being null.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        ///     Event that is triggered whenever either this element's value
        /// </summary>
        event EventHandler ValueChanged;
    }
}