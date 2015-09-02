using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Building;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    public class MessageBuilderPerformanceTests
    {
        [TestMethod]
        public void MessageBuilder_Timely_CanBuildSmallMessages()
        {
            var testString = "ZZZ|" + Randomized.String();
            var expectedString = string.Format("MSH|^~\\&\xD" + string.Concat(Enumerable.Repeat("{0}\xD", 8)) + "{0}", testString);
            var time = Measure.ExecutionTime(() =>
            {
                var builder = new MessageBuilder();
                for (var i = 0; i < 9; i++)
                {
                    builder.Segment(i + 2, testString);
                }
                Assert.AreEqual(expectedString, builder.ToString());
            }, 1000);
            AssertTime.IsWithin(500, time);
        }

        [TestMethod]
        public void MessageBuilder_Timely_CanBuildLargeMessages()
        {
            var testString = "ZZZ|" + Randomized.String();
            var expectedString = string.Format("MSH|^~\\&\xD" + string.Concat(Enumerable.Repeat("{0}\xD", 98)) + "{0}", testString);
            var time = Measure.ExecutionTime(() =>
            {
                var builder = new MessageBuilder();
                for (var i = 0; i < 99; i++)
                {
                    builder.Segment(i + 2, testString);
                }
                Assert.AreEqual(expectedString, builder.ToString());
            }, 100);
            AssertTime.IsWithin(500, time);
        }
    }
}
