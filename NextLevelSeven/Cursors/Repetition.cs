using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors
{
    sealed internal class Repetition : Element
    {
        public Repetition(Element ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        protected override char Delimiter
        {
            get { return EncodingConfiguration.ComponentDelimiter; }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return Ancestor.EncodingConfiguration; }
        }

        public override IElement GetDescendant(int index)
        {
            return new Component(this, index - 1, index);
        }
    }
}
