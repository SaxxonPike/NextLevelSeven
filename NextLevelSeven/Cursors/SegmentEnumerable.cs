using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors
{
    internal class SegmentEnumerable : IEnumerable<ISegment>
    {
        public SegmentEnumerable(Message message)
        {
            Element = message;
        }

        Message Element
        {
            get;
            set;
        }

        public IEnumerator<ISegment> GetEnumerator()
        {
            return new ElementEnumerator<ISegment>(Element.DescendantDivider, Element.GetSegment);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join("|", this.Select(e => e.ToString()));
        }
    }
}
