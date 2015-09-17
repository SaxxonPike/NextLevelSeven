using System;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents a segment-level element in an HL7 message.
    /// </summary>
    internal sealed class NativeSegment : NativeElement, INativeSegment
    {
        /// <summary>
        ///     Internal component cache.
        /// </summary>
        private readonly IndexedCache<int, NativeField> _cache;

        public NativeSegment(NativeElement ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
            _cache = new IndexedCache<int, NativeField>(CreateField);
        }

        private NativeSegment(string value, EncodingConfiguration config)
            : base(value, config)
        {
            _cache = new IndexedCache<int, NativeField>(CreateField);
        }

        INativeField INativeSegment.this[int index]
        {
            get { return _cache[index]; }
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

        public override int ValueCount
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

        public string GetValue(int field = -1, int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return field < 0
                ? Value
                : _cache[field].GetValue(repetition, component, subcomponent);
        }

        public IEnumerable<string> GetValues(int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return field < 0
                ? Values
                : _cache[field].GetValues(repetition, component, subcomponent);
        }

        public override IElement Clone()
        {
            return CloneInternal();
        }

        ISegment ISegment.Clone()
        {
            return CloneInternal();
        }

        public override INativeElement GetDescendant(int index)
        {
            return _cache[index];
        }

        /// <summary>
        ///     Create a field object.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private NativeField CreateField(int index)
        {
            if (index < 0)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.FieldIndexMustBeZeroOrGreater));
            }

            if (string.Equals(Type, "MSH", StringComparison.Ordinal))
            {
                if (index == 1)
                {
                    var descendant = new NativeFieldDelimiter(this);
                    return descendant;
                }

                if (index == 2)
                {
                    var descendant = new NativeEncodingField(this);
                    return descendant;
                }

                if (index > 2)
                {
                    var descendant = new NativeField(this, index - 1, index);
                    return descendant;
                }
            }

            var result = new NativeField(this, index, index);
            return result;
        }

        private NativeSegment CloneInternal()
        {
            return new NativeSegment(Value, EncodingConfiguration) {Index = Index};
        }
    }
}