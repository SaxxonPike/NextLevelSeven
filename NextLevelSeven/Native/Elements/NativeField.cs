using System;
using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents a field-level element in an HL7 message.
    /// </summary>
    internal sealed class NativeField : NativeElement
    {
        private readonly Dictionary<int, INativeElement> _cache = new Dictionary<int, INativeElement>();
        private readonly EncodingConfiguration _encodingConfigurationOverride;

        public NativeField(NativeElement ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        public NativeField(string value, EncodingConfiguration config)
            : base(value)
        {
            _encodingConfigurationOverride = new EncodingConfiguration(config);
        }

        public override char Delimiter
        {
            get { return EncodingConfiguration.RepetitionDelimiter; }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return _encodingConfigurationOverride ?? Ancestor.EncodingConfiguration; }
        }

        public override INativeElement CloneDetached()
        {
            return new NativeField(Value, EncodingConfiguration);
        }

        public override INativeElement GetDescendant(int index)
        {
            if (_cache.ContainsKey(index))
            {
                return _cache[index];
            }

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
    }
}