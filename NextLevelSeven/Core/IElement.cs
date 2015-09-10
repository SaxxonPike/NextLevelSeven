using System.Collections.Generic;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Represents an abstract element in an HL7 message.
    /// </summary>
    public interface IElement
    {
        /// <summary>
        ///     Get the index of the element.
        /// </summary>
        int Index { get; }

        /// <summary>
        ///     Get or set the complete value of the element.
        /// </summary>
        string Value { get; set; }

        /// <summary>
        ///     Get or set the subvalues of the element.
        /// </summary>
        IEnumerable<string> Values { get; set; }
    }
}