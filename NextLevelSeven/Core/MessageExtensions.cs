using System.Collections.Generic;

namespace NextLevelSeven.Core
{
    /// <summary>Extensions to the IMessage interface.</summary>
    public static class MessageExtensions
    {
        /// <summary>Get segments that start with the specified segment type, and include all segments below until the next split.</summary>
        /// <param name="message">Message to get segments from.</param>
        /// <param name="segmentType">Segment type to split by.</param>
        /// <param name="includeExtras">If true, include the extra split data at the beginning.</param>
        /// <returns>Segment sets that start with the specified segment type.</returns>
        public static IEnumerable<IEnumerable<ISegment>> SplitSegments(this IMessage message, string segmentType,
            bool includeExtras = false)
        {
            var currentSegments = new List<ISegment>();
            var skipSegmentGroup = !includeExtras;

            foreach (var segment in message.Segments)
            {
                if (segment.Type == segmentType)
                {
                    if (currentSegments.Count > 0 && !skipSegmentGroup)
                    {
                        yield return currentSegments;
                    }
                    currentSegments = new List<ISegment>();
                    skipSegmentGroup = false;
                }
                currentSegments.Add(segment);
            }

            if (currentSegments.Count > 0)
            {
                yield return currentSegments;
            }
        }
    }
}