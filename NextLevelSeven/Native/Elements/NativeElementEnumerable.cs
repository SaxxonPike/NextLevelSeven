using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Enumerable interface for element descendants.
    /// </summary>
    internal sealed class NativeElementEnumerable : IEnumerable<INativeElement>
    {
        public NativeElementEnumerable(NativeElement element)
        {
            Element = element;
        }

        private NativeElement Element { get; set; }

        public IEnumerator<INativeElement> GetEnumerator()
        {
            return new NativeElementEnumerator<INativeElement>(Element.DescendantDivider, Element.GetDescendant);
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