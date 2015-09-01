using System;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Cursors
{
    /// <summary>
    ///     Represents a segment-level element in an HL7 message.
    /// </summary>
    internal sealed class Segment : Element, ISegment
    {
        private readonly Dictionary<int, IElement> _cache = new Dictionary<int, IElement>();
        private readonly EncodingConfiguration _encodingConfigurationOverride;

        public Segment(Element ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        private Segment(string value, EncodingConfiguration config)
            : base(value)
        {
            _encodingConfigurationOverride = new EncodingConfiguration(config);
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return _encodingConfigurationOverride ?? Ancestor.EncodingConfiguration; }
        }

        public override IElement CloneDetached()
        {
            return CloneDetachedSegment();
        }

        ISegment ISegment.CloneDetached()
        {
            return CloneDetachedSegment();
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

        public override string Key
        {
            get
            {
                var index = ((Message) Ancestor).Segments
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

        private ISegment CloneDetachedSegment()
        {
            return new Segment(Value, EncodingConfiguration);
        }

        public override IElement GetDescendant(int index)
        {
            if (_cache.ContainsKey(index))
            {
                return _cache[index];
            }

            if (index < 0)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.FieldIndexMustBeZeroOrGreater));
            }

            if (string.Equals(Type, "MSH", StringComparison.Ordinal))
            {
                if (index == 1)
                {
                    var descendant = new FieldDelimiter(this);
                    _cache[index] = descendant;
                    return descendant;
                }

                if (index == 2)
                {
                    var descendant = new EncodingField(this);
                    _cache[index] = descendant;
                    return descendant;
                }

                if (index > 2)
                {
                    var descendant = new Field(this, index - 1, index);
                    _cache[index] = descendant;
                    return descendant;
                }
            }

            var result = new Field(this, index, index);
            _cache[index] = result;
            return result;
        }
    }
}