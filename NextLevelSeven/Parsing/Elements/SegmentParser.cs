using System;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>
    ///     Represents a segment-level element in an HL7 message.
    /// </summary>
    internal sealed class SegmentParser : ElementParser, ISegmentParser
    {
        /// <summary>
        ///     Internal component cache.
        /// </summary>
        private readonly IndexedCache<int, FieldParser> _fields;

        public SegmentParser(ElementParser ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
            _fields = new IndexedCache<int, FieldParser>(CreateField);
        }

        private SegmentParser(string value, EncodingConfigurationBase config)
            : base(value, config)
        {
            _fields = new IndexedCache<int, FieldParser>(CreateField);
        }

        private bool IsMsh
        {
            get { return (string.Equals(Type, "MSH", StringComparison.Ordinal)); }
        }

        IFieldParser ISegmentParser.this[int index]
        {
            get { return _fields[index]; }
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

        public override IEnumerable<IElementParser> DescendantElements
        {
            get
            {
                return new ProxyEnumerable<IElementParser>(GetDescendant,
                    null,
                    GetValueCount);
            }
        }

        public override int ValueCount
        {
            get
            {
                if (IsMsh)
                {
                    return DescendantDivider.Count + 1;
                }
                return DescendantDivider.Count;
            }
        }

        public override string Key
        {
            get
            {
                var index = ((MessageParser) Ancestor).Segments
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
                : _fields[field].GetValue(repetition, component, subcomponent);
        }

        public IEnumerable<string> GetValues(int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return field < 0
                ? Values
                : _fields[field].GetValues(repetition, component, subcomponent);
        }

        public override IElement Clone()
        {
            return CloneInternal();
        }

        ISegment ISegment.Clone()
        {
            return CloneInternal();
        }

        public override IEnumerable<string> Values
        {
            get { return base.Values; }
            set
            {
                if (IsMsh)
                {
                    // MSH changes how indices work
                    var values = value.ToList();
                    var delimiter = values[1];
                    values.RemoveAt(1);
                    DescendantDivider.Value = string.Join(delimiter, values);
                    return;
                }
                base.Values = value;
            }
        }

        /// <summary>
        ///     Get all components.
        /// </summary>
        public IEnumerable<IFieldParser> Fields
        {
            get
            {
                return new ProxyEnumerable<IFieldParser>(i => _fields[i],
                    null,
                    GetValueCount,
                    1);
            }
        }

        /// <summary>
        ///     Get all components.
        /// </summary>
        IEnumerable<IField> ISegment.Fields
        {
            get { return Fields; }
        }

        public override IElementParser GetDescendant(int index)
        {
            return _fields[index];
        }

        /// <summary>
        ///     Create a field object.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private FieldParser CreateField(int index)
        {
            if (index < 0)
            {
                throw new ParserException(ErrorCode.FieldIndexMustBeZeroOrGreater);
            }

            if (IsMsh)
            {
                if (index == 1)
                {
                    var descendant = new FieldParserDelimiter(this);
                    return descendant;
                }

                if (index == 2)
                {
                    var descendant = new EncodingFieldParser(this);
                    return descendant;
                }

                if (index > 2)
                {
                    var descendant = new FieldParser(this, index - 1, index);
                    return descendant;
                }
            }

            var result = new FieldParser(this, index, index);
            return result;
        }

        private SegmentParser CloneInternal()
        {
            return new SegmentParser(Value, EncodingConfiguration) {Index = Index};
        }
    }
}