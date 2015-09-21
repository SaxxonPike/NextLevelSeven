using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Specification;

namespace NextLevelSeven.Test.Specification.Elements
{
    [TestClass]
    public class NumberRangeExtensionTests : SpecificationTestFixture
    {
        private static void EdgeTest(decimal? low, decimal? high)
        {
            var range = GetNumberRange(low, high);
            if (low.HasValue)
            {
                Assert.IsFalse(range.Contains(range.LowValue - 1));
                Assert.IsTrue(range.Contains(range.LowValue));
                Assert.IsTrue(range.Contains(range.LowValue + 1));
            }
            if (high.HasValue)
            {
                Assert.IsTrue(range.Contains(range.HighValue - 1));
                Assert.IsTrue(range.Contains(range.HighValue));
                Assert.IsFalse(range.Contains(range.HighValue + 1));
            }
            Assert.IsFalse(range.Contains(null));
        }

        private static INumberRange GetNumberRange(decimal? low, decimal? high)
        {
            var range = Message.Parse()[1][3][1].AsNumberRange();
            range.LowValue = low;
            range.HighValue = high;
            return range;
        }

        [TestMethod]
        public void NumberRange_ShouldWork_WithBothLowAndHighLimits()
        {
            EdgeTest(Randomized.Number(100, 200), Randomized.Number(300, 400));
        }

        [TestMethod]
        public void NumberRange_ShouldWork_WithOnlyLowLimit()
        {
            EdgeTest(Randomized.Number(100, 200), null);
        }

        [TestMethod]
        public void NumberRange_ShouldWork_WithOnlyHighLimit()
        {
            EdgeTest(null, Randomized.Number(300, 400));
        }

        [TestMethod]
        public void NumberRange_Parses_LowValue()
        {
            var low = Randomized.Number(100, 200);
            var high = Randomized.Number(300, 400);
            var range = GetNumberRange(low, high);
            Assert.AreEqual(low, range.LowValue);
        }

        [TestMethod]
        public void NumberRange_Parses_HighValue()
        {
            var low = Randomized.Number(100, 200);
            var high = Randomized.Number(300, 400);
            var range = GetNumberRange(low, high);
            Assert.AreEqual(high, range.HighValue);
        }

        [TestMethod]
        public void NumberRange_Parses_NullLowValue()
        {
            var high = Randomized.Number(300, 400);
            var range = GetNumberRange(null, high);
            Assert.IsNull(range.LowValue);
        }

        [TestMethod]
        public void NumberRange_Parses_NullHighValue()
        {
            var low = Randomized.Number(100, 200);
            var range = GetNumberRange(low, null);
            Assert.IsNull(range.HighValue);
        }

        [TestMethod]
        public void NumberRange_Sets_LowValue()
        {
            var low = Randomized.Number(100, 200);
            var high = Randomized.Number(300, 400);
            var range = GetNumberRange(250, high);
            range.LowValue = low;
            Assert.AreEqual(low, range.LowValue);
        }

        [TestMethod]
        public void NumberRange_Sets_HighValue()
        {
            var low = Randomized.Number(100, 200);
            var high = Randomized.Number(300, 400);
            var range = GetNumberRange(low, 500);
            range.HighValue = high;
            Assert.AreEqual(high, range.HighValue);
        }

        [TestMethod]
        public void NumberRange_Sets_NullLowValue()
        {
            var low = Randomized.Number(100, 200);
            var high = Randomized.Number(300, 400);
            var range = GetNumberRange(low, high);
            range.LowValue = null;
            Assert.IsNull(range.LowValue);
        }

        [TestMethod]
        public void NumberRange_Sets_NullHighValue()
        {
            var low = Randomized.Number(100, 200);
            var high = Randomized.Number(300, 400);
            var range = GetNumberRange(low, high);
            range.HighValue = null;
            Assert.IsNull(range.HighValue);
        }
    }
}