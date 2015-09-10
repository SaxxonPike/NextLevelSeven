using System;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents a segment-level element in an HL7 message.
    /// </summary>
    internal sealed class NativeSegment : NativeElement, INativeSegment
    {
        private readonly Dictionary<int, INativeElement> _cache = new Dictionary<int, INativeElement>();
        private readonly EncodingConfiguration _encodingConfigurationOverride;

        public NativeSegment(NativeElement ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        private NativeSegment(string value, EncodingConfiguration config)
            : base(value)
        {
            _encodingConfigurationOverride = new EncodingConfiguration(config);
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return _encodingConfigurationOverride ?? Ancestor.EncodingConfiguration; }
        }

        public override INativeElement CloneDetached()
        {
            return CloneDetachedSegment();
        }

        INativeSegment INativeSegment.CloneDetached()
        {
            return CloneDetachedSegment();
        }

        public override char Delimiter
        {
            get
            {
                if (Ancestor != null)
                {
                    return EncodingConfiguration.FieldDelimiter;
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
                var index = ((NativeMessage) Ancestor).Segments
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

        private INativeSegment CloneDetachedSegment()
        {
            return new NativeSegment(Value, EncodingConfiguration);
        }

        public override INativeElement GetDescendant(int index)
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
                    var descendant = new NativeFieldDelimiter(this);
                    _cache[index] = descendant;
                    return descendant;
                }

                if (index == 2)
                {
                    var descendant = new NativeEncodingField(this);
                    _cache[index] = descendant;
                    return descendant;
                }

                if (index > 2)
                {
                    var descendant = new NativeField(this, index - 1, index);
                    _cache[index] = descendant;
                    return descendant;
                }
            }

            var result = new NativeField(this, index, index);
            _cache[index] = result;
            return result;
        }
    }
}