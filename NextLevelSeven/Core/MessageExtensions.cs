using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    static public class MessageExtensions
    {
        /// <summary>
        /// Get segments that do not match a specific segment type.
        /// </summary>
        /// <param name="message">Message to get segments from.</param>
        /// <param name="segmentType">Segment type to filter out.</param>
        /// <returns>Segments that do not match the filtered segment type.</returns>
        static public IEnumerable<ISegment> ExcludeSegments(this IMessage message, string segmentType)
        {
            return message.Segments.Where(s => s.Type != segmentType);
        }

        /// <summary>
        /// Get segments that do not match any of the specified segment types.
        /// </summary>
        /// <param name="message">Message to get segments from.</param>
        /// <param name="segmentTypes">Segment types to filter out.</param>
        /// <returns>Segments that do not match the filtered segment types.</returns>
        static public IEnumerable<ISegment> ExcludeSegments(this IMessage message, IEnumerable<string> segmentTypes)
        {
            return message.Segments.Where(s => !segmentTypes.Contains(s.Type));
        }

        /// <summary>
        /// Get only segments that match the specified segment type.
        /// </summary>
        /// <param name="message">Message to get segments from.</param>
        /// <param name="segmentType">Segment type to get.</param>
        /// <returns>Segments that match the specified segment type.</returns>
        static public IEnumerable<ISegment> OnlySegments(this IMessage message, string segmentType)
        {
            return message.Segments.Where(s => s.Type == segmentType);
        }

        /// <summary>
        /// Get only segments that match any of the specified segment types.
        /// </summary>
        /// <param name="message">Message to get segments from.</param>
        /// <param name="segmentTypes">Segment types to get.</param>
        /// <returns>Segments that match one of the specified segment types.</returns>
        static public IEnumerable<ISegment> OnlySegments(this IMessage message, IEnumerable<string> segmentTypes)
        {
            return message.Segments.Where(s => !segmentTypes.Contains(s.Type));
        }

        /// <summary>
        /// Get segments that start with the specified segment type, and include all segments below until the next split.
        /// </summary>
        /// <param name="message">Message to get segments from.</param>
        /// <param name="segmentType">Segment type to split by.</param>
        /// <param name="includeExtras">If true, include the extra split data at the beginning.</param>
        /// <returns>Segment sets that start with the specified segment type.</returns>
        static public IEnumerable<IEnumerable<ISegment>> SplitSegments(this IMessage message, string segmentType, bool includeExtras = false)
        {
            var result = new List<IEnumerable<ISegment>>();
            var currentSegments = new List<ISegment>();
            var skipSegmentGroup = !includeExtras;

            foreach (var segment in message.Segments)
            {
                if (segment.Type == segmentType)
                {
                    if (currentSegments.Count > 0 && !skipSegmentGroup)
                    {
                        result.Add(currentSegments);
                    }
                    currentSegments = new List<ISegment>();
                    skipSegmentGroup = false;
                }
                currentSegments.Add(segment);
            }

            if (currentSegments.Count > 0)
            {
                result.Add(currentSegments);
            }

            return result;
        }

        /// <summary>
        /// Get segments that start with any of the specified segment types, and include all segments below until the next split.
        /// </summary>
        /// <param name="message">Message to get segments from.</param>
        /// <param name="segmentTypes">Segment types to split by.</param>
        /// <param name="includeExtras">If true, include the extra split data at the beginning.</param>
        /// <returns>Segment sets that start with one of the specified segment types.</returns>
        static public IEnumerable<IEnumerable<ISegment>> SplitSegments(this IMessage message, IEnumerable<string> segmentTypes, bool includeExtras = false)
        {
            return segmentTypes.SelectMany(s => SplitSegments(message, s, includeExtras));
        }
    }
}
