using System;
using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents a field-level element in an HL7 message.
    /// </summary>
    internal class NativeField : NativeElement, INativeField
    {
        /// <summary>
        ///     Internal repetition cache.
        /// </summary>
        private readonly IndexedCache<int, NativeRepetition> _cache;

        /// <summary>
        ///     Create a field with the specified ancestor and indices.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="parentIndex">Zero-based index within the parent's raw data.</param>
        /// <param name="externalIndex">Exposed index.</param>
        public NativeField(NativeElement ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
            _cache = new IndexedCache<int, NativeRepetition>(CreateRepetition);
        }

        /// <summary>
        ///     Create a detached field with the specified initial value and configuration.
        /// </summary>
        /// <param name="value">Initial value.</param>
        /// <param name="config">Encoding configuration.</param>
        public NativeField(string value, EncodingConfigurationBase config)
            : base(value, config)
        {
            _cache = new IndexedCache<int, NativeRepetition>(CreateRepetition);
        }

        /// <summary>
        ///     Field repetition delimiter.
        /// </summary>
        public override char Delimiter
        {
            get { return EncodingConfiguration.RepetitionDelimiter; }
        }

        /// <summary>
        ///     Get the value at the specified indices.
        /// </summary>
        /// <param name="repetition">Repetition index.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>Value at the specified indices.</returns>
        public virtual string GetValue(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return repetition < 0
                ? Value
                : GetRepetition(repetition).GetValue(component, subcomponent);
        }

        /// <summary>
        ///     Get the values at the specified indices.
        /// </summary>
        /// <param name="repetition">Repetition index.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>Values at the specified indices.</returns>
        public virtual IEnumerable<string> GetValues(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return repetition < 0
                ? Values
                : GetRepetition(repetition).GetValues(component, subcomponent);
        }

        /// <summary>
        ///     Get a descendant field repetition.
        /// </summary>
        /// <param name="index">Index of the repetition.</param>
        /// <returns>Desired field repetition.</returns>
        public new INativeRepetition this[int index]
        {
            get { return GetRepetition(index); }
        }

        /// <summary>
        ///     Deep clone this field.
        /// </summary>
        /// <returns>Clone of the field.</returns>
        public override sealed IElement Clone()
        {
            return CloneInternal();
        }

        /// <summary>
        ///     Deep clone this field.
        /// </summary>
        /// <returns>Clone of the field.</returns>
        IField IField.Clone()
        {
            return CloneInternal();
        }

        /// <summary>
        ///     Get all field repetitions.
        /// </summary>
        public IEnumerable<INativeRepetition> Repetitions
        {
            get
            {
                return new WrapperEnumerable<INativeRepetition>(i => _cache[i],
                    (i, v) => { },
                    () => ValueCount,
                    1);
            }
        }

        /// <summary>
        ///     Get all field repetitions.
        /// </summary>
        IEnumerable<IRepetition> IField.Repetitions
        {
            get { return Repetitions; }
        }

        /// <summary>
        ///     Get the descendant element at the specified index.
        /// </summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Element at the specified index.</returns>
        public override sealed INativeElement GetDescendant(int index)
        {
            return GetRepetition(index);
        }

        /// <summary>
        ///     Get the descendant field repetition at the specified index.
        /// </summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Element at the specified index.</returns>
        protected INativeRepetition GetRepetition(int index)
        {
            return _cache[index];
        }

        /// <summary>
        ///     Create a repetition object.
        /// </summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Subcomponent.</returns>
        protected virtual NativeRepetition CreateRepetition(int index)
        {
            if (index < 0)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.RepetitionIndexMustBeZeroOrGreater));
            }

            return index == 0
                ? new NativeRepetition(new NativeFieldWithoutRepetitions(Ancestor, ParentIndex, Index), 0, 0)
                : new NativeRepetition(this, index - 1, index);
        }

        /// <summary>
        ///     Deep clone this field.
        /// </summary>
        /// <returns>Clone of the field.</returns>
        protected virtual NativeField CloneInternal()
        {
            return new NativeField(Value, EncodingConfiguration) {Index = Index};
        }
    }
}