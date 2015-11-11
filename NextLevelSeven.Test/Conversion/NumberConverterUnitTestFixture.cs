using System.Globalization;
using NextLevelSeven.Conversion;
using NUnit.Framework;

namespace NextLevelSeven.Test.Conversion
{
    [TestFixture]
    public class NumberConverterUnitTestFixture : ConversionBaseTestFixture
    {
        [Test]
        [TestCase("2", Result = 2)]
        [TestCase("2.4", Result = 2)]
        [TestCase("-2.4", Result = -2)]
        [TestCase("-2", Result = -2)]
        [TestCase("+2", Result = 2)]
        [TestCase("Invalid", Result = null)]
        [TestCase(null, Result = null)]
        public int? NumberConverter_CanDecodeInteger(string value)
        {
            return NumberConverter.ConvertToInt(value);
        }

        [Test]
        [TestCase(2, Result = "2")]
        [TestCase(-2, Result = "-2")]
        [TestCase(null, Result = null)]
        public string NumberConverter_CanEncodeInteger(int? value)
        {
            return NumberConverter.ConvertFromInt(value);
        }

        // Decimal is not a core CLR type, so it cannot be used for the following tests.
        // String values are parsed into Decimal type instead.

        [Test]
        [TestCase("-2.3", Result = "-2.3")]
        [TestCase("2.7", Result = "2.7")]
        [TestCase("+2.5", Result = "2.5")]
        [TestCase("Invalid", Result = null)]
        [TestCase(null, Result = null)]
        public string NumberConverter_CanDecodeDecimal(string value)
        {
            var result = NumberConverter.ConvertToDecimal(value);
            return result == null
                ? null
                : result.Value.ToString(CultureInfo.InvariantCulture);
        }

        [Test]
        [TestCase("-2.3", Result = "-2.3")]
        [TestCase("2.7", Result = "2.7")]
        [TestCase(null, Result = null)]
        public string NumberConverter_CanEncodeDecimal(string value)
        {
            return value == null
                ? NumberConverter.ConvertFromDecimal(null)
                : NumberConverter.ConvertFromDecimal(decimal.Parse(value));
        }
    }
}