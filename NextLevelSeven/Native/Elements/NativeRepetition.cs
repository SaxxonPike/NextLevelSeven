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
        private readonly EncodingConfiguration _encodingConfigurationOverride;

        public NativeRepetition(NativeElement ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        private NativeRepetition(string value, EncodingConfiguration config)
            : base(value)
        {
            _encodingConfigurationOverride = new EncodingConfiguration(config);
        }

        public override char Delimiter
        {
            get { return EncodingConfiguration.ComponentDelimiter; }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return _encodingConfigurationOverride ?? Ancestor.EncodingConfiguration; }
        }

        public override INativeElement CloneDetached()
        {
            return new NativeRepetition(Value, EncodingConfiguration);
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

        INativeRepetition INativeRepetition.CloneDetached()
        {
            return new NativeRepetition(Value, EncodingConfiguration);            
        }
    }
}