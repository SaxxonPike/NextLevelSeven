using NextLevelSeven.Conversion;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Specification.Elements;

namespace NextLevelSeven.Specification
{
    internal sealed class NumberRangeElement : SpecificationElementBase, INumberRange
    {
        public NumberRangeElement(IElement element)
            : base(element)
        {
        }

        public override void Validate()
        {
            decimal data;

            if (LowValueData != null)
            {
                if (!decimal.TryParse(LowValueData, out data))
                {
                    throw new ValidationException(ErrorCode.DataIsInvalidForType, "LowValueData");
                }
            }

            if (HighValueData == null)
            {
                return;
            }

            if (!decimal.TryParse(HighValueData, out data))
            {
                throw new ValidationException(ErrorCode.DataIsInvalidForType, "HighValueData");
            }
        }

        public decimal? LowValue
        {
            get
            {
                return NumberConverter.ConvertToDecimal(LowValueData);
            }
            set
            {
                LowValueData = NumberConverter.ConvertFromDecimal(value);
            }
        }

        public string LowValueData
        {
            get
            {
                return Element[1].Value;
            }
            set
            {
                Element[1].Value = value;
            }
        }

        public decimal? HighValue
        {
            get
            {
                return NumberConverter.ConvertToDecimal(HighValueData);
            }
            set
            {
                HighValueData = NumberConverter.ConvertFromDecimal(value);
            }
        }

        public string HighValueData
        {
            get
            {
                return Element[2].Value;
            }
            set
            {
                Element[2].Value = value;
            }
        }
    }
}