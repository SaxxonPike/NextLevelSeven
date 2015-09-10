using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Enumerable interface for segment descendants.
    /// </summary>
    internal sealed class NativeSegmentEnumerable : IEnumerable<INativeSegment>
    {
        public NativeSegmentEnumerable(NativeMessage message)
        {
            Element = message;
        }

        private NativeMessage Element { get; set; }

        public IEnumerator<INativeSegment> GetEnumerator()
        {
            return new NativeElementEnumerator<INativeSegment>(Element.DescendantDivider, Element.GetSegment);
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