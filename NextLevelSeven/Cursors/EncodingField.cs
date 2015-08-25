using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Cursors.Dividers;

namespace NextLevelSeven.Cursors
{
    sealed internal class EncodingField : Element
    {
        public EncodingField(Element ancestor)
            : base(ancestor, 1, 2)
        {
        }

        public override IElement CloneDetached()
        {
            return new Field(Value, EncodingConfiguration);
        }

        protected override char Delimiter
        {
            get { return '\0'; }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return Ancestor.EncodingConfiguration; }
        }

        public override IElement GetDescendant(int index)
        {
            return new EncodingField(this);
        }

        protected override IStringDivider GetDescendantDivider(Element ancestor, int index)
        {
            return new ProxyStringDivider(() => Ancestor.DescendantDivider[1], v => Ancestor.DescendantDivider[1] = v);
        }

        public override string ToString()
        {
            return Value;
        }

        public override string Value
        {
            get { return Ancestor.DescendantDivider[1]; }
            set
            {
                var s = Ancestor.DescendantDivider.Value;
                Ancestor.DescendantDivider.Value = s.Substring(0, 3) + value + s.Substring(4);
            }
        }    
    }
}
