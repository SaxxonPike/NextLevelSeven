using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NextLevelSeven.Xml;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Extensions to the IMessage interface.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        ///     Get segments that start with the specified segment type, and include all segments below until the next split.
        /// </summary>
        /// <param name="message">Message to get segments from.</param>
        /// <param name="segmentType">Segment type to split by.</param>
        /// <param name="includeExtras">If true, include the extra split data at the beginning.</param>
        /// <returns>Segment sets that start with the specified segment type.</returns>
        public static IEnumerable<IEnumerable<ISegment>> SplitSegments(this IMessage message,
            string segmentType,
            bool includeExtras = false)
        {
            var result = new List<IEnumerable<ISegment>>();
            var currentSegments = new List<ISegment>();
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
                        currentSegments = new List<ISegment>();
                        skipSegmentGroup = false;
                    }
                    currentSegments.Add((ISegment) message[index]);
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
        public static IEnumerable<IEnumerable<ISegment>> SplitSegments(this IMessage message,
            IEnumerable<string> segmentTypes, bool includeExtras = false)
        {
            return segmentTypes.SelectMany(s => SplitSegments(message, s, includeExtras));
        }

        /// <summary>
        ///     Convert a message to XML.
        /// </summary>
        /// <param name="message">Message to convert.</param>
        /// <returns>Converted message.</returns>
        public static XmlDocument ToXml(this IMessage message)
        {
            return V2Xml.ConvertToXml(message);
        }
    }
}