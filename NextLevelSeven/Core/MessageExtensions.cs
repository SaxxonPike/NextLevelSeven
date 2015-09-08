using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Routing;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Extends the functionality of top-level HL7 message elements.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        ///     Get segments that do not match a specific segment type.
        /// </summary>
        /// <param name="message">Message to get segments from.</param>
        /// <param name="segmentType">Segment type to filter out.</param>
        /// <returns>Segments that do not match the filtered segment type.</returns>
        public static IEnumerable<ISegment> ExcludeSegments(this IMessage message, string segmentType)
        {
            return message.Segments.Where(s => s.Type != segmentType);
        }

        /// <summary>
        ///     Get segments that do not match any of the specified segment types.
        /// </summary>
        /// <param name="message">Message to get segments from.</param>
        /// <param name="segmentTypes">Segment types to filter out.</param>
        /// <returns>Segments that do not match the filtered segment types.</returns>
        public static IEnumerable<ISegment> ExcludeSegments(this IMessage message, IEnumerable<string> segmentTypes)
        {
            return message.Segments.Where(s => !segmentTypes.Contains(s.Type));
        }

        /// <summary>
        ///     Get only segments that match the specified segment type.
        /// </summary>
        /// <param name="message">Message to get segments from.</param>
        /// <param name="segmentType">Segment type to get.</param>
        /// <returns>Segments that match the specified segment type.</returns>
        public static IEnumerable<ISegment> OnlySegments(this IMessage message, string segmentType)
        {
            return message.Segments.Where(s => s.Type == segmentType);
        }

        /// <summary>
        ///     Get only segments that match any of the specified segment types.
        /// </summary>
        /// <param name="message">Message to get segments from.</param>
        /// <param name="segmentTypes">Segment types to get.</param>
        /// <returns>Segments that match one of the specified segment types.</returns>
        public static IEnumerable<ISegment> OnlySegments(this IMessage message, IEnumerable<string> segmentTypes)
        {
            return message.Segments.Where(s => !segmentTypes.Contains(s.Type));
        }

        /// <summary>
        /// Send the message to a router.
        /// </summary>
        /// <param name="message">Message to route.</param>
        /// <param name="router">Router to route the message through.</param>
        /// <returns>If true, the router has successfully routed the message.</returns>
        public static bool RouteTo(this IMessage message, IRouter router)
        {
            return router.Route(message);
        }

        /// <summary>
        ///     Get segments that start with the specified segment type, and include all segments below until the next split.
        /// </summary>
        /// <param name="message">Message to get segments from.</param>
        /// <param name="segmentType">Segment type to split by.</param>
        /// <param name="includeExtras">If true, include the extra split data at the beginning.</param>
        /// <returns>Segment sets that start with the specified segment type.</returns>
        public static IEnumerable<IEnumerable<ISegment>> SplitSegments(this IMessage message, string segmentType,
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
        public static IEnumerable<IEnumerable<ISegment>> SplitSegments(this IMessage message,
            IEnumerable<string> segmentTypes, bool includeExtras = false)
        {
            return segmentTypes.SelectMany(s => SplitSegments(message, s, includeExtras));
        }
    }
}