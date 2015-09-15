using NextLevelSeven.Core;

namespace NextLevelSeven.Native
{
    public interface INativeRepetition : INativeElement, IRepetition
    {
        /// <summary>
        ///     Get a descendant component at the specified index. Indices match the HL7 specification, and are not necessarily
        ///     zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        new INativeComponent this[int index] { get; }
    }
}