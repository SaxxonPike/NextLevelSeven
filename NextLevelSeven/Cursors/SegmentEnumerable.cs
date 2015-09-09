using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors
{
    /// <summary>
    ///     Enumerable interface for segment descendants.
    /// </summary>
    internal sealed class SegmentEnumerable : IEnumerable<INativeSegment>
    {
        public SegmentEnumerable(Message message)
        {
            Element = message;
        }

        private Message Element { get; set; }

        public IEnumerator<INativeSegment> GetEnumerator()
        {
            return new ElementEnumerator<INativeSegment>(Element.DescendantDivider, Element.GetSegment);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join("|", this.Select(e => e.ToString()));
        }
    }
}