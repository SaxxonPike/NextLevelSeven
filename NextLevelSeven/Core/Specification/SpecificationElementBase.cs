using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core.Specification
{
    abstract internal class SpecificationElementBase
    {
        protected SpecificationElementBase(IElement element)
        {
            Element = element;
        }

        public IElement Element
        {
            get;
            private set;
        }
    }
}
