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

        [TestMethod]
        public void MessageBuilder_Timely_ModifiesSegment()
        {
            var builder = new MessageBuilder();
            var segment = Randomized.StringLetters(3) + "|" + Randomized.String();
            var time = Measure.ExecutionTime(() => builder.Segment(2, segment), 10000);
            AssertTime.IsWithin(1000, time);
        }

        [TestMethod]
        public void MessageBuilder_Timely_ModifiesField()
        {
            var builder = new MessageBuilder();
            var field = Randomized.String();
            var time = Measure.ExecutionTime(() => builder.Field(2, 2, field), 10000);
            AssertTime.IsWithin(500, time);
        }

        [TestMethod]
        public void MessageBuilder_Timely_ModifiesFieldRepetition()
        {
            var builder = new MessageBuilder();
            var value = Randomized.String();
            var time = Measure.ExecutionTime(() => builder.FieldRepetition(2, 2, 2, value), 10000);
            AssertTime.IsWithin(500, time);
        }

        [TestMethod]
        public void MessageBuilder_Timely_ModifiesComponent()
        {
            var builder = new MessageBuilder();
            var value = Randomized.String();
            var time = Measure.ExecutionTime(() => builder.Component(2, 2, 2, 2, value), 10000);
            AssertTime.IsWithin(500, time);
        }

        [TestMethod]
        public void MessageBuilder_Timely_ModifiesSubcomponent()
        {
            var builder = new MessageBuilder();
            var value = Randomized.String();
            var time = Measure.ExecutionTime(() => builder.Subcomponent(2, 2, 2, 2, 2, value), 10000);
            AssertTime.IsWithin(500, time);
        }
    }
}
