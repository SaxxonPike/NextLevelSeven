using System;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>Represents a segment-level element in an HL7 message.</summary>
    internal sealed class SegmentParser : DescendantParser, ISegmentParser
    {
        /// <summary>Internal component cache.</summary>
        private readonly IndexedCache<FieldParser> _fields;

        /// <summary>Create a segment with the specified ancestor and indices.</summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="parentIndex">Index within the ancestor string divider.</param>
        /// <param name="externalIndex">Exposed index.</param>
        public SegmentParser(Parser ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
            _fields = new WeakReferenceCache<FieldParser>(CreateField);
        }

        /// <summary>Create a segment root with the specified encoding configuration.</summary>
        /// <param name="config"></param>
        private SegmentParser(ReadOnlyEncodingConfiguration config)
            : base(config)
        {
            _fields = new WeakReferenceCache<FieldParser>(CreateField);
        }

        /// <summary>Returns true if the segment's type field is MSH.</summary>
        private bool IsMsh => string.Equals(Type, "MSH", StringComparison.Ordinal);

        /// <summary>Get the descendant field parser at the specified index.</summary>
        /// <param name="index">Index of the field.</param>
        /// <returns>Field parser at the specified index.</returns>
        IFieldParser ISegmentParser.this[int index] => _fields[index];

        /// <summary>Delete a descendant element.</summary>
        /// <param name="index">Index to insert at.</param>
        public override void Delete(int index)
        {
            if (IsMsh)
            {
                ThrowIfEncodingFieldIndex(index);
                DescendantDivider.Delete(index - 1);
            }
            else
            {
                DescendantDivider.Delete(index);
            }
        }

        /// <summary>Insert a descendant element.</summary>
        /// <param name="element">Element to insert.</param>
        /// <param name="index">Index to insert at.</param>
        public override IElement Insert(int index, IElement element)
        {
            Insert(index, element.Value);
            return GetDescendant(index);
        }

        /// <summary>Insert a descendant element.</summary>
        /// <param name="value">Value to insert.</param>
        /// <param name="index">Index to insert at.</param>
        public override IElement Insert(int index, string value)
        {
            if (IsMsh)
            {
                ThrowIfEncodingFieldIndex(index);
                DescendantDivider.Insert(index - 1, value);
            }
            else
            {
                DescendantDivider.Insert(index, value);
            }
            return GetDescendant(index);
        }

        /// <summary>Move a descendant.</summary>
        /// <param name="sourceIndex"></param>
        /// <param name="targetIndex"></param>
        public override void Move(int sourceIndex, int targetIndex)
        {
            if (IsMsh)
            {
                ThrowIfEncodingFieldIndex(sourceIndex, targetIndex);
                DescendantDivider.Move(sourceIndex - 1, targetIndex - 1);
            }
            else
            {
                DescendantDivider.Move(sourceIndex, targetIndex);
            }
        }

        /// <summary>Field delimiter.</summary>
        public override char Delimiter
        {
            get
            {
                // use ancestor's delimiter where possible
                if (Ancestor != null)
                {
                    return EncodingConfiguration.FieldDelimiter;
                }

                // if the string divider is not initialized yet, trying to use it indirectly causes an infinite loop.
                return DescendantDividerInitialized ? DescendantDividerDelimiter : '|';
            }
        }

        /// <summary>Get descendant fields as an enumerable set.</summary>
        public override IEnumerable<IElementParser> Descendants
        {
            get
            {
                var count = ValueCount;
                for (var i = 0; i < count; i++)
                {
                    yield return this[i];
                }
            }
        }

        /// <summary>Get the number of values in the segment.</summary>
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

        /// <summary>Get the segment type from index 0.</summary>
        public string Type
        {
            get => DescendantDivider[0];
            set => DescendantDivider[0] = value;
        }

        /// <summary>Get the value at the specified indices.</summary>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>Value at the specified indices.</returns>
        public string GetValue(int field = -1, int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return field < 0 ? Value : _fields[field].GetValue(repetition, component, subcomponent);
        }

        /// <summary>Get all values at the specified indices.</summary>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>Values at the specified indices.</returns>
        public IEnumerable<string> GetValues(int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return field < 0 ? Values : _fields[field].GetValues(repetition, component, subcomponent);
        }

        /// <summary>Deep clone this element.</summary>
        /// <returns>Clone of the element.</returns>
        public override IElement Clone()
        {
            return CloneSegment();
        }

        /// <summary>Deep clone this segment.</summary>
        /// <returns>Clone of the segment.</returns>
        ISegment ISegment.Clone()
        {
            return CloneSegment();
        }

        /// <summary>Get or set all field values in this segment.</summary>
        public override IEnumerable<string> Values
        {
            get { return Descendants.Select(d => d.Value); }
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

        /// <summary>Get all fields.</summary>
        public IEnumerable<IFieldParser> Fields
        {
            get
            {
                var count = ValueCount;
                for (var i = 0; i < count; i++)
                {
                    yield return _fields[i];
                }
            }
        }

        /// <summary>Get all components.</summary>
        IEnumerable<IField> ISegment.Fields => Fields;

        /// <summary>Get the next available index.</summary>
        public override int NextIndex => ValueCount;

        /// <summary>Get this element's heirarchy-specific ancestor.</summary>
        IMessage ISegment.Ancestor => Ancestor as IMessage;

        /// <summary>Get this element's heirarchy-specific ancestor parser.</summary>
        IMessageParser ISegmentParser.Ancestor => Ancestor as IMessageParser;

        /// <summary>Get the descendant element at the specified index.</summary>
        /// <param name="index">Index of the descendant element.</param>
        /// <returns>Descendant element at the specified index.</returns>
        protected override IElementParser GetDescendant(int index)
        {
            return _fields[index];
        }

        /// <summary>Create a field object.</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private FieldParser CreateField(int index)
        {
            if (index < 0)
            {
                throw new ElementException(ErrorCode.FieldIndexMustBeZeroOrGreater);
            }

            if (IsMsh)
            {
                if (index == 1)
                {
                    var descendant = new DelimiterFieldParser(this);
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

        /// <summary>Deep clone this segment.</summary>
        /// <returns>Clone of the segment.</returns>
        private SegmentParser CloneSegment()
        {
            return new SegmentParser(EncodingConfiguration)
            {
                Index = Index,
                Value = Value
            };
        }

        /// <summary>
        ///     Enforce not being able to modify placement for encoding fields.
        /// </summary>
        private static void ThrowIfEncodingFieldIndex(params int[] indices)
        {
            if (indices.Any(index => index <= 2))
            {
                throw new ElementException(ErrorCode.EncodingElementCannotBeMoved);
            }
        }
    }
}