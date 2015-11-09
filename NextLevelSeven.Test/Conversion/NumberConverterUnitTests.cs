using NextLevelSeven.Conversion;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NextLevelSeven.Test.Conversion
{
    [TestFixture]
    public class NumberConverterUnitTests : ConversionTestFixture
    {
        [Test]
        public void NumberConverter_CanDecodeIntegerWithDecimalPoint()
        {
            Assert.AreEqual(NumberConverter.ConvertToInt("2.4"), 2);
        }

        [Test]
        public void NumberConverter_CanDecodeIntegerWithNegative()
        {
            Assert.AreEqual(NumberConverter.ConvertToInt("-2"), -2);
        }

        [Test]
        public void NumberConverter_CanDecodeIntegerWithPositive()
        {
            Assert.AreEqual(NumberConverter.ConvertToInt("+2"), 2);
        }

        [Test]
        public void NumberConverter_CanDecodeNullInteger()
        {
            Assert.AreEqual(NumberConverter.ConvertToInt(null), null);
        }

        [Test]
        public void NumberConverter_CanDecodeValidInteger()
        {
            Assert.AreEqual(NumberConverter.ConvertToInt("2"), 2);
        }

        [Test]
        public void NumberConverter_CanEncodeNullInteger()
        {
            Assert.AreEqual(NumberConverter.ConvertFromInt(null), null);
        }

        [Test]
        public void NumberConverter_CanEncodeValidInteger()
        {
            Assert.AreEqual(NumberConverter.ConvertFromInt(2), "2");
        }

        [Test]
        public void NumberConverter_ReturnsNullForInvalidInteger()
        {
            Assert.AreEqual(NumberConverter.ConvertToInt(MockFactory.String()), null);
        }

        [Test]
        public void NumberConverter_CanDecodeDecimalWithNegative()
        {
            Assert.AreEqual(NumberConverter.ConvertToDecimal("-2.357"), -2.357M);
        }

        [Test]
        public void NumberConverter_CanDecodeDecimalWithPositive()
        {
            Assert.AreEqual(NumberConverter.ConvertToDecimal("+2.579"), 2.579M);
        }

        [Test]
        public void NumberConverter_CanDecodeNullDecimal()
        {
            Assert.AreEqual(NumberConverter.ConvertToDecimal(null), null);
        }

        [Test]
        public void NumberConverter_CanDecodeValidDecimal()
        {
            Assert.AreEqual(NumberConverter.ConvertToDecimal("3.1415927"), 3.1415927M);
        }

        [Test]
        public void NumberConverter_CanEncodeNullDecimal()
        {
            Assert.AreEqual(NumberConverter.ConvertFromDecimal(null), null);
        }

        [Test]
        public void NumberConverter_CanEncodeValidDecimal()
        {
            Assert.AreEqual(NumberConverter.ConvertFromDecimal(1.1235813M), "1.1235813");
        }

        [Test]
        public void NumberConverter_ReturnsNullForInvalidDecimal()
        {
            Assert.AreEqual(NumberConverter.ConvertToDecimal(MockFactory.String()), null);
        }
    }
}