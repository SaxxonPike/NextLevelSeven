using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors
{
    sealed internal class ElementEnumerable : IEnumerable<IElement>
    {
        public ElementEnumerable(Element element)
        {
            Element = element;
        }

        Element Element
        {
            get;
            set;
        }

        public IEnumerator<IElement> GetEnumerator()
        {
            return new ElementEnumerator<IElement>(Element.DescendantDivider, Element.GetDescendant);
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
