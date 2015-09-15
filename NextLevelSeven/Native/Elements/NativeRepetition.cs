using System;
using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents a repetition-level element in an HL7 message.
    /// </summary>
    internal sealed class NativeRepetition : NativeElement, INativeRepetition
    {
        private readonly Dictionary<int, INativeElement> _cache = new Dictionary<int, INativeElement>();

        public NativeRepetition(NativeElement ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        private NativeRepetition(string value, EncodingConfiguration config)
            : base(value, config)
        {
        }

        public override char Delimiter
        {
            get { return EncodingConfiguration.ComponentDelimiter; }
        }

        public string GetValue(int component = -1, int subcomponent = -1)
        {
            return component < 0
                ? Value
                : GetComponent(component).GetValue(subcomponent);
        }

        public IEnumerable<string> GetValues(int component = -1, int subcomponent = -1)
        {
            return component < 0
                ? Values
                : GetComponent(component).GetValues(subcomponent);
        }

        public new INativeComponent this[int index]
        {
            get { return GetComponent(index); }
        }

        public override IElement Clone()
        {
            return CloneInternal();
        }

        IRepetition IRepetition.Clone()
        {
            return CloneInternal();
        }

        public override INativeElement GetDescendant(int index)
        {
            return _cache.ContainsKey(index)
                ? _cache[index]
                : GetComponent(index);
        }

        private INativeComponent GetComponent(int index)
        {
            if (index < 1)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.ComponentIndexMustBeGreaterThanZero));
            }

            var result = new NativeComponent(this, index - 1, index);
            _cache[index] = result;
            return result;
        }

        private NativeRepetition CloneInternal()
        {
            return new NativeRepetition(Value, EncodingConfiguration) {Index = Index};
        }
    }
}