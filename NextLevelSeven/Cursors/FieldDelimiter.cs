using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Cursors.Dividers;

namespace NextLevelSeven.Cursors
{
    sealed internal class FieldDelimiter : Element
    {
        public FieldDelimiter(Element ancestor)
            : base(ancestor, 0, 1)
        {
        }

        public override IElement CloneDetached()
        {
            return new Field(Value, EncodingConfiguration);
        }

        public override char Delimiter
        {
            get { return '\0'; }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return Ancestor.EncodingConfiguration; }
        }

        public override IElement GetDescendant(int index)
        {
            return new FieldDelimiter(this);
        }

        protected override IStringDivider GetDescendantDivider(Element ancestor, int index)
        {
            return new ProxyStringDivider(() => Value, v => Value = v);
        }

        public override bool HasSignificantDescendants
        {
            get
            {
                return false;
            }
        }

        public override string ToString()
        {
            return Value;
        }

        public override string Value
        {
            get
            {
                var value = Ancestor.DescendantDivider.Value;
                if (value != null && value.Length > 3)
                {
                    return new string(value[3], 1);
                }
                return null;
            }
            set
            {
                // TODO: change the other delimiters in the segment
                var s = Ancestor.DescendantDivider.Value;
                if (s != null && s.Length >= 3)
                {
                    Ancestor.DescendantDivider.Value = s.Substring(0, 3) + value + (s.Length > 3 ? s.Substring(4) : string.Empty);
                }
            }
        }
    }
}
