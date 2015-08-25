using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors
{
    internal class Component : Element
    {
        public Component(Element ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        protected override char Delimiter
        {
            get { return EncodingConfiguration.SubcomponentDelimiter; }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return Ancestor.EncodingConfiguration; }
        }

        public override IElement GetDescendant(int index)
        {
            return new Subcomponent(this, index - 1, index);
        }
    }
}
