using System.Collections.Generic;
using NextLevelSeven.Core;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents a field that does not use a repetition delimiter (repeats are considered part of the value.)
    /// </summary>
    internal sealed class NativeFieldWithoutRepetitions : NativeElement, INativeField
    {
        public NativeFieldWithoutRepetitions(NativeElement ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        private NativeFieldWithoutRepetitions(string value, EncodingConfiguration config)
            : base(value, config)
        {
        }

        public override char Delimiter
        {
            get { return '\0'; }
        }

        public override int ValueCount
        {
            get { return 1; }
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

        public override IElement Clone()
        {
            return CloneInternal();
        }

        IField IField.Clone()
        {
            return CloneInternal();
        }

        public override INativeElement GetDescendant(int index)
        {
            return GetRepetition(index);
        }

        private INativeRepetition GetRepetition(int index)
        {
            return new NativeRepetition(this, index - 1, index);
        }

        private NativeFieldWithoutRepetitions CloneInternal()
        {
            return new NativeFieldWithoutRepetitions(Value, EncodingConfiguration) {Index = Index};
        }
    }
}