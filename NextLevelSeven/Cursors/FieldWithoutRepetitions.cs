using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors
{
    sealed internal class FieldWithoutRepetitions : Element
    {
        public FieldWithoutRepetitions(Element ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        protected override char Delimiter
        {
            get { return '\0'; }
        }

        public override int DescendantCount
        {
            get
            {
                return 1;
            }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return Ancestor.EncodingConfiguration; }
        }

        public override IElement GetDescendant(int index)
        {
            return new Repetition(this, index - 1, index);
        }
    }
}
