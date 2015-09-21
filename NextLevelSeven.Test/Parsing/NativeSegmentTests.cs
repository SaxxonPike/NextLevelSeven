using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Parsing
{
    [TestClass]
    public class NativeSegmentTests : NativeTestFixture
    {
        [TestMethod]
        public void Segment_WithIdenticalValueToAnotherSegment_IsEquivalent()
        {
            var segment1 = Message.Parse(ExampleMessages.Standard)[1];
            var segment2 = Message.Parse(ExampleMessages.Standard)[1];
            Assert.AreEqual(segment1.Value, segment2.Value);
        }

        [TestMethod]
        public void Segment_ReportsCorrectType()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            Assert.AreEqual("MSH", message[1].Type, @"Segment didn't report correct type.");
        }

        [TestMethod]
        public void Segment_CanBeCloned()
        {
            var segment = Message.Parse(ExampleMessages.Standard)[1];
            var clone = segment.Clone();
            Assert.AreNotSame(segment, clone, "Cloned segment is the same referenced object.");
            Assert.AreEqual(segment.Value, clone.Value, "Cloned segment has different contents.");
        }

        [TestMethod]
        public void Segment_CanAddDescendantsAtEnd()
        {
            var segment = Message.Parse(ExampleMessages.Standard)[2];
            var fieldCount = segment.ValueCount;
            var id = Randomized.String();
            segment[fieldCount].Value = id;
            Assert.AreEqual(fieldCount + 1, segment.ValueCount,
                @"Number of elements after appending at the end is incorrect.");
        }

        [TestMethod]
        public void Segment_Timely_AddsHighIndexField()
        {
            var testString = Randomized.String();
            var message = Message.Parse();
            var segment = message[1];
            var time = Measure.ExecutionTime(() => { segment[HighIndex].Value = testString; });
            Assert.AreEqual(segment[HighIndex], testString);
            AssertTime.IsWithin(1000, time);
        }

        [TestMethod]
        public void Segment_Timely_PopulatesFields()
        {
            var message = Message.Parse();
            var time = Measure.ExecutionTime(() =>
            {
                var msh = message[1];
                for (var i = 3; i <= 1000; i++)
                {
                    var id = Randomized.String();
                    msh[i].Value = id;
                    Assert.AreEqual(msh[i].Value, id);
                }
            });
            AssertTime.IsWithin(2000, time);
        }

        [TestMethod]
        public void Segment_Timely_ReadsLastFieldInLargeSegment()
        {
            var message = Message.Parse();
            string field = null;
            string value = Randomized.String();
            message[1][HighIndex].Value = value;
            var time = Measure.ExecutionTime(() => { field = message[1][HighIndex].Value; });
            Assert.AreEqual(value, field);
            AssertTime.IsWithin(1000, time);
        }

        [TestMethod]
        public void Segment_Timely_ReadsFirstFieldInLargeSegment()
        {
            var message = Message.Parse();
            string field = null;
            message[1][HighIndex].Value = Randomized.String();
            var time = Measure.ExecutionTime(() => { field = message[1][3].Value; });
            Assert.AreEqual(null, field);
            AssertTime.IsWithin(500, time);
        }

        [TestMethod]
        public void Segment_Timely_ReadsTwoFieldsInLargeSegment()
        {
            var message = Message.Parse();
            string field = null;
            message[1][HighIndex].Value = Randomized.String();
            var time = Measure.ExecutionTime(() =>
            {
                field = message[1][HighIndex - 1].Value;
                field = message[1][3].Value;
            });
            Assert.AreEqual(null, field);
            AssertTime.IsWithin(500, time);
        }

        [TestMethod]
        public void Segment_Timely_ModifiesFirstFieldInLargeSegment()
        {
            var message = Message.Parse();
            message[1][HighIndex].Value = "test";
            var time = Measure.ExecutionTime(() => { message[1][3].Value = "test2"; });
            AssertTime.IsWithin(500, time);
        }

        [TestMethod]
        public void Segment_CanGetFieldsByIndexer()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var segment = message[2];
            var field = segment[2];
            Assert.AreEqual(@"20130528073829", field.Value);
        }

        [TestMethod]
        public void Segment_CanDeleteField()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var segment = message[1];
            var field3 = segment[3].Value;
            var field5 = segment[5].Value;
            var field6 = segment[6].Value;
            segment.Delete(4);
            Assert.AreEqual(field3, segment[3].Value, @"Expected segment[3] to remain the same after delete.");
            Assert.AreEqual(field5, segment[4].Value, @"Expected segment[5] to become segment[4].");
            Assert.AreEqual(field6, segment[5].Value, @"Expected segment[6] to become segment[5].");
        }

        [TestMethod]
        public void Segment_CanDeleteFieldsViaLinq()
        {
            var message = Message.Parse("MSH|^~\\&|1|2|3|4|5");
            var segment = message[1];
            segment.DescendantElements.Skip(2).Where(i => i.As.Int%2 == 0).Delete();
            Assert.AreEqual("MSH|^~\\&|1|3|5", message.Value, @"Message was modified unexpectedly.");
        }

        [TestMethod]
        public void Segment_WillPointToCorrectFieldValue_WhenFieldsChange()
        {
            var message = Message.Parse();
            var msh3 = message[1][3];
            var msh4 = message[1][4];
            var expected = Randomized.String().Substring(0, 5);

            msh4.Value = expected;
            msh3.Value = Randomized.String();
            Assert.AreEqual(msh4.Value, expected);
        }

        [TestMethod]
        public void Segment_WithSignificantDescendants_ShouldClaimToHaveSignificantDescendants()
        {
            var message = Message.Parse();
            Assert.IsTrue(message[1].HasSignificantDescendants,
                @"Segment claims to not have descendants when it should.");
        }

        [TestMethod]
        public void Segment_WillConsiderNonPresentValuesToNotExist()
        {
            var message = Message.Parse();
            Assert.IsFalse(message[2].Exists, @"Nonexistant segment is marked as existing.");
        }

        [TestMethod]
        public void Segment_CanWriteStringValue()
        {
            var segment = Message.Parse(ExampleMessages.Standard)[2];
            var value = Randomized.String();
            segment.Value = value;
            Assert.AreEqual(value, segment.Value, "Value mismatch after write.");
        }

        [TestMethod]
        public void Segment_CanWriteNullValue()
        {
            var segment = Message.Parse(ExampleMessages.Standard)[2];
            var value = Randomized.String();
            segment.Value = value;
            segment.Value = null;
            Assert.IsNull(segment.Value, "Value mismatch after write.");
        }
    }
}