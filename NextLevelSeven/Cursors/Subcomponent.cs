using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors
{
    sealed internal class Subcomponent : Element
    {
        public Subcomponent(Element ancestor, int index, int externalIndex)
            : base(ancestor, index, externalIndex)
        {
        }

        protected override char Delimiter
        {
            get { return '\0'; }
        }

        public override int DescendantCount
        {
            get { return 0; }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return Ancestor.EncodingConfiguration; }
        }

        public override IElement GetDescendant(int index)
        {
            return null;
        }
    }
}
