using NextLevelSeven.Core;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building
{
    /// <summary>
    ///     Base interface for element builders.
    /// </summary>
    public interface IBuilder : IElement
    {
        /// <summary>
        ///     Get or set the character used to separate component-level content.
        /// </summary>
        char ComponentDelimiter { get; set; }

        /// <summary>
        ///     Get or set the character used to signify escape sequences.
        /// </summary>
        char EscapeDelimiter { get; set; }

        /// <summary>
        ///     Get or set the character used to separate fields.
        /// </summary>
        char FieldDelimiter { get; set; }

        /// <summary>
        ///     Get or set the character used to separate field repetition content.
        /// </summary>
        char RepetitionDelimiter { get; set; }

        /// <summary>
        ///     Get or set the character used to separate subcomponent-level content.
        /// </summary>
        char SubcomponentDelimiter { get; set; }
    }
}