using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors
{
    internal class Field : Element
    {
        public Field(Element ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        protected override char Delimiter
        {
            get { return EncodingConfiguration.RepetitionDelimiter; }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return Ancestor.EncodingConfiguration; }
        }

        public override IElement GetDescendant(int index)
        {
            if (index <= 0)
            {
                return new Repetition(new FieldWithoutRepetitions(Ancestor, ParentIndex, Index), 0, 0);
            }
            return new Repetition(this, index - 1, index);
        }
    }
}
