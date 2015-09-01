namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Common interface for the second depth level of an HL7 message, which contains segment type information.
    /// </summary>
    public interface ISegment : IElement
    {
        /// <summary>
        ///     Get or set the three-character segment type.
        /// </summary>
        string Type { get; set; }

        /// <summary>
        ///     Create a detached clone of the segment with no ancestors.
        /// </summary>
        /// <returns></returns>
        new ISegment CloneDetached();
    }
}