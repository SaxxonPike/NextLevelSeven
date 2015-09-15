using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Specification;

namespace NextLevelSeven.Test.Core.Specification
{
    [TestClass]
    public class NumberRangeExtensionTests : SpecificationTestFixture
    {
        static void EdgeTest(decimal? low, decimal? high)
        {
            var range = Message.Create()[1][3][1].AsNumberRange();
            range.LowValue = low;
            range.HighValue = high;
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
    }
}
