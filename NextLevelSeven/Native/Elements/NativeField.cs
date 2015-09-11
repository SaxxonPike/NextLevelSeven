using System;
using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents a field-level element in an HL7 message.
    /// </summary>
    internal sealed class NativeField : NativeElement, INativeField
    {
        private readonly Dictionary<int, INativeElement> _cache = new Dictionary<int, INativeElement>();

        public NativeField(NativeElement ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        public NativeField(string value, EncodingConfiguration config)
            : base(value, config)
        {
        }

        public override char Delimiter
        {
            get { return EncodingConfiguration.RepetitionDelimiter; }
        }

        public override INativeElement GetDescendant(int index)
        {
            return _cache.ContainsKey(index)
                ? _cache[index]
                : GetRepetition(index);
        }

        private INativeRepetition GetRepetition(int index)
        {
            if (index < 0)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.RepetitionIndexMustBeZeroOrGreater));
            }

            if (index == 0)
            {
                var descendant = new NativeRepetition(new NativeFieldWithoutRepetitions(Ancestor, ParentIndex, Index), 0,
                    0);
                _cache[index] = descendant;
                return descendant;
            }

            var result = new NativeRepetition(this, index - 1, index);
            _cache[index] = result;
            return result;            
        }

        public string GetValue(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return repetition < 0
                ? Value
                : GetRepetition(repetition).GetValue(component, subcomponent);
        }

        public IEnumerable<string> GetValues(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return repetition < 0
                ? Values
                : GetRepetition(repetition).GetValues(component, subcomponent);
        }

        public new INativeRepetition this[int index]
        {
            get { return GetRepetition(index); }
        }

        override public IElement Clone()
        {
            return CloneInternal();
        }

        IField IField.Clone()
        {
            return CloneInternal();
        }

        NativeField CloneInternal()
        {
            return new NativeField(Value, EncodingConfiguration) { Index = Index };
        }
    }
}