using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>Represents a field-level element in an HL7 message.</summary>
    internal class FieldParser : DescendantParser, IFieldParser
    {
        /// <summary>Internal repetition cache.</summary>
        private readonly IndexedCache<RepetitionParser> _repetitions;

        /// <summary>Create a field with the specified ancestor and indices.</summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="parentIndex">Zero-based index within the parent's raw data.</param>
        /// <param name="externalIndex">Exposed index.</param>
        public FieldParser(Parser ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
            _repetitions = new WeakReferenceCache<RepetitionParser>(CreateRepetition);
        }

        /// <summary>Create a detached field with the specified initial value and configuration.</summary>
        /// <param name="config">Encoding configuration.</param>
        public FieldParser(ReadOnlyEncodingConfiguration config)
            : base(config)
        {
            _repetitions = new WeakReferenceCache<RepetitionParser>(CreateRepetition);
        }

        /// <summary>Field repetition delimiter.</summary>
        public override char Delimiter => EncodingConfiguration.RepetitionDelimiter;

        /// <summary>Get the value at the specified indices.</summary>
        /// <param name="repetition">Repetition index.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>Value at the specified indices.</returns>
        public virtual string GetValue(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return repetition < 0 ? Value : _repetitions[repetition].GetValue(component, subcomponent);
        }

        /// <summary>Get the values at the specified indices.</summary>
        /// <param name="repetition">Repetition index.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>Values at the specified indices.</returns>
        public virtual IEnumerable<string> GetValues(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return repetition < 0 ? Values : _repetitions[repetition].GetValues(component, subcomponent);
        }

        /// <summary>Get a descendant field repetition.</summary>
        /// <param name="index">Index of the repetition.</param>
        /// <returns>Desired field repetition.</returns>
        public new IRepetitionParser this[int index] => _repetitions[index];

        /// <summary>Deep clone this field.</summary>
        /// <returns>Clone of the field.</returns>
        public sealed override IElement Clone()
        {
            return CloneField();
        }

        /// <summary>Deep clone this field.</summary>
        /// <returns>Clone of the field.</returns>
        IField IField.Clone()
        {
            return CloneField();
        }

        /// <summary>Get all field repetitions.</summary>
        public IEnumerable<IRepetitionParser> Repetitions
        {
            get
            {
                var count = ValueCount;
                for (var i = 1; i <= count; i++)
                {
                    yield return _repetitions[i];
                }
            }
        }

        /// <summary>Get all field repetitions.</summary>
        IEnumerable<IRepetition> IField.Repetitions => Repetitions;

        /// <summary>Get this element's heirarchy-specific ancestor.</summary>
        ISegment IField.Ancestor => Ancestor as ISegment;

        /// <summary>Get this element's heirarchy-specific ancestor parser.</summary>
        ISegmentParser IFieldParser.Ancestor => Ancestor as ISegmentParser;

        /// <summary>Get the descendant element at the specified index.</summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Element at the specified index.</returns>
        protected override IElementParser GetDescendant(int index)
        {
            return _repetitions[index];
        }

        /// <summary>Create a repetition object.</summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Subcomponent.</returns>
        protected virtual RepetitionParser CreateRepetition(int index)
        {
            if (index <= 0)
            {
                throw new ElementException(ErrorCode.RepetitionIndexMustBeGreaterThanZero);
            }

            return new RepetitionParser(this, index - 1, index);
        }

        /// <summary>Deep clone this field.</summary>
        /// <returns>Clone of the field.</returns>
        protected virtual FieldParser CloneField()
        {
            return new FieldParser(EncodingConfiguration)
            {
                Index = Index,
                Value = Value
            };
        }
    }
}