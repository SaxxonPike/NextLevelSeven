using System;
using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents a component level element of an HL7 message.
    /// </summary>
    internal sealed class NativeComponent : NativeElement, INativeComponent
    {
        private readonly Dictionary<int, INativeElement> _cache = new Dictionary<int, INativeElement>();

        public NativeComponent(NativeElement ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        private NativeComponent(string value, EncodingConfiguration config)
            : base(value, config)
        {
        }

        public override char Delimiter
        {
            get { return EncodingConfiguration.SubcomponentDelimiter; }
        }

        public string GetValue(int subcomponent = -1)
        {
            return subcomponent < 0
                ? Value
                : GetSubcomponent(subcomponent).Value;
        }

        public IEnumerable<string> GetValues(int subcomponent = -1)
        {
            return subcomponent < 0
                ? Values
                : GetSubcomponent(subcomponent).Value.Yield();
        }

        public new INativeSubcomponent this[int index]
        {
            get { return GetSubcomponent(index); }
        }

        public override IElement Clone()
        {
            return CloneInternal();
        }

        IComponent IComponent.Clone()
        {
            return CloneInternal();
        }

        public override INativeElement GetDescendant(int index)
        {
            if (_cache.ContainsKey(index))
            {
                return _cache[index];
            }

            return GetSubcomponent(index);
        }

        private INativeSubcomponent GetSubcomponent(int index)
        {
            if (index < 1)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.SubcomponentIndexMustBeGreaterThanZero));
            }

            var result = new NativeSubcomponent(this, index - 1, index);
            _cache[index] = result;
            return result;
        }

        private NativeComponent CloneInternal()
        {
            return new NativeComponent(Value, EncodingConfiguration) {Index = Index};
        }
    }
}