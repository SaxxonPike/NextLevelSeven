using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors
{
    sealed internal class Segment : Element, ISegment
    {
        public Segment(Element ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        protected override char Delimiter
        {
            get
            {
                if (DescendantDivider == null)
                {
                    return Ancestor.DescendantDivider.Value[3];
                }
                return Value[3];
            }
        }

        public override int DescendantCount
        {
            get
            {
                if (string.Equals(Type, "MSH", StringComparison.Ordinal))
                {
                    return DescendantDivider.Count + 1;
                }
                return DescendantDivider.Count;
            }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return Ancestor.EncodingConfiguration; }
        }

        public override IElement GetDescendant(int index)
        {
            if (string.Equals(Type, "MSH", StringComparison.Ordinal))
            {
                if (index == 1)
                {
                    var descendant = new FieldDelimiter(this);
                    return descendant;
                }

                if (index == 2)
                {
                    var descendant = new EncodingField(this);
                    return descendant;
                }

                if (index > 2)
                {
                    var descendant = new Field(this, index - 1, index);
                    return descendant;
                }                
            }
            return new Field(this, index, index);
        }

        public override string Key
        {
            get
            {
                var index = ((Message)Ancestor).Segments
                    .Where(s => s.Type == Type)
                    .Select(e => e.Index)
                    .ToList()
                    .IndexOf(Index) + 1;
                return Type + index;
            }
        }

        public string Type
        {
            get { return DescendantDivider[0]; }
            set { DescendantDivider[0] = value; }
        }
    }
}
