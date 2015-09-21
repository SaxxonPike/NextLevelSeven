using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Routing;

namespace NextLevelSeven.Native
{
    /// <summary>
    ///     Extends the functionality of top-level HL7 message elements.
    /// </summary>
    public static class NativeMessageExtensions
    {
        /// <summary>
        ///     Get segments that start with the specified segment type, and include all segments below until the next split.
        /// </summary>
        /// <param name="message">Message to get segments from.</param>
        /// <param name="segmentType">Segment type to split by.</param>
        /// <param name="includeExtras">If true, include the extra split data at the beginning.</param>
        /// <returns>Segment sets that start with the specified segment type.</returns>
        public static IEnumerable<IEnumerable<ISegmentParser>> SplitSegments(this IMessageParser message,
            string segmentType,
            bool includeExtras = false)
        {
            var result = new List<IEnumerable<ISegmentParser>>();
            var currentSegments = new List<ISegmentParser>();
            var skipSegmentGroup = !includeExtras;
            var index = 1;

            foreach (var segment in message.Value.Split(message.Delimiter))
            {
                if (segment.Length > 3)
                {
                    var thisSegmentType = segment.Substring(0, 3);
                    if (thisSegmentType == segmentType)
                    {
                        if (currentSegments.Count > 0 && !skipSegmentGroup)
                        {
                            result.Add(currentSegments);
                        }
                        currentSegments = new List<ISegmentParser>();
                        skipSegmentGroup = false;
                    }
                    currentSegments.Add(message[index]);
                }
                index++;
            }

            if (currentSegments.Count > 0)
            {
                result.Add(currentSegments);
            }

            return result;
        }

        /// <summary>
        ///     Get segments that start with any of the specified segment types, and include all segments below until the next
        ///     split.
        /// </summary>
        /// <param name="message">Message to get segments from.</param>
        /// <param name="segmentTypes">Segment types to split by.</param>
        /// <param name="includeExtras">If true, include the extra split data at the beginning.</param>
        /// <returns>Segment sets that start with one of the specified segment types.</returns>
        public static IEnumerable<IEnumerable<ISegmentParser>> SplitSegments(this IMessageParser message,
            IEnumerable<string> segmentTypes, bool includeExtras = false)
        {
            return segmentTypes.SelectMany(s => SplitSegments(message, s, includeExtras));
        }
    }
}