using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Cursors
{
    /// <summary>
    /// Represents a field-level element in an HL7 message.
    /// </summary>
    internal class Field : Element
    {
        public Field(Element ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        public Field(string value, EncodingConfiguration config)
            : base(value)
        {
            _encodingConfigurationOverride = new EncodingConfiguration(config);
        }

        public override IElement CloneDetached()
        {
            return new Field(Value, EncodingConfiguration);
        }

        public override char Delimiter
        {
            get { return EncodingConfiguration.RepetitionDelimiter; }
        }

        private readonly EncodingConfiguration _encodingConfigurationOverride;
        public override EncodingConfiguration EncodingConfiguration
        {
            get { return _encodingConfigurationOverride ?? Ancestor.EncodingConfiguration; }
        }

        public override IElement GetDescendant(int index)
        {
            if (index < 0)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.RepetitionIndexMustBeZeroOrGreater));
            }
            if (index == 0)
            {
                return new Repetition(new FieldWithoutRepetitions(Ancestor, ParentIndex, Index), 0, 0);
            }
            return new Repetition(this, index - 1, index);
        }
    }
}
