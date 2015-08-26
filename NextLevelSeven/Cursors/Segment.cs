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

        private Segment(string value, EncodingConfiguration config)
            : base(value)
        {
            _encodingConfigurationOverride = new EncodingConfiguration(config);
        }

        public override IElement CloneDetached()
        {
            return CloneDetachedSegment();
        }

        ISegment ISegment.CloneDetached()
        {
            return CloneDetachedSegment();
        }

        ISegment CloneDetachedSegment()
        {
            return new Segment(Value, EncodingConfiguration);
        }

        public override char Delimiter
        {
            get
            {
                if (Ancestor != null && DescendantDivider == null)
                {
                    return Ancestor.DescendantDivider.Value[3];
                }

                var value = Value;
                if (value != null && value.Length > 3)
                {
                    return value[3];                    
                }

                return '|';
            }
        }

        public override int DescendantCount
        {
            get
            {
                if (string.Equals(Type, "MSH", StringComparison.Ordinal))
                {
                    return DescendantDivider.Count;
                }
                return DescendantDivider.Count - 1;
            }
        }

        private readonly EncodingConfiguration _encodingConfigurationOverride;
        public override EncodingConfiguration EncodingConfiguration
        {
            get { return _encodingConfigurationOverride ?? Ancestor.EncodingConfiguration; }
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
