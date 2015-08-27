using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Cursors.Dividers;

namespace NextLevelSeven.Cursors
{
    /// <summary>
    /// Represents the special field at MSH-2, which contains encoding characters for a message.
    /// </summary>
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
            return new EncodingField(this);
        }

        protected override IStringDivider GetDescendantDivider(Element ancestor, int index)
        {
            return new ProxyStringDivider(() => Ancestor.DescendantDivider[1], v => Ancestor.DescendantDivider[1] = v);
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
            get { return Ancestor.DescendantDivider[1]; }
            set
            {
                var s = Ancestor.DescendantDivider.Value;
                var builder = new StringBuilder();
                builder.Append(s.Substring(0, 3));
                builder.Append(value);
                builder.Append(s.Substring(4));
                Ancestor.DescendantDivider.Value = builder.ToString();
            }
        }    
    }
}
