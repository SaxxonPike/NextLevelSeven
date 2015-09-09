using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Native;

namespace NextLevelSeven.Cursors
{
    /// <summary>
    ///     Enumerable interface for element descendants.
    /// </summary>
    internal sealed class ElementEnumerable : IEnumerable<INativeElement>
    {
        public ElementEnumerable(Element element)
        {
            Element = element;
        }

        private Element Element { get; set; }

        public IEnumerator<INativeElement> GetEnumerator()
        {
            return new ElementEnumerator<INativeElement>(Element.DescendantDivider, Element.GetDescendant);
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