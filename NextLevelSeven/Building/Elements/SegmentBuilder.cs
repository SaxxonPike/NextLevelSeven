using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>Represents an HL7 segment.</summary>
    internal sealed class SegmentBuilder : DescendantBuilder, ISegmentBuilder
    {
        /// <summary>Descendant builders.</summary>
        private readonly BuilderElementCache<FieldBuilder> _fields;

        /// <summary>Create a segment builder with the specified encoding configuration.</summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index in the ancestor.</param>
        internal SegmentBuilder(Builder builder, int index)
            : base(builder, index)
        {
            _fields = new BuilderElementCache<FieldBuilder>(CreateFieldBuilder);
        }

        private SegmentBuilder(IEncoding config, int index)
            : base(config, index)
        {
            _fields = new BuilderElementCache<FieldBuilder>(CreateFieldBuilder);
        }

        /// <summary>If true, this is an MSH segment which has special behavior in fields 1 and 2.</summary>
        private bool IsMsh
        {
            get
            {
                // We don't want to modify the cache here, so don't use the indexer until we know it exists.
                if (!_fields.Contains(0))
                {
                    return false;
                }
                return _fields[0].Value == "MSH";
            }
        }

        /// <summary>Get a descendant field builder.</summary>
        /// <param name="index">Index within the segment to get the builder from.</param>
        /// <returns>Field builder for the specified index.</returns>
        public new IFieldBuilder this[int index] => _fields[index];

        /// <summary>Get the number of fields in this segment, including fields with no content.</summary>
        public override int ValueCount => _fields.Count > 0
            ? _fields.MaxKey + 1
            : 0;

        /// <summary>Get or set the three-letter type field of this segment.</summary>
        public string Type
        {
            get => _fields[0].Value;
            set => SetField(0, value);
        }

        /// <summary>Get or set field content within this segment.</summary>
        public override IEnumerable<string> Values
        {
            get
            {
                var count = ValueCount;
                for (var i = 0; i < count; i++)
                {
                    yield return _fields[i].Value;
                }
            }
            set => SetFields(value.ToArray());
        }

        /// <summary>Get or set the segment string.</summary>
        public override string Value
        {
            get
            {
                if (_fields.Count == 0)
                {
                    return null;
                }

                var index = 0;
                var result = new StringBuilder();
                var typeIsMsh = IsMsh;
                var fieldDelimiter = FieldDelimiter;

                foreach (var field in _fields.OrderedByKey.Where(field => field.Key >= 0))
                {
                    while (index < field.Key)
                    {
                        result.Append(fieldDelimiter);
                        index++;
                    }

                    if (typeIsMsh && index == 1)
                    {
                        index++;
                    }
                    else
                    {
                        result.Append(field.Value);
                    }
                }

                return result.Length == 0
                    ? null
                    : result.ToString();
            }
            set => SetSegment(value);
        }

        /// <summary>Set a component's content.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public ISegmentBuilder SetComponent(int fieldIndex, int repetition, int componentIndex, string value)
        {
            _fields[fieldIndex].SetComponent(repetition, componentIndex, value);
            return this;
        }

        /// <summary>Replace all component values within a field repetition.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public ISegmentBuilder SetComponents(int fieldIndex, int repetition, params string[] components)
        {
            _fields[fieldIndex].SetComponents(repetition, components);
            return this;
        }

        /// <summary>Set a sequence of components within a field repetition, beginning at the specified start index.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="startIndex">Component index to begin replacing at.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public ISegmentBuilder SetComponents(int fieldIndex, int repetition, int startIndex, params string[] components)
        {
            _fields[fieldIndex].SetComponents(repetition, startIndex, components);
            return this;
        }

        /// <summary>Set a field's content.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public ISegmentBuilder SetField(int fieldIndex, string value)
        {
            _fields[fieldIndex].SetField(value);
            return this;
        }

        /// <summary>Replace all field values within this segment.</summary>
        /// <param name="fields">Values to replace with.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public ISegmentBuilder SetFields(params string[] fields)
        {
            _fields.Clear();
            return SetFields(0, fields);
        }

        /// <summary>Set a sequence of fields within this segment, beginning at the specified start index.</summary>
        /// <param name="startIndex">Field index to begin replacing at.</param>
        /// <param name="fields">Values to replace with.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public ISegmentBuilder SetFields(int startIndex, params string[] fields)
        {
            if (fields == null)
            {
                return this;
            }

            foreach (var field in fields)
            {
                SetField(startIndex++, field);
            }

            return this;
        }

        /// <summary>Set a field repetition's content.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public ISegmentBuilder SetFieldRepetition(int fieldIndex, int repetition, string value)
        {
            _fields[fieldIndex].SetFieldRepetition(repetition, value);
            return this;
        }

        /// <summary>Replace all field repetitions within a field.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public ISegmentBuilder SetFieldRepetitions(int fieldIndex, params string[] repetitions)
        {
            _fields[fieldIndex].SetFieldRepetitions(repetitions);
            return this;
        }

        /// <summary>Set a sequence of field repetitions within a field, beginning at the specified start index.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="startIndex">Field repetition index to begin replacing at.</param>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public ISegmentBuilder SetFieldRepetitions(int fieldIndex, int startIndex, params string[] repetitions)
        {
            _fields[fieldIndex].SetFieldRepetitions(startIndex, repetitions);
            return this;
        }

        /// <summary>Set this segment's content.</summary>
        /// <param name="value">New value.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public ISegmentBuilder SetSegment(string value)
        {
            if (Type != "MSH" && value == null)
            {
                // non-MSH fields can be nullified.
                SetFields(null);
                return this;
            }

            if (value == null)
            {
                throw new ElementException(ErrorCode.SegmentDataMustNotBeNull);
            }

            if (value.Length <= 3)
            {
                throw new ElementException(ErrorCode.SegmentDataIsTooShort);
            }

            var isMsh = value.Substring(0, 3) == "MSH";
            if (isMsh)
            {
                // MSH determines field delimiter.
                FieldDelimiter = value[3];
            }

            var values = value.Split(FieldDelimiter);
            if (!isMsh)
            {
                return SetFields(values.ToArray());
            }

            var valueList = values.ToList();
            valueList.Insert(1, new string(FieldDelimiter, 1));
            values = valueList.ToArray();
            return SetFields(values.ToArray());
        }

        /// <summary>Set a subcomponent's content.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponentIndex">Subcomponent index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public ISegmentBuilder SetSubcomponent(int fieldIndex, int repetition, int componentIndex, int subcomponentIndex,
            string value)
        {
            _fields[fieldIndex].SetSubcomponent(repetition, componentIndex, subcomponentIndex, value);
            return this;
        }

        /// <summary>Replace all subcomponents within a component.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponents">Subcomponent index.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public ISegmentBuilder SetSubcomponents(int fieldIndex, int repetition, int componentIndex,
            params string[] subcomponents)
        {
            _fields[fieldIndex].SetSubcomponents(repetition, componentIndex, subcomponents);
            return this;
        }

        /// <summary>Set a sequence of subcomponents within a component, beginning at the specified start index.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="startIndex">Subcomponent index to begin replacing at.</param>
        /// <param name="subcomponents">Values to replace with.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public ISegmentBuilder SetSubcomponents(int fieldIndex, int repetition, int componentIndex, int startIndex,
            params string[] subcomponents)
        {
            _fields[fieldIndex].SetSubcomponents(repetition, componentIndex, startIndex, subcomponents);
            return this;
        }

        /// <summary>Get the value at the specified index.</summary>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns></returns>
        public string GetValue(int field = -1, int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return field < 0 ? Value : _fields[field].GetValue(repetition, component, subcomponent);
        }

        /// <summary>Get the values at the specified indices.</summary>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition index.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns></returns>
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

        /// <summary>Get this segment's data delimiter.</summary>
        public override char Delimiter => FieldDelimiter;

        /// <summary>Get this element's fields.</summary>
        IEnumerable<IField> ISegment.Fields
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

        /// <summary>Get the next available index.</summary>
        public override int NextIndex => ValueCount;

        /// <summary>If true, the element is considered to exist.</summary>
        public override bool Exists => _fields.AnyExists;

        /// <summary>Get this element's heirarchy-specific ancestor.</summary>
        IMessage ISegment.Ancestor => Ancestor as IMessage;

        /// <summary>Get this element's heirarchy-specific ancestor builder.</summary>
        IMessageBuilder ISegmentBuilder.Ancestor => Ancestor as IMessageBuilder;

        /// <summary>Deep clone this segment.</summary>
        /// <returns>Clone of the segment.</returns>
        private SegmentBuilder CloneSegment()
        {
            return new SegmentBuilder(new EncodingConfiguration(Encoding), Index)
            {
                Value = Value
            };
        }

        /// <summary>
        ///     Throw an exception if the index is one for an encoding element or is otherwise invalid.
        /// </summary>
        /// <param name="index"></param>
        protected override bool AssertIndexIsMovable(int index)
        {
            if (index < 0)
            {
                throw new ElementException(ErrorCode.ElementIndexMustBeZeroOrGreater);
            }
            if (index == 0)
            {
                throw new ElementException(ErrorCode.SegmentTypeCannotBeMoved);
            }
            if (index >= 1 && index <= 2 && IsMsh)
            {
                throw new ElementException(ErrorCode.EncodingElementCannotBeMoved);
            }
            return true;
        }

        /// <summary>Get descendant elements.</summary>
        /// <returns>Descendant elements.</returns>
        protected override IEnumerable<IElement> GetDescendants()
        {
            var count = ValueCount;
            for (var i = 0; i < count; i++)
            {
                yield return _fields[i];
            }
        }

        /// <summary>Create a field builder object.</summary>
        /// <param name="index">Index of the object to create.</param>
        /// <returns>Field builder object.</returns>
        private FieldBuilder CreateFieldBuilder(int index)
        {
            // segment type is treated specially
            if (index == 0)
            {
                return new TypeFieldBuilder(this, OnTypeFieldModified, index);
            }

            if (!IsMsh)
            {
                return new FieldBuilder(this, index);
            }

            // msh-1 and msh-2 are treated specially
            if (index == 1)
            {
                return new DelimiterFieldBuilder(this, index);
            }

            return index == 2
                ? new EncodingFieldBuilder(this, index)
                : new FieldBuilder(this, index);
        }

        /// <summary>Method that is called when a descendant type field has changed.</summary>
        /// <param name="oldValue">Old type field value.</param>
        /// <param name="newValue">New type field value.</param>
        private static void OnTypeFieldModified(string oldValue, string newValue)
        {
            if (oldValue != null && oldValue != newValue && (newValue == "MSH" || oldValue == "MSH"))
            {
                throw new ElementException(ErrorCode.ChangingSegmentTypesToAndFromMshIsNotSupported);
            }
        }

        /// <summary>Get the descendant element at the specified index.</summary>
        /// <param name="index">Index to reference.</param>
        /// <returns>Element at the index.</returns>
        protected override IElement GetGenericElement(int index)
        {
            return _fields[index];
        }

        protected override IIndexedCache<Builder> GetCache()
        {
            return _fields;
        }
    }
}